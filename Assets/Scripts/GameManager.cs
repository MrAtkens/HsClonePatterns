using System.Collections;
using System.Collections.Generic;
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
        var list = new List<Card>();
        for(var i=0; i < 10; i++)
            list.Add(CardManager.AllCards[Random.Range(0, CardManager.AllCards.Count)]);
        return list;
    }
}
public class GameManager : MonoBehaviour
{
    public Game CurrentGame;
    // Поля рук игроков
    public Transform EnemyHand, PlayerHand, EnemyField, PlayerField;
    public GameObject CardPref;
    private int Turn, TurnTime = 30;
    public TextMeshProUGUI TurnTimeText;
    public Button EndTurnButton;

    public int PlayerMana = 10, EnemyMana = 10;
    public TextMeshProUGUI PlayerManaText, EnemyManaText;

    public int PlayerHP, EnemyHP;
    public TextMeshProUGUI PlayerHpText, EnemyHpText;

    public GameObject ResultGameObject;
    public TextMeshProUGUI ResultText;

    public List<CardInfo> PlayerHandCards = new List<CardInfo>(),
                          PlayerFieldCards = new List<CardInfo>(),
                          EnemyHandCards = new List<CardInfo>(),
                          EnemyFieldCards = new List<CardInfo>();
    
    public bool IsPlayerTurn => Turn % 2 == 0;

    private void Start()
    {
        Turn = 0;
        // инициализация игры
        CurrentGame = new Game();
        //Выдача карт противнику
        GiveHandCards(CurrentGame.EnemyDeck, EnemyHand);
        //Выдача карт игроку
        GiveHandCards(CurrentGame.PlayerDeck, PlayerHand);

        PlayerHP = EnemyHP = 30;

        //Показать количество маны
        ShowMana();

        //Начать ходы
        StartCoroutine(TurnFunc());
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

        Card card = deck[0];
        GameObject cardGameObject = Instantiate(CardPref, hand, false);

        if (hand == EnemyHand)
        {
            cardGameObject.GetComponent<CardInfo>().HideCardInfo(card);
            EnemyHandCards.Add(cardGameObject.GetComponent<CardInfo>());
        }
        else
        {
            cardGameObject.GetComponent<CardInfo>().ShowCardInfo(card, true);
            PlayerHandCards.Add(cardGameObject.GetComponent<CardInfo>());
            //Игрок не может атаковать сам себя
            cardGameObject.GetComponent<Attack>().enabled = false;
        }

        deck.RemoveAt(0);
    }

