
using Assets.Scripts.Models.Enums;

namespace Assets.Scripts.Models
{
    public abstract class AbstractCard
    {
        public virtual int Damage { get; set; }
        public string Image { get; set; }
        public CardType Type { get; set; }
        public string Description { get; set; }
    }
}
