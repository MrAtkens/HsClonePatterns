using Models.Enums;

namespace Models.FactoryMethod
{
    public class SpellCreator : Creator
    {
        protected override AbstractCard FactoryMethod(string name, string description, string logoPath, int attack, int health, int manaCost,
            AbilityType abilityType)
        {
            throw new System.NotImplementedException();
        }

        protected override AbstractCard FactoryMethod(string name, string description, string logoPath, int manaCost, SpellType spellType,
            TargetType targetType, int spellValue)
        {
            return new SpellCard(name, description, logoPath, manaCost, spellValue, targetType, spellType);
        }
    }
}