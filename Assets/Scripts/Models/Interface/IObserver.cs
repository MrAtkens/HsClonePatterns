using UnityEngine;

namespace Models.Interface
{
    public abstract class IObserver : MonoBehaviour
    {
        public abstract void UpdateValue(int value);
    }
}