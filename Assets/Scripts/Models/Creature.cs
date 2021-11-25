using System.Collections.Generic;
using System.Linq;
using Models.Decorator;
using Models.Enums;
using Models.Interface;
using Models.Weapons;
using UnityEngine;

namespace Models { 
    public class Creature : AbstractCard
    {
        private Stat Health { get; set; }
        private Stat Attack { get; set; }
        public Sprite WeaponIcon { get; set; }
        public List<AbilityType> Abilities { get; }
        private IWeapon _weapon;

        public Creature(string name, string description, string logoPath, int attack, int health, int manacost, AbilityType abilityType = 0) : 
            base(name, description, logoPath, manacost)
        {
            Attack = new Stat(attack, StatType.ATTACK);
            Health = new Stat(health, StatType.HEALTH);
            CanAttack = false;
            IsPlaced = false;
            _weapon = new Arm();
            Abilities = new List<AbilityType>();
            if(abilityType != 0)
                Abilities.Add(abilityType);
            TimesDealDamage = 0;
        }

        private Creature(Creature card) : base(card.Name, card.Description, card.Logo, card.ManaCost)
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
        public override void GetDamage(int damage)
        {
            if (damage <= 0) return;
            //Поглащение урона щитом если его нет то просто наносим урон
            if (Abilities.Exists(x => x == AbilityType.SHIELD))
                Abilities.Remove(AbilityType.SHIELD);
            else
                Health = new SpellEffect(Health, SpellEffectType.SUBSTRACT, damage).GetEffect();
            Debug.Log("Card: " + Name + " Stat: " + Health.StatType + " " + Health.Value);
        }

        public override IWeapon GetWeapon()
        {
            return _weapon;
        }

        public override Sprite GetWeaponIcon()
        {
            return WeaponIcon;
        }

        public override void SetWeapon(IWeapon weapon)
        {
            _weapon = weapon;
            WeaponIcon = Resources.Load<Sprite>(weapon.GetWeaponIcon());
        }

        public override int GetAttackForDamage()
        {
            return Attack.Value + _weapon.UseWeapon();
        }

        public override AbstractCard GetCardCopy()
        {
            return new Creature(this);
        }

        public override void AddAbility(int value)
        {
            Abilities.Add((AbilityType)value);
        }

        public override bool IsSpell()
        {
            return false;
        }

        public override bool HasAbility()
        {
            return Abilities.Count > 0;
        }
        //Return Health or Attack
        public override Stat GetStat(StatType name)
        {
            if (name == Health.StatType)
                return Health;
            return name == Attack.StatType ? Attack : new Stat(0, StatType.DEFAULT);
        }
        //Set Health or Attack
        public override void SetStat(Stat stat)
        {
            if (stat.StatType == Health.StatType)
                Health = stat;
            if (stat.StatType == Attack.StatType)
                Attack = stat;
        }

        public override int GetTargetType()
        {
            return (int)TargetType.NO_TARGET;
        }

        public override List<int> GetAbility()
        {
            return Abilities.Select(ability => (int) ability).ToList();
        }
    }
}