using System;
using Models.Interface;
using UnityEngine;
using Random = System.Random;

namespace Models.Weapons
{
    public class Mace : AbstractWeapon
    {
        public Mace(int durability, int damage) : base(durability, damage, WeaponIcons.MaceIcon)
        {
        }
        public override int GetDurability()
        {
            return Durability;
        }
        public override int UseWeapon()
        {
            Durability = Mathf.Clamp(Durability - 1, 0, int.MaxValue); 
            var random = new Random();
            var weaponDamage = random.Next(1, 100);
            if (weaponDamage < 50)
            {
                Debug.Log("Weapon damage: " + GetType() + " Durability " + Durability + " Damage " + Damage * 2);
                return Damage * 2;
            }
            Debug.Log("Weapon damage: " + GetType() + " Durability " + Durability + " Damage " + Damage);
            return Damage;
        }
    }
}
