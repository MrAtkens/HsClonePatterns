using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Attack : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Если сейчас не ход игрока выходим из функций
        if (!GetComponent<CardMovement>().GameManager.IsPlayerTurn)
            return;

        CardInfo card = eventData.pointerDrag.GetComponent<CardInfo>();

        // Если данная карта может атаковать 
        if (card && card.SelfCard.CanAttack &&
            transform.parent == GetComponent<CardMovement>().GameManager.EnemyField)
        {
            // Переводим состояние атаки на false
            card.SelfCard.ChangeAttackState(false);
            
            if(card.IsPlayer)
                card.HighliteOff();
            // Бой карт между друг другом
            GetComponent<CardMovement>().GameManager.CardsFight(card, GetComponent<CardInfo>());
        }
    }
}
