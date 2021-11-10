using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AttackHero : MonoBehaviour, IDropHandler{
    public enum HeroType
    {
        Enemy, Player
    }

    public HeroType Type;
    public GameManager GameManager;
    public Color NormalCol, TargetCol;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameManager.IsPlayerTurn)
            return;
        CardInfo card = eventData.pointerDrag.GetComponent<CardInfo>();

        if(card && card.SelfCard.CanAttack && Type == HeroType.Enemy)
        {
            card.SelfCard.CanAttack = false;
            GameManager.DamageHero(card, true);
        }
    }
    
    public void HighlightAsTarget(bool highlight)
    {
        GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
    }
}
