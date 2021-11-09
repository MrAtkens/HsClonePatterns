using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Game
{
    public List<Card> EnemyDeck, EnemyHand, EnemyField, PlayerDeck, PlayerHand, PlayerField;

    public Game()
    {
        EnemyDeck = GiveDeckCard();
        PlayerDeck = GiveDeckCard();
        
        EnemyHand = new List<Card>();
        PlayerHand = new List<Card>();
        
        EnemyField = new List<Card>();
        PlayerField = new List<Card>();
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
    public Transform EnemyHand, PlayerHand;
    public GameObject CardPref;
    private int Turn, TurnTime = 30;
    public TextMeshProUGUI TurnTimeText;
    public Button EndTurnButton;

    public bool isPlayerTurn
    {
        get
        {
            return Turn % 2 == 0;
        }
    }
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
        
        if(hand == EnemyHand)
            cardGameObject.GetComponent<CardInfo>().HideCardInfo(card);
        else
            cardGameObject.GetComponent<CardInfo>().ShowCardInfo(card);
        deck.RemoveAt(0);
    }
    
    //Смена хода
    public void ChangeTurn()
    {
        StopAllCoroutines();
        Turn++;
        EndTurnButton.interactable = isPlayerTurn;
        //Выдача новых карт в конце хода
        if (isPlayerTurn) 
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
    
    //Отсчёт времени у таймера хода
    IEnumerator TurnFunc()
    {
        TurnTime = 30;
        TurnTimeText.text = TurnTime.ToString();
        if (isPlayerTurn)
        {
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
        }
        //Смена хода
        ChangeTurn();
    }
}
