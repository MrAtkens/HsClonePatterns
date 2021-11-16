using System.Collections.Generic;
using Models;
using Models.Enums;
using UnityEngine;

//Структура потому что это значимый тип и мы сможем передавать все данные а не ссылку

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
        CardManager.AllCards.Add(new Card("ebalo", "","Sprites/Cards/ebalo", 5, 5, 6));
        CardManager.AllCards.Add(new Card("buldiga", "","Sprites/Cards/buldiga", 4, 3, 5));
        CardManager.AllCards.Add(new Card("hmm", "","Sprites/Cards/hmm", 3, 3, 4));
        CardManager.AllCards.Add(new Card("micro", "","Sprites/Cards/micro", 2, 1, 2));
        CardManager.AllCards.Add(new Card("pominki", "","Sprites/Cards/pominki", 8, 1, 7));
        CardManager.AllCards.Add(new Card("pomoika", "","Sprites/Cards/pomoika", 1, 1, 1));

        CardManager.AllCards.Add(new Card("provocation", "","Sprites/Cards/provocation", 1, 2, 3, AbilityType.PROVOCATION));
        CardManager.AllCards.Add(new Card("regeneration", "","Sprites/Cards/regen", 4, 2, 5, AbilityType.REGENERATION_EACH_TURN));
        CardManager.AllCards.Add(new Card("doubleAttack", "","Sprites/Cards/doubleAttack", 3, 2, 4, AbilityType.DOUBLE_ATTACK));
        CardManager.AllCards.Add(new Card("instantActive", "","Sprites/Cards/instantActive", 2, 1, 2, AbilityType.INSTANT_ACTIVE));
        CardManager.AllCards.Add(new Card("shield", "","Sprites/Cards/shield", 5, 1, 7, AbilityType.SHIELD));
        CardManager.AllCards.Add(new Card("counterAttack", "","Sprites/Cards/counterAttack", 3, 1, 1, AbilityType.COUNTER_ATTACK));
        
        CardManager.AllCards.Add(new SpellCard("HEAL_ALLY_FIELD_CARDS",  "","Sprites/Cards/healAllyCards", 2,
            SpellType.HEAL_ALLY_FIELD_CARDS, 2, TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("DAMAGE_ENEMY_FIELD_CARDS", "","Sprites/Cards/damageEnemyCards", 2,
            SpellType.DAMAGE_ENEMY_FIELD_CARDS, 2, TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("HEAL_ALLY_HERO", "","Sprites/Cards/healAllyHero", 2,
            SpellType.HEAL_ALLY_HERO, 2, TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("DAMAGE_ENEMY_HERO", "","Sprites/Cards/damageEnemyHero", 2,
            SpellType.DAMAGE_ENEMY_HERO, 2, TargetType.NO_TARGET));
        CardManager.AllCards.Add(new SpellCard("HEAL_ALLY_CARD", "","Sprites/Cards/healAllyCard", 2,
            SpellType.HEAL_ALLY_CARD, 2, TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("DAMAGE_ENEMY_CARD","", "Sprites/Cards/damageEnemyCard", 2,
            SpellType.DAMAGE_ENEMY_CARD, 2, TargetType.ENEMY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("SHIELD_ON_ALLY_CARD", "","Sprites/Cards/shieldOnAllyCard", 2,
            SpellType.SHIELD_ON_ALLY_CARD, 0, TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("PROVOCATION_ON_ALLY_CARD", "","Sprites/Cards/provocationOnAllyCard", 2,
            SpellType.PROVOCATION_ON_ALLY_CARD, 0, TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("BUFF_CARD_DAMAGE", "","Sprites/Cards/buffCardDamage", 2,
            SpellType.BUFF_CARD_DAMAGE, 2, TargetType.ALLY_CARD_TARGET));
        CardManager.AllCards.Add(new SpellCard("DEBUFF_CARD_DAMAGE", "","Sprites/Cards/debuffCardDamage", 2,
            SpellType.DEBUFF_CARD_DAMAGE, 2, TargetType.ENEMY_CARD_TARGET));
    }
}
