using Models;
using Models.Enums;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    public CardController cardController;
    public GameObject Shield, Provocation;

    public void OnCast()
    {
        foreach (var ability in cardController.Card.Abilities)
        {
            switch (ability)
            {
                case AbilityType.INSTANT_ACTIVE:
                    cardController.Card.CanAttack = true;
                    if(cardController.isPlayerCard)
                        cardController.Info.HighlightCard(true);
                    break;
                case AbilityType.SHIELD:
                    Shield.SetActive(true);
                    break;
                case AbilityType.PROVOCATION:
                    Provocation.SetActive(true);
                    break;
            }
        }
    }

    public void OnDamageDeal()
    {
        foreach (var ability in cardController.Card.Abilities)
        {
            switch (ability)
            {
                case AbilityType.DOUBLE_ATTACK:
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
        foreach (var ability in cardController.Card.Abilities)
        {
            switch (ability)
            {
                case AbilityType.SHIELD:
                    Shield.SetActive(true);
                    break;
                case AbilityType.COUNTER_ATTACK:
                    if(attacker != null)
                        attacker.Card.GetDamage(cardController.Card.Attack);
                    break;
            }
        }
    }

    public void OnNewTurn()
    {
        cardController.Card.TimesDealDamage = 0;
        foreach (var ability in cardController.Card.Abilities)
        {
            switch (ability)
            {
                case AbilityType.REGENERATION_EACH_TURN:
                    cardController.Card.Health += 2;
                    cardController.Info.RefreshData();
                    break;
            }
        }
    }
}