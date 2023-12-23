using UnityEngine;

namespace RSG.Trellis.Signals
{
    [CreateAssetMenu(menuName = "Signals/int")]
    public class IntSignal : ManualSignal<int>
    {
        public void Increment(int howMuch)
        {
            SetValueInternal(Value + howMuch);
        }
    }
}