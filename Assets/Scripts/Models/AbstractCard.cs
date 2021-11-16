using UnityEngine;

namespace Models
{
    public abstract class AbstractCard
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Logo { get; }
        public int ManaCost { get; }
        public bool IsPlaced { get; set; }
        public bool IsSpell { get; protected set; }
        protected AbstractCard(string name, string description, string logoPath, int manaCost)
        {
            Name = name;
            Description = description;
            Logo = Resources.Load<Sprite>(logoPath);
            ManaCost = manaCost;
        }
        
        protected AbstractCard(string name, string description, Sprite logo, int manaCost)
        {
            Name = name;
            Description = description;
            Logo = logo;
            ManaCost = manaCost;
        }
        
    }
}