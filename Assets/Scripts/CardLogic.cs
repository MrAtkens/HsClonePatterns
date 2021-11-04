using UnityEngine;
using UnityEngine.EventSystems;

public class CardLogic : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //������� ������, ������, ����� ����������� ����� � ����� ������, ����� ���� ����� �����
    Camera MainCamera;
    Vector3 offset;
    //���� �� ������� ��������� �����
    public Transform DefaultParent, DefaultBufferCard;
    GameObject BufferCard;

    void Awake()
    {
        //������������� ����� ������ � ����� ������������ ������
        MainCamera = Camera.allCameras[0];
        BufferCard = GameObject.Find("BufferCard");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ���������� ������ �� ����� ������������� � ����������� ������������� � ����� ������
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        DefaultParent = DefaultBufferCard = transform.parent;

        // ���������� �������������� ����� �� ������� ����
        BufferCard.transform.SetParent(DefaultParent);
        BufferCard.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(DefaultParent.parent);
        //����� ��� ����������� ��������� �������������� � ������ �� canvas ������� ���� ����� ������ 
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //����������� ����� �� �����
        Vector3 newPosition = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = newPosition + offset;

        if (BufferCard.transform.parent != DefaultBufferCard)
            BufferCard.transform.SetParent(DefaultBufferCard);

        //����������� ������� ����� ������������ ������ ����
        CheckPoisition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(DefaultParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //�������������� � ��������� ������
        transform.SetSiblingIndex(BufferCard.transform.GetSiblingIndex());
        BufferCard.transform.SetParent(GameObject.Find("Canvas").transform);
        BufferCard.transform.localPosition = new Vector3(2538, 200); 
    }


    void CheckPoisition()
    {
        int newIndex = DefaultBufferCard.childCount;
        // ����� �� ������� �� ������ ���� ��� ���� ����� ������� �� ����� ����������� �� ������� ������ �� ��� ������ ���� �����
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
        //������������� ������� �����
        BufferCard.transform.SetSiblingIndex(newIndex);
    }
}
