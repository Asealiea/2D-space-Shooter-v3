using System;
using System.Collections.Generic;
using System.Linq;

namespace RSG.Trellis.Signals.Derived
{
    public abstract class DerivedSignal<T> : Signal<T>, IDerivedSignal
    {
        public abstract IEnumerable<Signal> Dependees { get; }
        private Signal[] _dependees;
        
        private void OnValidate()
        {
            ReInitialize();
        }

        protected override void ReInitialize()
        {
            base.ReInitialize();
            EnsureRegistered();
            Recalculate();
        }

        private void EnsureRegistered()
        {
            foreach (var dep in _dependees ?? Array.Empty<Signal>())
            {
                if (dep)
                {
                    dep.Unregister(this);
                }
            }

            _dependees = Dependees.Where(d => d != null).ToArray();
            foreach (var dep in _dependees)
            {
                dep.Register(this);
            }
            Recalculate();
        }

        public abstract void Recalculate();
    }
}