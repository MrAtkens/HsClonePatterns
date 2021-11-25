using Models.Interface;

namespace Models.Weapons
{
    public abstract class AbstractWeapon : IWeapon
    {
        protected int Durability;
        protected readonly int Damage;
        protected readonly string Icon;

        protected AbstractWeapon(int durability, int damage, string icon)
        {
            Durability = durability;
            Damage = damage;
            Icon = icon;
        }

        public string GetWeaponIcon()
        {
            return Icon;
        }

        public int GetWeaponDamage()
        {
            return Damage;
        }
        public abstract int GetDurability();
        public abstract int UseWeapon();
    }
}