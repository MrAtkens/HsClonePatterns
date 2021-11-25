using System.Collections.Generic;
using Models.Enums;
using Models.Interface;
using UnityEngine;

namespace Models
{
    public abstract class AbstractCard
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Logo { get; }
        public Stat ManaCost { get; }
        public int TimesDealDamage { get; set; }

        public bool IsPlaced { get; set; }
        public bool CanAttack { get; set; }

        protected AbstractCard(string name, string description, string logoPath, int manaCost)
        {
            Name = name;
            Description = description;
            Logo = Resources.Load<Sprite>(logoPath);
            ManaCost = new Stat(manaCost, StatType.MANA);
        }
        
        protected AbstractCard(string name, string description, Sprite logo, Stat manaCost)
        {
            Name = name;
            Description = description;
            Logo = logo;
            ManaCost = manaCost;
        }

        public abstract void GetDamage(int damage);
        public abstract int GetAttackForDamage();
        public abstract AbstractCard GetCardCopy();
        public abstract int GetTargetType();
        public abstract List<int> GetAbility();
        public abstract void AddAbility(int value);
        public abstract bool IsSpell();
        public abstract bool HasAbility();
        public abstract Stat GetStat(StatType name);
        public abstract void SetStat(Stat stat);

        public abstract IWeapon GetWeapon();
        public abstract Sprite GetWeaponIcon();
        public abstract void SetWeapon(IWeapon weapon);
    }
}