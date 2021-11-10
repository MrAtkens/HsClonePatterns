using UnityEngine;
using UnityEngine.EventSystems;

public enum FieldType
{
    SELF_HAND,
    SELF_FIELD,
    ENEMY_HAND,
    ENEMY_FIELD
}
public class FieldLogic : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public FieldType Type;
    public void OnDrop(PointerEventData eventData)
    {
        if (Type != FieldType.SELF_FIELD)
            return;
        
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        //Выставление карты на стол если данный объект является картой
        if (card && card.GameManager.PlayerFieldCards.Count < 6 && card.GameManager.IsPlayerTurn &&
            card.GameManager.PlayerMana >= card.GetComponent<CardInfo>().SelfCard.ManaCost)
        {
            //Добавление карты в общий список на поле игрока
            card.GameManager.PlayerHandCards.Remove(card.GetComponent<CardInfo>());
            card.GameManager.PlayerFieldCards.Add(card.GetComponent<CardInfo>());
            card.DefaultParent = transform;

            card.GameManager.ReduceMana(true, card.GetComponent<CardInfo>().SelfCard.ManaCost);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Проверка на перетягивание карты если это не поле или это поле противника поставить карту нельзя 
        if (eventData.pointerDrag == null || Type == FieldType.ENEMY_FIELD || Type == FieldType.ENEMY_HAND || Type == FieldType.SELF_HAND)
            return;
        //данное поле становится родителем для карты
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        if (card)
            card.DefaultBufferCard = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Проверка на перетягивание карты
        if (eventData.pointerDrag == null)
            return;

        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        if (card && card.DefaultBufferCard == transform)
            card.DefaultBufferCard = card.DefaultParent;
    }
}