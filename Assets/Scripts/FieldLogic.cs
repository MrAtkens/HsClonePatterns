using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class FieldLogic : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            CardLogic card = eventData.pointerDrag.GetComponent<CardLogic>();
            //Выставление карты на стол если данный объект является картой
            if (card)
                card.DefaultParent = transform;

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Проверка на перетягивание карты
            if (eventData.pointerDrag == null)
                return;
            //данное поле становится родителем для карты
            CardLogic card = eventData.pointerDrag.GetComponent<CardLogic>();
            if (card)
                card.DefaultBufferCard = transform;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Проверка на перетягивание карты
            if (eventData == null)
                return;

            CardLogic card = eventData.pointerDrag.GetComponent<CardLogic>();
            if (card && card.DefaultBufferCard == transform)
                card.DefaultBufferCard = card.DefaultParent;
        }
    }
}
