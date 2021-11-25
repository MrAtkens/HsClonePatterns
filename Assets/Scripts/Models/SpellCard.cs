using System.Collections.Generic;
using Models.Enums;
using Models.Interface;
using UnityEngine;

namespace Models
{
    public class SpellCard : AbstractCard
    {
        private SpellType Spell;
        public TargetType SpellTarget;
        private Stat SpellValue;

        public SpellCard(string name, string description, string logoPath, int manacost, int spellValue,
            TargetType targetType = 0, SpellType spellType = 0) : base(name, description, logoPath, manacost)
        {
            Spell = spellType;
            SpellTarget = targetType;
            SpellValue = new Stat(spellValue, StatType.SPELL_VALUE);
        }

        private SpellCard(SpellCard card) : base(card.Name, card.Description, card.Logo, card.ManaCost)
        {
            Spell = card.Spell;
            SpellTarget = card.SpellTarget;
            SpellValue = card.SpellValue;
        }

        public override void GetDamage(int damage)
        {
            throw new System.NotImplementedException();
        }

        public override int GetAttackForDamage()
        {
            throw new System.NotImplementedException();
        }

        public override AbstractCard GetCardCopy()
        {
            return new SpellCard(this);
        }
        
        public override bool IsSpell()
        {
            return true;
        }
        
        public override bool HasAbility()
        {
            return false;
        }

        public override Stat GetStat(StatType name)
        {
            return name == SpellValue.StatType ? SpellValue : new Stat(0, StatType.DEFAULT);
        }

        public override void SetStat(Stat stat)
        {
            if (stat.StatType == SpellValue.StatType)
                SpellValue = stat;
        }

        public override int GetTargetType()
        {
            return (int)SpellTarget;
        }

        public override List<int> GetAbility()
        {
            var list = new List<int> {(int) Spell};
            return list;
        }
        
        
        public override IWeapon GetWeapon()
        {
            return null;
        }

        public override Sprite GetWeaponIcon()
        {
            return null;
        }

        public override void SetWeapon(IWeapon weapon){}
        
        public override void AddAbility(int value)
        { 
            Spell = (SpellType)value;
        }
    }
}