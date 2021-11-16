using System.Collections.Generic;
using Models;
using Models.Enums;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card Card;
    public bool isPlayerCard;
    public CardInfo Info;
    public CardMovement Movement;
    public CardAbility Ability;

    private GameManager gameManager;

    public void Init(Card card, bool isPlayer)
    {
        Card = card;
        gameManager = GameManager.Instance;
        isPlayerCard = isPlayer;

        if (isPlayerCard)
        {
            Info.ShowCardInfo();
            GetComponent<Attack>().enabled = false;
        }
        else
            Info.HideCardInfo();
    }

    public void OnCast()
    {
        if (Card.IsSpell && ((SpellCard)Card).SpellTarget != TargetType.NO_TARGET)
            return;
        
        if (isPlayerCard)
        {
            gameManager.PlayerHandCards.Remove(this);
            gameManager.PlayerFieldCards.Add(this);
            gameManager.Player.ReduceMana(Card.ManaCost);
            gameManager.CheckCardsForManaAvailability();
        }
        else
        {
            gameManager.EnemyHandCards.Remove(this);
            gameManager.EnemyFieldCards.Add(this);
            gameManager.Enemy.ReduceMana(Card.ManaCost);
            Info.ShowCardInfo();
        }

        Card.IsPlaced = true;
        
        if(Card.HasAbility)
            Ability.OnCast();
        
        if(Card.IsSpell)
            UseSpell(null);
    }

    public void OnTakeDamage(CardController attacker = null)
    {
        CheckForAlive();   
        Ability.OnTookDamage(attacker);
    }

    public void OnDamageDeal()
    {
        Card.TimesDealDamage++;
        Card.CanAttack = false;
        Info.HighlightCard(false);
        
        if(Card.HasAbility)
            Ability.OnDamageDeal();
    }

    public void UseSpell(CardController target)
    {
        var spellCard = (SpellCard)Card;
        switch (spellCard.Spell)
        {
            case SpellType.HEAL_ALLY_FIELD_CARDS:
                var allyCards = isPlayerCard ? gameManager.PlayerFieldCards : gameManager.EnemyFieldCards;
                foreach (var card in allyCards)
                {
                    card.Card.Health += spellCard.SpellValue;
                    card.Info.RefreshData();
                }
                break;
            case SpellType.DAMAGE_ENEMY_FIELD_CARDS:
                var enemyCards = isPlayerCard
                    ? new List<CardController>(gameManager.EnemyFieldCards)
                    : new List<CardController>(gameManager.PlayerFieldCards);
                foreach (var card in enemyCards)
                    GiveDamageTo(card, spellCard.SpellValue);
                break;
            case SpellType.HEAL_ALLY_HERO:
                
                if(isPlayerCard)
                    gameManager.Player.Heal(spellCard.SpellValue);
                else 
                    gameManager.Enemy.Heal(spellCard.SpellValue);
                break;
            case SpellType.DAMAGE_ENEMY_HERO:
                if(isPlayerCard)
                    gameManager.Enemy.ApplyDamage(spellCard.SpellValue);
                else 
                    gameManager.Player.ApplyDamage(spellCard.SpellValue);
                break;
            case SpellType.HEAL_ALLY_CARD:
                target.Card.Health += spellCard.SpellValue;
                break;
            case SpellType.DAMAGE_ENEMY_CARD:
                GiveDamageTo(target, spellCard.SpellValue);
                break;
            case SpellType.SHIELD_ON_ALLY_CARD:
                if(!target.Card.Abilities.Exists(x => x == AbilityType.SHIELD))
                    target.Card.Abilities.Add(AbilityType.SHIELD);
                break;
            case SpellType.PROVOCATION_ON_ALLY_CARD:
                if(!target.Card.Abilities.Exists(x => x == AbilityType.PROVOCATION))
                    target.Card.Abilities.Add(AbilityType.PROVOCATION);
                break;
            case SpellType.BUFF_CARD_DAMAGE:
                target.Card.Attack += spellCard.SpellValue;
                break;
            case SpellType.DEBUFF_CARD_DAMAGE:
                target.Card.Attack = Mathf.Clamp(target.Card.Attack - spellCard.SpellValue, 0, int.MaxValue);
                break;
        }

        if (target != null)
        {
            target.Ability.OnCast();
            target.CheckForAlive();
        }
        
        DestroyCard();
    }

    private void GiveDamageTo(CardController card, int damage)
    {
        card.Card.GetDamage(damage);
        card.CheckForAlive();
        card.OnTakeDamage();
    }

    public void CheckForAlive()
    {
        if(Card.IsAlive)
            Info.RefreshData(); 
        else 
            DestroyCard();
    }

    public void DestroyCard()
    {
        // Убираем баг связанный с перемещением карты
        Movement.OnEndDrag(null);
        // Удаляем карту из листов 
        RemoveCardFromList(gameManager.EnemyFieldCards);
        RemoveCardFromList(gameManager.EnemyHandCards);
        RemoveCardFromList(gameManager.PlayerFieldCards);
        RemoveCardFromList(gameManager.PlayerHandCards);
        
        // Уничтожаем игровой объект в Unity
        Destroy(gameObject);
    }
    
    private void RemoveCardFromList(List<CardController> list)
    {
        if (list.Exists(x => x == this))
            list.Remove(this);
    }
}