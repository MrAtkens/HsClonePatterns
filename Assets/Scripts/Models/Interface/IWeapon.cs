namespace Assets.Scripts.Models.Interface
{
    public interface IWeapon
    {
        int getDurability();
        int useWeapon(int cardDamage);
    }
}
