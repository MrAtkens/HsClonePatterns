using System.Collections;
using System.Collections.Generic;
using Models;
using Models.Enums;
using Models.Observer;
using Models.Weapons;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Game
{
    public List<Card> EnemyDeck, PlayerDeck;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();

    }
    // Инициализация карт в колодах
    List<Card> GiveDeckCard()
    {
        List<Card> list = new List<Card>();

        list.Add(CardManager.AllCards[6].GetCardCopy());

        for (int i = 0; i < 20; i++)
        {
            var card = CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)];

            if (card.IsSpell)
                list.Add(((SpellCard)card).GetCardCopy());
            else
                list.Add(card.GetCardCopy());
        }
        return list;
    }
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Game CurrentGame;
    // Поля рук игроков
    public Transform EnemyHand, PlayerHand, EnemyField, PlayerField;
    public GameObject CardPref;
    private int Turn, TurnTime = 30;

    public Player Player, Enemy;
    public AI EnemyAI;
    
    public List<CardController> PlayerHandCards = new List<CardController>(),
                          PlayerFieldCards = new List<CardController>(),
                          EnemyHandCards = new List<CardController>(),
                          EnemyFieldCards = new List<CardController>();
    
    public bool IsPlayerTurn => Turn % 2 == 0;

    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    private void Start()
    {
        StartGame();
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        foreach (var card in PlayerHandCards)
            Destroy(card.gameObject);
        foreach (var card in PlayerFieldCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyHandCards)
            Destroy(card.gameObject);
        foreach (var card in EnemyFieldCards)
            Destroy(card.gameObject);
        
        PlayerHandCards.Clear();
        PlayerFieldCards.Clear();
        EnemyHandCards.Clear();
        EnemyFieldCards.Clear();

        StartGame();
    }

    void StartGame()
    {
        Turn = 0;
        
        CurrentGame = new Game();
        //Выдача карт
        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand);
        Instance.Player.RestoreMana();
        Instance.Enemy.RestoreMana();
        Instance.Player.HealthRestore();
        Instance.Enemy.HealthRestore();


        UIController.Instance.StartGame();
        //Карутина игры
        StartCoroutine(TurnFunc());
    }
    
    public void CheckForResult()
    {
        if (Instance.Enemy.GetHealth() == 0 || Instance.Player.GetHealth() == 0)
        {
            StopAllCoroutines();
            UIController.Instance.ShowResult();
        }
    }

    private void CreateCardPref(Card card, Transform hand)
    {
        var cardGameObject = Instantiate(CardPref, hand, false);
        var cardC = cardGameObject.GetComponent<CardController>();
        cardC.Init(card, hand == PlayerHand);
        if(cardC.isPlayerCard)
            PlayerHandCards.Add(cardC);
        else
            EnemyHandCards.Add(cardC);
    }
    
    //Выдача карт в начале игры
    private void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while(i++ < 4)
            GiveCardToHand(deck, hand);
    }
    //Простая выдача карт в каждый ход
    private void GiveCardToHand(List<Card> deck, Transform hand)
    {
        if (deck.Count == 0)
            return;
        
        CreateCardPref(deck[0], hand);

        deck.RemoveAt(0);
    }
    
    // Выдача карт на новом ходе
    private void GiveNewCards()
    {
        GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
        GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
    }

    //Подсветка противника
    public void HighlightTargets(CardController attacker, bool highlight)
    {
        var targets = new List<CardController>();

        if (attacker.Card.IsSpell)
        {
            var spellTarget = (SpellCard) attacker.Card;
            if (spellTarget.SpellTarget == TargetType.NO_TARGET)
                targets = new List<CardController>();
            else if (spellTarget.SpellTarget == TargetType.ALLY_CARD_TARGET)
                targets = PlayerFieldCards;
            else
                targets = EnemyFieldCards;
        }
        else
        {
            //Подсветка если есть провокаций 
            if (EnemyFieldCards.Exists(x => x.Card.IsProvocation))
                targets = EnemyFieldCards.FindAll(x => x.Card.IsProvocation);
            else
            {
                targets = EnemyFieldCards;
                Enemy.HighlightAsTarget(highlight);
            }
        }

        foreach (var card in targets)
        {
            if(attacker.Card.IsSpell)
                card.Info.HighlightAsSpellTarget(highlight);
            else
                card.Info.HighlightAsTarget(highlight);
        }
    }
    // Сражение между картами
    public void CardsFight(CardController attacker, CardController defender)
    {
        GetWeapon(attacker);
        //нанесение урона по карте игрока и противника тоже
        defender.Card.GetDamage(attacker.Card.GetAttackForDamage());
        attacker.OnDamageDeal();
        defender.OnTakeDamage(attacker);
        
        attacker.Card.GetDamage(defender.Card.GetAttackForDamage());
        attacker.OnTakeDamage();
        //проверка на то жива ли карта после сражения
        attacker.CheckForAlive();
        defender.CheckForAlive();
    }
    
    
    //Нанесение урона герою
    public void DamageHero(CardController card, bool isEnemyHero)
    {
        GetWeapon(card);
        //Атака с использованием оружия и проверка на то чья карта сделала удар
        if (isEnemyHero)
            Enemy.ApplyDamage(card.Card.GetAttackForDamage());
        else
            Player.ApplyDamage(card.Card.GetAttackForDamage());
        
        card.OnDamageDeal();
        CheckForResult();
    }
    
    
    //Подсветка карт которые нельзя поставить из за нехватки маны
    public void CheckCardsForManaAvailability()
    {
        foreach (var card in PlayerHandCards)
            card.Info.HighlightManaAvailability(Player.GetMana());
    }

    //Отсчёт времени у таймера хода
    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        UIController.Instance.UpdateTurnTime(TurnTime);

        foreach (var card in PlayerFieldCards)
            card.Info.HighlightCard(false);

        CheckCardsForManaAvailability();

        if (IsPlayerTurn)
        {
            //Активируем все карты игрока и способности тоже и включаем подстветку карт
            foreach (var card in PlayerFieldCards)
            {
                card.Card.CanAttack = true;
                card.Info.HighlightCard(true);
                card.Ability.OnNewTurn();
            }

            while (TurnTime-- > 0)
            {
                UIController.Instance.UpdateTurnTime(TurnTime);
                yield return new WaitForSeconds(1);
            }

            ChangeTurn();
        }
        else
        {
            //Способности на новом ходу и даём возможность картам атаковать на след ход
            foreach (var card in EnemyFieldCards)
            {
                card.Card.CanAttack = true;
                card.Ability.OnNewTurn();
            }
            //Противник делает ход
            EnemyAI.MakeTurn();

            while (TurnTime-- > 0)
            {
                UIController.Instance.UpdateTurnTime(TurnTime);
                yield return new WaitForSeconds(1);
            }
            //Смена хода
            ChangeTurn();
        }
    }
	
    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;

        UIController.Instance.DisableTurnBtn();

        if (IsPlayerTurn)
        {
            GiveNewCards();

            Instance.Player.IncreaseManaPool();
            Instance.Player.RestoreMana();
        }
        else
        {
            Instance.Enemy.IncreaseManaPool();
            Instance.Enemy.RestoreMana();
        }

        StartCoroutine(TurnFunc());
    }
    
    //Выдача оружия если своё сломалось
    private void GetWeapon(CardController card)
    {
        var random = Random.Range(0, 100);
        Debug.Log("Durability " + card.Card.GetDurability());
        if (card.Card.GetDurability() == 0)
        {
            Debug.Log("Random " + random);
            if(random < 25)
                card.Card.SetWeapon(new Axe(2, 2));
            else if(random > 25 && random < 50)
                card.Card.SetWeapon(new Mace(2, 3));
            else if (random > 50 && random < 75)
                card.Card.SetWeapon(new Sword(3, 1));
            else if (random > 75)
                card.Card.SetWeapon(new Arm());
            card.Info.RefreshData();
            Debug.Log(card.Card.GetWeaponDamage());
        }
    }
}
