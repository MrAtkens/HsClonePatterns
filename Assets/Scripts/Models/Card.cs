using System.Collections.Generic;
using Models.Enums;
using Models.Interface;
using Models.Weapons;
using UnityEngine;

namespace Models { 
    public class Card : AbstractCard
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        public Sprite WeaponIcon { get; set; }
        public bool CanAttack { get; set; }
        public List<AbilityType> Abilities { get; }
        public bool IsAlive => Health > 0;
        public bool HasAbility => Abilities.Count > 0;
        public bool IsProvocation => Abilities.Exists(x => x == AbilityType.PROVOCATION);
        public int TimesDealDamage { get; set; }
        private IWeapon _weapon;

        public Card(string name, string description, string logoPath, int attack, int health, int manacost, AbilityType abilityType = 0) : 
            base(name, description, logoPath, manacost)
        {
            Attack = attack;
            Health = health;
            CanAttack = false;
            IsPlaced = false;
            _weapon = new Arm();
            WeaponIcon = Resources.Load<Sprite>(_weapon.GetWeaponIcon());
            Abilities = new List<AbilityType>();
            if(abilityType != 0)
                Abilities.Add(abilityType);
            TimesDealDamage = 0;
        }

        protected Card(Card card) : base(card.Name, card.Description, card.Logo, card.ManaCost)
        {
            Attack = card.Attack;
            Health = card.Health;
            _weapon = new Arm();
            WeaponIcon = Resources.Load<Sprite>(_weapon.GetWeaponIcon());
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

        public int GetDurability()
        {
            return _weapon.GetDurability();
        }

        public void SetWeapon(IWeapon weapon)
        {
            _weapon = weapon;
            WeaponIcon = Resources.Load<Sprite>(weapon.GetWeaponIcon());
        }

        public int GetAttackForDamage()
        {
            return Attack + _weapon.UseWeapon();
        }

        public int GetWeaponDamage()
        {
            return _weapon.GetWeaponDamage();
        }

        public Card GetCardCopy()
        {
            return new Card(this);
        }
        
    }
}