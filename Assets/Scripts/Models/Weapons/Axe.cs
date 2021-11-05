using Assets.Scripts.Models.Interface;
using System;

namespace Assets.Scripts.Models.Weapons
{
    public class Axe : IWeapon
    {
        public int Damage { get; set; }

        public int getDurability()
        {
            throw new NotImplementedException();
        }

        public int useWeapon(int cardDamage)
        {
            Random random = new Random();
            int weaponDamage = random.Next(1, 10);
            if(weaponDamage > 4)
                return (int)Math.Round(cardDamage * 1.5);
            return cardDamage + Damage;
        }
    }
}
