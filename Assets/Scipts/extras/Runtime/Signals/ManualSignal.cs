using UnityEngine;

namespace RSG.Trellis.Signals
{
    public abstract class ManualSignal<T> : Signal<T>
    {
        [SerializeField] protected T initialValue;

        private void OnValidate()
        {
            //Value = initialValue ?? DefaultValue;

            if (initialValue != null)
            {
                Value = initialValue;
            }
            else
            {
                Value = DefaultValue;
            }
            NotifyDerived();
        }

        protected override void ReInitialize()
        {
            base.ReInitialize();
            if (initialValue != null)
            {
                Value = initialValue;
            }
            else
            {
                Value = DefaultValue;
            }
            //Value = initialValue ?? DefaultValue;
            
        }

        public void SetValue(T value)
        {
            SetValueInternal(value);
        }
    }
}