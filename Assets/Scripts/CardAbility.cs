using Models;
using Models.Decorator;
using Models.Enums;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    public CardController cardController;
    public GameObject Shield, Provocation;

    public void OnCast()
    {
        foreach (var ability in cardController.Card.GetAbility())
        {
            switch (ability)
            {
                case (int)AbilityType.INSTANT_ACTIVE:
                    cardController.Card.CanAttack = true;
                    if(cardController.isPlayerCard)
                        cardController.Info.HighlightCard(true);
                    break;
                case (int)AbilityType.SHIELD:
                    Shield.SetActive(true);
                    break;
                case (int)AbilityType.PROVOCATION:
                    Provocation.SetActive(true);
                    break;
            }
        }
    }

    public void OnDamageDeal()
    {
        foreach (var ability in cardController.Card.GetAbility())
        {
            switch (ability)
            {
                case (int)AbilityType.DOUBLE_ATTACK:
                    if (cardController.Card.TimesDealDamage == 1)
                    {
                        cardController.Card.CanAttack = true;
                        if(cardController.isPlayerCard)
                            cardController.Info.HighlightCard(true);
                    }
                    break;
            }
        }
    }

    public void OnTookDamage(CardController attacker = null)
    {
        Shield.SetActive(false);
        foreach (var ability in cardController.Card.GetAbility())
        {
            switch (ability)
            {
                case (int)AbilityType.SHIELD:
                    Shield.SetActive(true);
                    break;
                case (int)AbilityType.COUNTER_ATTACK:
                    if(attacker != null)
                        attacker.Card.GetDamage(cardController.Card.GetAttackForDamage());
                    break;
            }
        }
    }

    public void OnNewTurn()
    {
        cardController.Card.TimesDealDamage = 0;
        foreach (var ability in cardController.Card.GetAbility())
        {
            switch (ability)
            {
                case (int)AbilityType.REGENERATION_EACH_TURN:
                    cardController.Card.SetStat(new SpellEffect(cardController.Card.GetStat(StatType.HEALTH), SpellEffectType.ADD, 2).GetEffect());
                    cardController.Info.RefreshData();
                    break;
            }
        }
    }
}