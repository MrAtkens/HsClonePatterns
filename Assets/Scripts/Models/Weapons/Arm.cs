using UnityEngine;

namespace Models.Weapons
{
    public class Arm : AbstractWeapon
    {
        public Arm() : base(1, 0, WeaponIcons.ArmIcon)
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
            return 0;
        }
    }
}