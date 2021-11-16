using Assets.Scripts.Models.Interface;

namespace Models.Weapons
{
    public class Sword : IWeapon
    {
        private int Durability;
        public int Damage { get; }
        public Sword(int durability, int damage)
        {
            Durability = durability;
            Damage = damage;
        }
        public int getDurability()
        {
            return Durability;
        }

        public int useWeapon(int cardDamage)
        {
            return Damage + cardDamage;
        }
    }
}
