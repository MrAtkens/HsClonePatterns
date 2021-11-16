using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;


    public GameObject ResultGO;
    public TextMeshProUGUI ResultTxt;

    public TextMeshProUGUI TurnTime;
    public Button EndTurnBtn;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }

    public void StartGame()
    {
        EndTurnBtn.interactable = true;
        ResultGO.SetActive(false);
    }

    public void ShowResult()
    {
        ResultGO.SetActive(true);

        if (GameManager.Instance.Enemy.GetHealth() == 0)
            ResultTxt.text = "WIN";
        else
            ResultTxt.text = "-25";
    }

    public void UpdateTurnTime(int time)
    {
        TurnTime.text = time.ToString();
    }

    public void DisableTurnBtn()
    {
        EndTurnBtn.interactable = GameManager.Instance.IsPlayerTurn;
    }
}