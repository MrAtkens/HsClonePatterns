using Models.Enums;

namespace Models
{
    public class Stat
    {
        public int Value { get; }
        public StatType StatType { get; }
        public Stat(int statValue, StatType statName)
        {
            Value = statValue;
            StatType = statName;
        }
    }
}