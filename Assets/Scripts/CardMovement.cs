using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //Игровая камера, отступ, класс перемещения карты и карты буфера, также сама карта буфер
    Camera MainCamera;
    Vector3 offset;
    //Поля на которых находятся карты
    public Transform DefaultParent, DefaultBufferCard;
    public GameObject BufferedCard;
    public GameManager GameManager;
    //Проверка на то можно ли переносить карту
    public bool IsDragble;

    private void Awake()
    {
        //Инициализация карты буфера и нашей единственной камеры
        MainCamera = Camera.allCameras[0];
        BufferedCard = GameObject.Find("BufferCard");
        GameManager = FindObjectOfType<GameManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Выставляем отступ во время перетягивание и присваеваем перетягивание к нашим картам
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        DefaultParent = DefaultBufferCard = transform.parent;

        // Проверяем находится ли карта в нашей руке если нет значит перенести мы её не сможем
        IsDragble = GameManager.IsPlayerTurn && 
                    (
                        (DefaultParent.GetComponent<FieldLogic>().Type == FieldType.SELF_HAND &&
                         GameManager.PlayerMana >= GetComponent<CardInfo>().SelfCard.ManaCost) ||
                        (DefaultParent.GetComponent<FieldLogic>().Type == FieldType.SELF_FIELD &&
                         GetComponent<CardInfo>().SelfCard.CanAttack)
                    );
        //Можно ли перетаскивать карту
        if (!IsDragble)
            return;

        if(GetComponent<CardInfo>().SelfCard.CanAttack)
            GameManager.HighlightTargets(true);
        
        // Выставляем местоположение карты на игровом поле
        BufferedCard.transform.SetParent(DefaultParent);
        BufferedCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

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

        if (BufferedCard.transform.parent != DefaultBufferCard)
            BufferedCard.transform.SetParent(DefaultBufferCard);

        //Определение позиций карты относительно других карт
        if(DefaultParent.GetComponent<FieldLogic>().Type != FieldType.SELF_FIELD)
            CheckPoisition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Можно ли перетаскивать карту
        if (!IsDragble)
            return;
        
        
        GameManager.HighlightTargets(false);
        
        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //Взаймодействие с временной картой
        transform.SetSiblingIndex(BufferedCard.transform.GetSiblingIndex());
        BufferedCard.transform.SetParent(GameObject.Find("Canvas").transform);
        BufferedCard.transform.localPosition = new Vector3(2538, 200); 
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
                if (BufferedCard.transform.GetSiblingIndex() < newIndex)
                    newIndex--;
                break;
            }
        }
        //Устанавливаем позицию карты
        BufferedCard.transform.SetSiblingIndex(newIndex);
    }

    //Анимация выставления карты на поле
    public void MoveToField(Transform field)
    {
        transform.SetParent(GameObject.Find("Canvas").transform);
        transform.DOMove(field.position, .5f);
    }

    //Анимация атаки
    public void MoveToTarget(Transform target)
    {
        StartCoroutine(MoveToTargetCor(target));
    }
    //Анимация атаки и возврата карты на исходное положение
    IEnumerator MoveToTargetCor(Transform target)
    {
        var pos = transform.position;
        var parent = transform.parent;
        int index = transform.GetSiblingIndex();

        //Отключаем данный компонент из за смещение карт при анимаций
        transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = false;
        
        transform.SetParent(GameObject.Find("Canvas").transform);
        //Перемещение к цели
        transform.DOMove(target.position, .25f);
        yield return new WaitForSeconds(.25f);
        //Перемещение 
        transform.DOMove(pos, .25f);
        yield return new WaitForSeconds(.25f);

        transform.SetParent(parent);
        transform.SetSiblingIndex(index);
        transform.parent.GetComponent<HorizontalLayoutGroup>().enabled = true;
    }
}
