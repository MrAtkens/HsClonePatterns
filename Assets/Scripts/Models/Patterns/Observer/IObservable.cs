using UnityEngine;

namespace Models.Interface
{
    public interface IObservable
    {
        void NotifyHealthObserver(int value);
        void NotifyManaObserver(int value);
    }
}