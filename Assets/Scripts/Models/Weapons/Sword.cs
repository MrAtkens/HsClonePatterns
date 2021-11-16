using Models.Interface;
using UnityEngine;

namespace Models.Weapons
{
    public class Sword : AbstractWeapon
    {
        public Sword(int durability, int damage) : base(durability, damage, WeaponIcons.SwordIcon)
        {
        }
        public override int GetDurability()
        {
            return Durability;
        }

        public override int UseWeapon()
        {
            Durability = Mathf.Clamp(Durability - 1, 0, int.MaxValue); 
            Debug.Log("Weapon damage: " + GetType() + " Durability " + Durability + " Damage " + Damage);
            return Damage;
        }
    }
}
