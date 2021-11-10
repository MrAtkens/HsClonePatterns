using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
   public Card SelfCard;
   public Image Logo;
   public TextMeshProUGUI Name, Description, Attack, Health, ManaCost;
   public GameObject HideObject, HihgliteObject;
   public bool IsPlayer;
   public Color NormalCol, TargetCol;
   public void HideCardInfo(Card card)
   {
      SelfCard = card;
      HideObject.SetActive(true);
      IsPlayer = false;
      ManaCost.text = "";
   }
   public void ShowCardInfo(Card card, bool isPlayer)
   {
      IsPlayer = isPlayer;
      HideObject.SetActive(false);
      SelfCard = card;
      Logo.sprite = card.Logo;
      Logo.preserveAspect = true;
      Name.text = card.Name;
      Description.text = card.Description;
      Attack.text = card.Attack.ToString();
      Health.text = card.Health.ToString();
      ManaCost.text = card.ManaCost.ToString();
   }

   public void RefreshData()
   {
      Attack.text = SelfCard.Attack.ToString();
      Health.text = SelfCard.Health.ToString();
      ManaCost.text = SelfCard.ManaCost.ToString(); 
   }

   public void HighliteOn()
   {
      HihgliteObject.SetActive(true);
   }
   
   public void HighliteOff()
   {
      HihgliteObject.SetActive(false);
   }

   public void CheckForAvailability(int currentMana)
   {
      GetComponent<CanvasGroup>().alpha = currentMana >= SelfCard.ManaCost ? 1 : .5f;
   }

   public void HighlightAsTarget(bool highlight)
   {
      GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
   }
}