    //Смена хода
    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;
        EndTurnButton.interactable = IsPlayerTurn;
        //Выдача новых карт в конце хода
        if (IsPlayerTurn) 
            GiveNewCards();
        //Отсчёт времени у таймера хода
        StartCoroutine(TurnFunc());
    }
    // Выдача карт на новом ходе
    private void GiveNewCards()
    {
        GiveCardToHand(CurrentGame.EnemyDeck, EnemyHand);
        GiveCardToHand(CurrentGame.PlayerDeck, PlayerHand);
    }
    
    private void EnemyTurn(List<CardInfo> cards)
    {
        //Количества карт противника которое он будет ходить 
        var count = Random.Range(0, cards.Count+1);

        //цикл по выставлению карт на поле
        for (var i = 0; i < count; i++)
        {
            if (EnemyFieldCards.Count > 5 || EnemyMana == 0)
                return;

            List<CardInfo> cardList = cards.FindAll(x => EnemyMana >= x.SelfCard.ManaCost);

            if (cardList.Count == 0)
                break;

            ReduceMana(false, cardList[0].SelfCard.ManaCost);

            cardList[0].ShowCardInfo(cardList[0].SelfCard, false);
            cardList[0].transform.SetParent(EnemyField);
            
            EnemyFieldCards.Add(cardList[0]);
            EnemyHandCards.Remove(cardList[0]);
        }
        // не атакует потому что нужно выставить canAttack
        foreach (var activeCard in EnemyFieldCards.FindAll(x => x.SelfCard.CanAttack))
        {
            if(Random.Range(0, 2) == 0 &&
                PlayerFieldCards.Count > 0)
            {
                var enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];

                activeCard.SelfCard.ChangeAttackState(false);
                CardsFight(enemy, activeCard);
            }
            else
            {
                activeCard.SelfCard.ChangeAttackState(false);
                DamageHero(activeCard, false);
            }
        }
    }

    public void CardsFight(CardInfo playerCard, CardInfo enemyCard)
    {
        //нанесение урона по карте игрока и противника тоже
        playerCard.SelfCard.GetDamage(enemyCard.SelfCard.Attack);
        enemyCard.SelfCard.GetDamage(playerCard.SelfCard.Attack);
        
        //Проверка на то жива ли карта игрока, если жива то обновляем данные
         if(!playerCard.SelfCard.IsAlive)
            DestroyCard(playerCard);
        else
            playerCard.RefreshData();
        //Проверка на то жива ли карта противника
        if(!enemyCard.SelfCard.IsAlive)
            DestroyCard(enemyCard);
        else
            enemyCard.RefreshData();
    }

    void DestroyCard(CardInfo card)
    {
        card.GetComponent<CardMovement>().OnEndDrag(null);
        
        if (EnemyFieldCards.Exists(x => x == card))
            EnemyFieldCards.Remove(card);
        
        if (PlayerFieldCards.Exists(x => x == card))
            PlayerFieldCards.Remove(card);
        
        Destroy(card.gameObject);
    }
    
    private void ShowMana()
    {
        PlayerManaText.text = PlayerMana.ToString();
        EnemyManaText.text = EnemyMana.ToString();
    }
    private void ShowHP()
    {
        EnemyHpText.text = EnemyHP.ToString();
        PlayerHpText.text = PlayerHP.ToString();
    }

    //Функция уменьшение маны
    public void ReduceMana(bool playerMana, int manacost)
    {
        // Проверка на то уменьшаем ли мы ману именно у игрока
        if (playerMana)
            PlayerMana = Mathf.Clamp(PlayerMana - manacost, 0, int.MaxValue);
        else
            EnemyMana = Mathf.Clamp(EnemyMana - manacost, 0, int.MaxValue);
        //Обновляем количество маны у игроков
        ShowMana();
    }


    public void DamageHero(CardInfo card, bool isEnemyHero)
    {
        if (isEnemyHero)
            EnemyHP = Mathf.Clamp(EnemyHP - card.SelfCard.Attack, 0, int.MaxValue);
        else
            PlayerHP = Mathf.Clamp(PlayerHP - card.SelfCard.Attack, 0, int.MaxValue);

        ShowHP();
        card.HighliteOff();
        CheckGameResult();
    }

    //Отсчёт времени у таймера хода
    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeText.text = TurnTime.ToString();

        //Отключение подстветки карты
        foreach (var card in PlayerFieldCards)
            card.HighliteOff();

        if (IsPlayerTurn)
        {
            //Карты могут взаймодействовать с другими
            foreach (var card in PlayerFieldCards)
            {
                card.SelfCard.ChangeAttackState(true);
                card.HighliteOn();
            }

            while (TurnTime-- > 0)
            {
                //Смена счётчика таймера и мы ждём одну секунду в итирацию
                TurnTimeText.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }
        }
        else
        {
            foreach (var card in EnemyFieldCards)
            {
                card.SelfCard.ChangeAttackState(true);
            }

            while (TurnTime-- > 27)
            {
                TurnTimeText.text = TurnTime.ToString();
                yield return new WaitForSeconds(1);
            }
            
            if(EnemyHandCards.Count > 0)
                EnemyTurn(EnemyHandCards);
        }
        //Смена хода
        ChangeTurn();
    }

    private void CheckGameResult()
    {
        if(EnemyHP == 0 || PlayerHP == 0)
        {
            ResultGameObject.SetActive(true);
            StopAllCoroutines();

            if (EnemyHP == 0)
                ResultText.text = "You win";
            else
                ResultText.text = "-25";
        }
    }
}
