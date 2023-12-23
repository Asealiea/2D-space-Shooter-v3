using RSG.Trellis.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace RSG.Trellis.Listeners
{
    public class BoolListener : SignalListener<bool, Signal<bool>>
    {
        [SerializeField] private UnityEvent onTrue;
        [SerializeField] private UnityEvent onFalse;

        protected override void OnChanged()
        {
            base.OnChanged();

            if (signal.Value)
            {
                onTrue.Invoke();
            }
            else
            {
                onFalse.Invoke();
            }
        }
    }
}