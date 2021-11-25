using System.Security.Cryptography;
using Models.Enums;

namespace Models.FactoryMethod
{
    public abstract class Creator
    {
        protected abstract AbstractCard FactoryMethod(string name, string description, string logoPath, int attack, int health, int manaCost, AbilityType abilityType);

        protected abstract AbstractCard FactoryMethod(string name, string description, string logoPath, int manaCost, SpellType spellType, TargetType targetType, int spellValue);
        public AbstractCard CreateCard(string name, string description, string logoPath, int attack, int health, int manaCost, AbilityType abilityType = 0)
        {
            var product = FactoryMethod(name,description,logoPath, attack, health, manaCost, abilityType);
            return product;
        }
        
        public AbstractCard CreateCard(string name, string description, string logoPath, int manaCost, int spellValue, TargetType targetType, SpellType spellType)
        {
            var product = FactoryMethod(name,description,logoPath,manaCost, spellType, targetType, spellValue);
            return product;
        }
    }
}