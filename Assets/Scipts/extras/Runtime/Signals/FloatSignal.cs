using UnityEngine;

namespace RSG.Trellis.Signals
{
    [CreateAssetMenu(menuName = "Signals/float")]
    public class FloatSignal : ManualSignal<float>
    {
        public void Increment(float howMuch)
        {
            SetValueInternal(Value + howMuch);
        }

        public void Decrement(float howMuch)
        {
            SetValueInternal(Value - howMuch);
        }
}
}