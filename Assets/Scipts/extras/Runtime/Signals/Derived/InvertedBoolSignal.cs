using System.Collections.Generic;
using UnityEngine;

namespace RSG.Trellis.Signals.Derived
{
    [CreateAssetMenu(menuName = "Signals/Calculated/Inverted bool")]
    public class InvertedBoolSignal : DerivedSignal<bool>, IDerivedSignal
    {
        [SerializeField] private Signal<bool> _source;

        public override IEnumerable<Signal> Dependees
        {
            get { yield return _source; }
        }

        public override void Recalculate()
        {
            if (_source)
            {
                SetValueInternal(!_source.Value);
            }
        }
    }
}