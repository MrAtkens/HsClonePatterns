using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
   public Card SelfCard;
   public Image Logo;
   public TextMeshProUGUI Name;
   public TextMeshProUGUI Description;

   public void HideCardInfo(Card card)
   {
      SelfCard = card;
      Logo.sprite = null;
      Name.text = "";
   }
   public void ShowCardInfo(Card card)
   {
      SelfCard = card;
      Logo.sprite = card.Logo;
      Logo.preserveAspect = true;
      Name.text = card.Name;
      Description.text = card.Description;
   }

   private void Start()
   {   
      // Инициализация prefab карты на поле
      //ShowCardInfo(CardManager.AllCards[transform.GetSiblingIndex()]);
   }
}
