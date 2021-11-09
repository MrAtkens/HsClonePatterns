using System.Collections.Generic;
using UnityEngine;

//Структура потому что это значимый тип и мы сможем передавать все данные а не ссылку
public struct Card {
    public string Name;
    public string Description;
    public Sprite Logo;
    public int Attack, Health;
    public bool CanAttack;

    public bool IsAlive => Health > 0;

    public Card(string name, string description, string logoPath, int attack, int health)
    {
        Name = name;
        Description = description;
        Logo = Resources.Load<Sprite>(logoPath);
        Attack = attack;
        Health = health;
        CanAttack = false;
    }

    public void ChangeAttackState(bool can)
    {
        CanAttack = can;
    }

    public void GetDamage(int damage)
    {
        Health -= damage;
    }
}

public static class CardManager
{
    // место где хранятся карты
    public static List<Card> AllCards = new List<Card>();
}

public class CardManagerSrc : MonoBehaviour
{
    public void Awake()
    {
        // Инициализация карт
        CardManager.AllCards.Add(new Card("Litso", "Лицо", "Sprites/Cards/litso", 5, 5));
        CardManager.AllCards.Add(new Card("Buldiga","Лицо", "Sprites/Cards/buldiga", 2, 5));
        CardManager.AllCards.Add(new Card("Pominki", "Лицо", "Sprites/Cards/pominki", 10, 2));
        CardManager.AllCards.Add(new Card("Hmm", "Лицо", "Sprites/Cards/hmm", 1, 4));
        CardManager.AllCards.Add(new Card("Micro", "Лицо", "Sprites/Cards/micro", 2, 2));
        CardManager.AllCards.Add(new Card("Pomoika", "Лицо", "Sprites/Cards/pomoika", 1, 1));
        CardManager.AllCards.Add(new Card("Pomoika", "Лицо", "Sprites/Cards/pomoika", 1, 1));

    }
}
