using RSG.Trellis.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace RSG.Trellis.Listeners
{
    public class SignalListener<T, TSignal> : MonoBehaviour
        where TSignal : ISignal<T>
    {
        [SerializeField] private bool syncOnStart = true;
        [SerializeField] protected TSignal signal;
        [SerializeField] private UnityEvent<T> onChanged;

        private void OnEnable()
        {
            signal.AddListener(OnChanged, syncOnStart);
        }

        private void OnDisable()
        {
            signal.RemoveListener(OnChanged);
        }

        protected virtual void OnChanged()
        {
            onChanged.Invoke(signal.Value);
        }
    }
}