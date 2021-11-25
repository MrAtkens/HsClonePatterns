using System.Collections.Generic;
using Models;
using Models.Enums;
using Models.FactoryMethod;
using UnityEngine;

public static class CardManager
{
    // место где хранятся карты
    public static readonly List<AbstractCard> AllCards = new List<AbstractCard>();
}

public class CardManagerSrc : MonoBehaviour
{
    public void Awake()
    {
        Creator creator = new CreatureCreator();
        // Инициализация карт
        CardManager.AllCards.Add(creator.CreateCard("Bull", "","Sprites/Cards/bull", 1, 1, 1));
        CardManager.AllCards.Add(creator.CreateCard("Elephant", "","Sprites/Cards/elephant", 4, 3 ,5));
        CardManager.AllCards.Add(creator.CreateCard("Hyena", "","Sprites/Cards/hyena", 3, 3, 4));
        CardManager.AllCards.Add(creator.CreateCard("Tiger", "","Sprites/Cards/tiger", 5, 5, 2));
        CardManager.AllCards.Add(creator.CreateCard("Lion", "","Sprites/Cards/lion", 10, 10, 7));
        
        CardManager.AllCards.Add(creator.CreateCard("Provocation", "","Sprites/Cards/provocation", 1, 2, 3, AbilityType.PROVOCATION));
        CardManager.AllCards.Add(creator.CreateCard("Regeneration", "","Sprites/Cards/regen", 5, 5 ,4, AbilityType.REGENERATION_EACH_TURN));
        CardManager.AllCards.Add(creator.CreateCard("DoubleAttack", "","Sprites/Cards/doubleAttack", 2, 3, 2, AbilityType.DOUBLE_ATTACK));
        CardManager.AllCards.Add(creator.CreateCard("InstantActive", "","Sprites/Cards/instantActive", 2, 1 ,2, AbilityType.INSTANT_ACTIVE));
        CardManager.AllCards.Add(creator.CreateCard("Shield", "","Sprites/Cards/shield", 5, 1 ,5, AbilityType.SHIELD));
        CardManager.AllCards.Add(creator.CreateCard("CounterAttack", "","Sprites/Cards/counterAttack", 3, 2, 1, AbilityType.COUNTER_ATTACK));
      
        creator = new SpellCreator();
        CardManager.AllCards.Add(creator.CreateCard("HEAL_ALLY_FIELD_CARDS",  "","Sprites/Cards/healAllyCards", 2, 2, TargetType.NO_TARGET, SpellType.HEAL_ALLY_FIELD_CARDS));
        CardManager.AllCards.Add(creator.CreateCard("DAMAGE_ENEMY_FIELD_CARDS", "","Sprites/Cards/damageEnemyCards", 2, 2, TargetType.NO_TARGET, SpellType.DAMAGE_ENEMY_FIELD_CARDS));
        CardManager.AllCards.Add(creator.CreateCard("HEAL_ALLY_HERO", "","Sprites/Cards/healAllyHero", 2, 2, TargetType.NO_TARGET, SpellType.HEAL_ALLY_HERO));
        CardManager.AllCards.Add(creator.CreateCard("DAMAGE_ENEMY_HERO", "","Sprites/Cards/damageEnemyHero", 2, 2, TargetType.NO_TARGET, SpellType.DAMAGE_ENEMY_HERO));
        CardManager.AllCards.Add(creator.CreateCard("HEAL_ALLY_CARD", "","Sprites/Cards/healAllyCard", 2, 2, TargetType.ALLY_CARD_TARGET, SpellType.HEAL_ALLY_CARD));
        CardManager.AllCards.Add(creator.CreateCard("DAMAGE_ENEMY_CARD","", "Sprites/Cards/damageEnemyCard", 2, 2, TargetType.ENEMY_CARD_TARGET, SpellType.DAMAGE_ENEMY_CARD));
        CardManager.AllCards.Add(creator.CreateCard("SHIELD_ON_ALLY_CARD", "","Sprites/Cards/shieldOnAllyCard", 2, 2, TargetType.ALLY_CARD_TARGET, SpellType.SHIELD_ON_ALLY_CARD));
        CardManager.AllCards.Add(creator.CreateCard("PROVOCATION_ON_ALLY_CARD", "","Sprites/Cards/provocationOnAllyCard", 2, 2, TargetType.ALLY_CARD_TARGET, SpellType.PROVOCATION_ON_ALLY_CARD));
        CardManager.AllCards.Add(creator.CreateCard("BUFF_CARD_DAMAGE", "","Sprites/Cards/buffCardDamage", 2, 2, TargetType.ALLY_CARD_TARGET, SpellType.BUFF_CARD_DAMAGE));
        CardManager.AllCards.Add(creator.CreateCard("DEBUFF_CARD_DAMAGE", "","Sprites/Cards/debuffCardDamage", 2, 2, TargetType.ENEMY_CARD_TARGET, SpellType.DEBUFF_CARD_DAMAGE));
    }
}
