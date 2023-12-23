using System.Collections.Generic;
using UnityEngine;

namespace RSG.Trellis.Signals.Derived
{
    public abstract class MatcherSignal<T> : DerivedSignal<bool> 
    {
        [SerializeField] private Signal<T> source;
        [SerializeField] private T compareWith;
        [SerializeField] private bool invertResult;

        public override IEnumerable<Signal> Dependees
        {
            get { yield return source; }
        }

        public override void Recalculate()
        {
            if (source)
            {
                var matches = source.Value.Equals(compareWith);
                SetValueInternal(matches ^ invertResult);
            }
        }
    }
}