using UnityEngine;

namespace RSG.Trellis.Signals
{
    [CreateAssetMenu(menuName = "Signals/bool")]
    public class BoolSignal : ManualSignal<bool>
    {
        public void Toggle()
        {
            SetValue(!Value);
        }
    }
}