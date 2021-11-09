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

        //Начать ходы
        StartCoroutine(TurnFunc());
    }
    //Выдача карт в начале игры
    void GiveHandCards(List<Card> deck, Transform hand)
    {
        int i = 0;
        while(i++ < 4)
            GiveCardToHand(deck, hand);
    }
    //Простая выдача карт в каждый ход
    void GiveCardToHand(List<Card> deck, Transform hand)
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
    
    void EnemyTurn(List<CardInfo> cards)
    {
        //Количества карт противника которое он будет ходить 
        var count = Random.Range(0, cards.Count+1);

        //цикл по выставлению карт на поле
        for (var i = 0; i < count; i++)
        {
            if (EnemyFieldCards.Count > 5)
                return;
            
            cards[0].ShowCardInfo(cards[0].SelfCard, false);
            cards[0].transform.SetParent(EnemyField);
            
            EnemyFieldCards.Add(cards[0]);
            EnemyHandCards.Remove(cards[0]);
        }
        // не атакует потому что нужно выставить canAttack
        foreach (var activeCard in EnemyFieldCards.FindAll(x => x.SelfCard.CanAttack))
        {
            if (PlayerFieldCards.Count == 0)
                return;

            var enemy = PlayerFieldCards[Random.Range(0, PlayerFieldCards.Count)];
            
            activeCard.SelfCard.ChangeAttackState(false);
            CardsFight(enemy, activeCard);
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
}
