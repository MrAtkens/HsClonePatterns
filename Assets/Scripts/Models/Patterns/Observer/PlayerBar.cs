using Models.Interface;
using TMPro;
using UnityEngine;

namespace Models.Observer
{
    public class PlayerBar : IObserver
    {
        public TextMeshProUGUI PlayerText;
        public override void UpdateValue(int value)
        {
            PlayerText.text = value.ToString();
        }
    }
}