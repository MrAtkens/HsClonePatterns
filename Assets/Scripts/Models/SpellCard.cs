using Models.Enums;

namespace Models
{
    public class SpellCard : Card
    {
        public SpellType Spell;
        public TargetType SpellTarget;
        public int SpellValue;

        public SpellCard(string name, string description,  string logoPath, int manacost, SpellType spellType = 0, int spellValue = 0,
            TargetType targetType = 0) : base(name, description, logoPath, 0, 0, manacost)
        {
            IsSpell = true;
            Spell = spellType;
            SpellTarget = targetType;
            SpellValue = spellValue;
        }

        public SpellCard(SpellCard card) : base(card)
        {
            IsSpell = true;
            Spell = card.Spell;
            SpellTarget = card.SpellTarget;
            SpellValue = card.SpellValue;
        }

        public new SpellCard GetCardCopy()
        {
            return new SpellCard(this);
        }
    }
}