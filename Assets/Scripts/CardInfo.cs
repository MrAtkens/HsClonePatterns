using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
   public Card SelfCard;
   public Image Logo;
   public TextMeshProUGUI Name, Description, Attack, Health;
   public GameObject HideObject, HihgliteObject;
   public bool IsPlayer;
   public void HideCardInfo(Card card)
   {
      SelfCard = card;
      HideObject.SetActive(true);
      IsPlayer = false;
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
   }

   public void RefreshData()
   {
      Attack.text = SelfCard.Attack.ToString();
      Health.text = SelfCard.Health.ToString();
   }

   public void HighliteOn()
   {
      HihgliteObject.SetActive(true);
   }
   
   public void HighliteOff()
   {
      HihgliteObject.SetActive(false);
   }
}
