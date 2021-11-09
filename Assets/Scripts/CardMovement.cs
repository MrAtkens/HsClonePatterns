using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Игровая камера, отступ, класс перемещения карты и карты буфера, также сама карта буфер
    Camera MainCamera;
    Vector3 offset;
    //Поля на которых находятся карты
    public Transform DefaultParent, DefaultBufferCard;
    private GameObject _bufferCard;
    private GameManager _gameManager;
    //Проверка на то можно ли переносить карту
    public bool IsDragble;

    private void Awake()
    {
        //Инициализация карты буфера и нашей единственной камеры
        MainCamera = Camera.allCameras[0];
        _bufferCard = GameObject.Find("BufferCard");
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Выставляем отступ во время перетягивание и присваеваем перетягивание к нашим картам
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        DefaultParent = DefaultBufferCard = transform.parent;

        // Проверяем находится ли карта в нашей руке если нет значит перенести мы её не сможем
        IsDragble = DefaultParent.GetComponent<FieldLogic>().Type == FieldType.SELF_HAND && _gameManager.isPlayerTurn;
        //Можно ли перетаскивать карту
        if (!IsDragble)
            return;

        // Выставляем местоположение карты на игровом поле
        _bufferCard.transform.SetParent(DefaultParent);
        _bufferCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent);
        //Нужно для возможность включение взаймодействия с картой на canvas который стал рукой игрока 
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Можно ли перетаскивать карту
        if (!IsDragble)
            return;
        
        //Перемещение карты по столу
        Vector3 newPosition = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPosition + offset;

        if (_bufferCard.transform.parent != DefaultBufferCard)
            _bufferCard.transform.SetParent(DefaultBufferCard);

        //Определение позиций карты относительно других карт
        CheckPoisition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Можно ли перетаскивать карту
        if (!IsDragble)
            return;
        
        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //Взаймодействие с временной картой
        transform.SetSiblingIndex(_bufferCard.transform.GetSiblingIndex());
        _bufferCard.transform.SetParent(GameObject.Find("Canvas").transform);
        _bufferCard.transform.localPosition = new Vector3(2538, 200); 
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
                if (_bufferCard.transform.GetSiblingIndex() < newIndex)
                    newIndex--;
                break;
            }
        }
        //Устанавливаем позицию карты
        _bufferCard.transform.SetSiblingIndex(newIndex);
    }
}
