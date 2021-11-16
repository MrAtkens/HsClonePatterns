using System.Collections.Generic;
using Models.Enums;
using UnityEngine;

namespace Models { 
    public class Card : AbstractCard {
        public int Attack { get; set; }
        public int Health { get; set; }
        public bool CanAttack { get; set; }
        public List<AbilityType> Abilities { get; }
        public bool IsAlive => Health > 0;
        public bool HasAbility => Abilities.Count > 0;
        public bool IsProvocation => Abilities.Exists(x => x == AbilityType.PROVOCATION);
        public int TimesDealDamage { get; set; }

        public Card(string name, string description, string logoPath, int attack, int health, int manacost, AbilityType abilityType = 0) : 
            base(name, description, logoPath, manacost)
        {
            Attack = attack;
            Health = health;
            CanAttack = false;
            IsPlaced = false;
            
            Abilities = new List<AbilityType>();
            if(abilityType != 0)
                Abilities.Add(abilityType);
            TimesDealDamage = 0;
        }

        public Card(Card card) : base(card.Name, card.Description, card.Logo, card.ManaCost)
        {
            Attack = card.Attack;
            Health = card.Health;
            CanAttack = false;
            IsPlaced = false;
            Abilities = new List<AbilityType>(card.Abilities);
            TimesDealDamage = 0;
        }
        public void GetDamage(int damage)
        {
            if (damage <= 0) return;
            //Поглащение урона щитом если его нет то просто наносим урон
            if (Abilities.Exists(x => x == AbilityType.SHIELD))
                Abilities.Remove(AbilityType.SHIELD);
            else
                Health -= damage;
        }

        public Card GetCardCopy()
        {
            return new Card(this);
        }
        
    }
}