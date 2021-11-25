using Models.Enums;

namespace Models.FactoryMethod
{
    public class CreatureCreator : Creator
    {
        protected override AbstractCard FactoryMethod(string name, string description, string logoPath, int manaCost, int attack, int health, AbilityType abilityType)
        {
            return new Creature(name, description, logoPath, attack, health, manaCost, abilityType);
        }

        protected override AbstractCard FactoryMethod(string name, string description, string logoPath, int manaCost, SpellType spellType,
            TargetType targetType, int spellValue)
        {
            throw new System.NotImplementedException();
        }
    }
}