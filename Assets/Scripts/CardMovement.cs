using UnityEngine;
using UnityEngine.EventSystems;

public class CardLogic : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Игровая камера, отступ, класс перемещения карты и карты буфера, также сама карта буфер
    Camera MainCamera;
    Vector3 offset;
    //Поля на которых находятся карты
    public Transform DefaultParent, DefaultBufferCard;
    GameObject BufferCard;

    void Awake()
    {
        //Инициализация карты буфера и нашей единственной камеры
        MainCamera = Camera.allCameras[0];
        BufferCard = GameObject.Find("BufferCard");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Выставляем отступ во время перетягивание и присваеваем перетягивание к нашим картам
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        DefaultParent = DefaultBufferCard = transform.parent;

        // Выставляем местоположение карты на игровом поле
        BufferCard.transform.SetParent(DefaultParent);
        BufferCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent);
        //Нужно для возможность включение взаймодействия с картой на canvas который стал рукой игрока 
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Перемещение карты по столу
        Vector3 newPosition = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPosition + offset;

        if (BufferCard.transform.parent != DefaultBufferCard)
            BufferCard.transform.SetParent(DefaultBufferCard);

        //Определение позиций карты относительно других карт
        CheckPoisition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //Взаймодействие с временной картой
        transform.SetSiblingIndex(BufferCard.transform.GetSiblingIndex());
        BufferCard.transform.SetParent(GameObject.Find("Canvas").transform);
        BufferCard.transform.localPosition = new Vector3(2538, 200); 
    }


    void CheckPoisition()
    {
        int newIndex = DefaultBufferCard.childCount;
        // Здесь мы исходим из логики того что если карта которую мы хотим переместить по индексу меньше то она должна быть слева
        for(int i = 0; i < DefaultBufferCard.childCount; i++)
        {
            if (transform.position.x < DefaultBufferCard.GetChild(i).position.x)
            {
                newIndex = i;
                if (BufferCard.transform.GetSiblingIndex() < newIndex)
                    newIndex--;
                break;
            }
        }
        //Устанавливаем позицию карты
        BufferCard.transform.SetSiblingIndex(newIndex);
    }
}
