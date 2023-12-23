using UnityEngine;

namespace RSG.Trellis.Signals
{
    [CreateAssetMenu(menuName = "Signals/string")]
    public class StringSignal : ManualSignal<string>
    {
        protected override string DefaultValue => string.Empty;
    }
}