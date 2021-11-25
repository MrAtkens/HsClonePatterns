using System;
using System.Text.RegularExpressions;
using Models.Interface;
using UnityEngine;
using Random = System.Random;

namespace Models.Weapons
{
    public class Axe : AbstractWeapon
    {
        public Axe(int durability, int damage) : base(durability, damage, WeaponIcons.AxeIcon)
        {
        }
        public override int GetDurability()
        {
            return Durability;
        }
        public override int UseWeapon()
        {
            Durability = Mathf.Clamp(Durability - 1, 0, int.MaxValue); 
            Debug.Log(Durability);
            var random = new Random();
            var weaponDamage = random.Next(1, 100);
            if (weaponDamage > 50)
            {
                Debug.Log("Weapon damage: " + GetType() + " Durability " + Durability + " Damage " + Math.Round(Damage * 1.5));
                return (int) Math.Round(Damage * 1.5);
            }
            Debug.Log("Weapon damage: " + GetType() + " Durability " + Durability + " Damage " + Damage);
            return Damage;
        }
    }
}
