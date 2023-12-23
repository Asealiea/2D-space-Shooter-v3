using System.Collections.Generic;

namespace RSG.Trellis.Signals.Derived
{
    public interface IDerivedSignal
    {
        void Recalculate();
        IEnumerable<Signal> Dependees { get; }
    }
}