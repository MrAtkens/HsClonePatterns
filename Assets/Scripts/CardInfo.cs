using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour
{
   public CardController cardController;
   public Image Logo;
   public TextMeshProUGUI Name, Description, Attack, Health, ManaCost;
   public GameObject HideObject, HihgliteObject;
   public Color NormalCol, TargetCol, SpellTargetColor;
   public void HideCardInfo()
   {
      HideObject.SetActive(true);
      ManaCost.text = "";
   }
   //
   public void ShowCardInfo()
   {
      HideObject.SetActive(false);
      
      Logo.sprite = cardController.Card.Logo;
      Logo.preserveAspect = true;
      Name.text = cardController.Card.Name;
      Description.text = cardController.Card.Description;

      if (cardController.Card.IsSpell)
      {
         Attack.gameObject.SetActive(false);
         Health.gameObject.SetActive(false);
      }
      
      RefreshData();
   }
   
   //Обновить prefab карты
   public void RefreshData()
   {
      Attack.text = cardController.Card.Attack.ToString();
      Health.text = cardController.Card.Health.ToString();
      ManaCost.text = cardController.Card.ManaCost.ToString(); 
   }
   //подсветка карты на то можно ли её использовать на поле
   public void HighlightCard(bool isHighlight)
   {
      HihgliteObject.SetActive(isHighlight);
   }
   //подстветка на то что не хватает маны
   public void HighlightManaAvailability(int currentMana)
   {
      GetComponent<CanvasGroup>().alpha = currentMana >= cardController.Card.ManaCost ? 1 : .5f;
   }
   //подстветка цели для карты на столе
   public void HighlightAsTarget(bool highlight)
   {
      GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
   }
   //подстветка цели для заклинания
   public void HighlightAsSpellTarget(bool highlight)
   {
      GetComponent<Image>().color = highlight ? SpellTargetColor : NormalCol;
   }
}
