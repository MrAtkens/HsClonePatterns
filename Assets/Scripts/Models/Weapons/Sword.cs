using Assets.Scripts.Models.Enums;
using Assets.Scripts.Models.Interface;

namespace Assets.Scripts.Models.Weapons
{
    public class Sword : IWeapon
    {
        private int Durability;
        public int Damage { get; }
        public Status Effect { get; }
        public Sword(int durability, int damage, Status status)
        {
            Durability = durability;
            Damage = damage;
            Effect = status;

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
