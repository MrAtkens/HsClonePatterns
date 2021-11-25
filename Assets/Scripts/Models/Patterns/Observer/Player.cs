using System;
using Models.Enums;
using Models.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Models.Observer
{
    public class Player : MonoBehaviour, IDropHandler, IObservable
    {
        private const int MAX_MANAPOOL = 10;
        private int Health;
        private int Mana, ManaPool;
        public HeroType Type;
        public Color NormalCol, TargetCol;
        public IObserver PlayerMana, PlayerHealth;

        public Player()
        {
            Health = 30;
            Mana = ManaPool = 2;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (!GameManager.Instance.IsPlayerTurn)
                return;
            var card = eventData.pointerDrag.GetComponent<CardController>();

            if(card && card.Card.CanAttack && Type == HeroType.ENEMY && !GameManager.Instance.EnemyFieldCards.Exists(x => x.IsProvocation()))
            {
                GameManager.Instance.DamageHero(card, true);
            }
        }
    
        public void HighlightAsTarget(bool highlight)
        {
            GetComponent<Image>().color = highlight ? TargetCol : NormalCol;
        }

        public void ReduceMana(int mana)
        {
            //Чтобы значение не упало ниже нуля
            Mana = Mathf.Clamp(Mana - mana, 0, int.MaxValue);
            NotifyManaObserver(Mana);
        }
        
        public void RestoreMana()
        {
            Mana = ManaPool;
            NotifyManaObserver(Mana);
        }

        public void IncreaseManaPool()
        {
            ManaPool = Mathf.Clamp(ManaPool + 1, 0, MAX_MANAPOOL);
        }

        public void ApplyDamage(int damage)
        {
            Health =  Mathf.Clamp(Health - damage, 0, int.MaxValue);;
            NotifyHealthObserver(Health);
        }

        public void Heal(int heal)
        {
            Health =  Mathf.Clamp(Health + heal, 0, int.MaxValue);;
            NotifyHealthObserver(Health);
        }

        public void HealthRestore()
        {
            Health = 30;
            NotifyHealthObserver(Health);
        }

        public void RestoreManaPool()
        {
            ManaPool = 2;
        }

        public void NotifyHealthObserver(int value)
        {
            PlayerHealth.UpdateValue(value);
        }

        public void NotifyManaObserver(int value)
        {
            PlayerMana.UpdateValue(value);
        }

        public int GetHealth()
        {
            return Health;
        }

        public int GetMana()
        {
            return Mana;
        }
    }
}