using System.Collections.Generic;
using Models;
using Models.Decorator;
using Models.Enums;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public AbstractCard Card;
    public bool isPlayerCard;
    public CardInfo Info;
    public CardMovement Movement;
    public CardAbility Ability;

    private GameManager gameManager;

    public void Init(AbstractCard card, bool isPlayer)
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
            Info.ShowCardInfo();
    }

    public void OnCast()
    {
        if (Card.IsSpell() && Card.GetTargetType() != (int)TargetType.NO_TARGET)
            return;
        
        if (isPlayerCard)
        {
            gameManager.PlayerHandCards.Remove(this);
            gameManager.PlayerFieldCards.Add(this);
            gameManager.Player.ReduceMana(Card.ManaCost.Value);
            gameManager.CheckCardsForManaAvailability();
        }
        else
        {
            gameManager.EnemyHandCards.Remove(this);
            gameManager.EnemyFieldCards.Add(this);
            gameManager.Enemy.ReduceMana(Card.ManaCost.Value);
            Info.ShowCardInfo();
        }

        Card.IsPlaced = true;
        
        if(Card.HasAbility())
            Ability.OnCast();
        
        if(Card.IsSpell())
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
        
        if(Card.HasAbility())
            Ability.OnDamageDeal();
    }

    public void UseSpell(CardController target)
    {
        var spellCard = Card;
        switch (spellCard.GetAbility()[0])
        {
            case (int)SpellType.HEAL_ALLY_FIELD_CARDS:
                var allyCards = isPlayerCard ? gameManager.PlayerFieldCards : gameManager.EnemyFieldCards;
                foreach (var card in allyCards)
                {
                    card.Card.SetStat(new SpellEffect(card.Card.GetStat(StatType.HEALTH), SpellEffectType.ADD,
                        spellCard.GetStat(StatType.SPELL_VALUE).Value).GetEffect());
                    card.Info.RefreshData();
                }
                break;
            case (int)SpellType.DAMAGE_ENEMY_FIELD_CARDS:
                var enemyCards = isPlayerCard
                    ? new List<CardController>(gameManager.EnemyFieldCards)
                    : new List<CardController>(gameManager.PlayerFieldCards);
                foreach (var card in enemyCards)
                    GiveDamageTo(card, spellCard.GetStat(StatType.SPELL_VALUE).Value);
                break;
            case (int)SpellType.HEAL_ALLY_HERO:
                
                if(isPlayerCard)
                    gameManager.Player.Heal(spellCard.GetStat(StatType.SPELL_VALUE).Value);
                else 
                    gameManager.Enemy.Heal(spellCard.GetStat(StatType.SPELL_VALUE).Value);
                break;
            case (int)SpellType.DAMAGE_ENEMY_HERO:
                if(isPlayerCard)
                    gameManager.Enemy.ApplyDamage(spellCard.GetStat(StatType.SPELL_VALUE).Value);
                else 
                    gameManager.Player.ApplyDamage(spellCard.GetStat(StatType.SPELL_VALUE).Value);
                break;
            case (int)SpellType.HEAL_ALLY_CARD:
                target.Card.SetStat(new SpellEffect(target.Card.GetStat(StatType.HEALTH), SpellEffectType.ADD,
                    spellCard.GetStat(StatType.SPELL_VALUE).Value).GetEffect());
                break;
            case (int)SpellType.DAMAGE_ENEMY_CARD:
                GiveDamageTo(target, spellCard.GetStat(StatType.SPELL_VALUE).Value);
                break;
            case (int)SpellType.SHIELD_ON_ALLY_CARD:
                if(!target.Card.GetAbility().Exists(x => x == (int)AbilityType.SHIELD))
                    target.Card.AddAbility((int)AbilityType.SHIELD);
                break;
            case (int)SpellType.PROVOCATION_ON_ALLY_CARD:
                if(!target.Card.GetAbility().Exists(x => x == (int)AbilityType.PROVOCATION))
                    target.Card.AddAbility((int)AbilityType.PROVOCATION);
                break;
            case (int)SpellType.BUFF_CARD_DAMAGE:
                target.Card.SetStat(new SpellEffect(target.Card.GetStat(StatType.ATTACK), SpellEffectType.ADD,
                    spellCard.GetStat(StatType.SPELL_VALUE).Value).GetEffect());
                break;
            case (int)SpellType.DEBUFF_CARD_DAMAGE:
                target.Card.SetStat(new SpellEffect(target.Card.GetStat(StatType.ATTACK), SpellEffectType.SUBSTRACT, spellCard.GetStat(StatType.SPELL_VALUE).Value).GetEffect());
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
        //Is Alive
        if(Card.GetStat(StatType.HEALTH).Value != 0)
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

    public bool IsProvocation()
    {
        return Card.GetAbility().Contains((int) AbilityType.PROVOCATION);
    }
    
    private void RemoveCardFromList(List<CardController> list)
    {
        if (list.Exists(x => x == this))
            list.Remove(this);
    }
    
}