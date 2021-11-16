using System.Collections;
using System.Collections.Generic;
using Models;
using Models.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsPlayerTurn)
            return;

        CardController spell = eventData.pointerDrag.GetComponent<CardController>(),
                       target = GetComponent<CardController>();

        if (spell &&
            spell.Card.IsSpell &&
            spell.isPlayerCard &&
            target.Card.IsPlaced &&
            GameManager.Instance.Player.GetMana() >= spell.Card.ManaCost)
        {
            var spellCard = (SpellCard) spell.Card;
            if ((spellCard.SpellTarget != TargetType.ALLY_CARD_TARGET || !target.isPlayerCard) &&
                (spellCard.SpellTarget != TargetType.ENEMY_CARD_TARGET || target.isPlayerCard)) return;
            GameManager.Instance.Player.ReduceMana(spell.Card.ManaCost);
            spell.UseSpell(target);
            GameManager.Instance.CheckCardsForManaAvailability();
        }
    }

}