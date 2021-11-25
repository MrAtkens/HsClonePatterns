
using Models.Enums;
using UnityEngine;

namespace Models.Decorator
{
    public class SpellEffect
    {
        private Stat _stat;
        private SpellEffectType _effect;
        private int _value;
        private static int MAX_EFFECT = 99;

        public SpellEffect(Stat stat, SpellEffectType effect, int value)
        {
            _stat = stat;
            _effect = effect;
            _value = value;
        }

        public Stat GetEffect()
        {
            switch (_effect)
            {
                case SpellEffectType.ADD:
                    return new Stat(Mathf.Clamp(_stat.Value + _value, 0, MAX_EFFECT), _stat.StatType);
                    break;
                case SpellEffectType.SUBSTRACT:
                    return new Stat(Mathf.Clamp(_stat.Value - _value, 0, MAX_EFFECT), _stat.StatType);
                    break;
                case SpellEffectType.MULTIPLY:
                    return new Stat(Mathf.Clamp(_stat.Value * _value, 0, MAX_EFFECT), _stat.StatType);
                    break;
                case SpellEffectType.DIVIDE:
                    return new Stat(Mathf.Clamp(_stat.Value / _value, 0, MAX_EFFECT), _stat.StatType);
                    break;
                default:
                    return _stat;
            }
        }
    }
}