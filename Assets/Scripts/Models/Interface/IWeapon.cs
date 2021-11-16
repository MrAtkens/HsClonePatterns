namespace Models.Interface
{
    public interface IWeapon
    {
        int GetDurability();
        int GetWeaponDamage();
        string GetWeaponIcon();
        int UseWeapon();
    }
}
