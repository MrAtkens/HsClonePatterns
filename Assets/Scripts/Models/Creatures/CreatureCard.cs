using Assets.Scripts.Models;
using Assets.Scripts.Models.Interface;

public class CreatureCard : AbstractCard
{
    protected IWeapon Weapon { get; set; }
    public int Health { get; set; }
    public override int Damage
    {
        get
        {
            return Weapon.useWeapon(Damage);
        }
    }

}
