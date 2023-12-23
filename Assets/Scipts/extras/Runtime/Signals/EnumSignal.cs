using System;

namespace RSG.Trellis.Signals
{
    public class EnumSignal<TEnum> : ManualSignal<TEnum>
        where TEnum : struct, Enum
    {
        public void SetFromInt(int value)
        {
            SetValueInternal((TEnum)(ValueType)value);
        }
    }
}