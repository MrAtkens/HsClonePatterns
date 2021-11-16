using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attack : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsPlayerTurn)
            return;

        CardController attacker = eventData.pointerDrag.GetComponent<CardController>(),
            defender = GetComponent<CardController>();

        if (!attacker || !attacker.Card.CanAttack || !defender.Card.IsPlaced) return;
        if(GameManager.Instance.EnemyFieldCards.Exists(x => x.Card.IsProvocation) && !defender.Card.IsProvocation)
            return;
        GameManager.Instance.CardsFight(attacker, defender);
    }
}
