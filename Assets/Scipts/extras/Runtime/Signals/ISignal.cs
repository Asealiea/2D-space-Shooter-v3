using UnityEngine.Events;

namespace RSG.Trellis.Signals
{
    public interface ISignal<out T>
    {
        void AddListener(UnityAction call, bool runNow = true);
        void RemoveListener(UnityAction call);
        
        T Value { get;  }
    }
}