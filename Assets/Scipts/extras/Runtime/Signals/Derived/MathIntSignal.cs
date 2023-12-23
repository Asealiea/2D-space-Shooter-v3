using System;
using System.Collections.Generic;
using UnityEngine;

namespace RSG.Trellis.Signals.Derived
{
    public enum Operator
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
    
    [CreateAssetMenu(menuName = "Signals/Calculated/Math (int)")]
    public class MathIntSignal : DerivedSignal<int>, IDerivedSignal
    {
        [SerializeField] private Signal<int> source1;
        [SerializeField] private Operator @operator;
        [SerializeField] private Signal<int> source2;

        public override IEnumerable<Signal> Dependees
        {
            get
            {
                yield return source1; 
                yield return source2;
            }
        }

        public override void Recalculate()
        {
            if (!source1 || !source2) return;
            
            int value;
            switch (@operator)
            {
                case Operator.Add:
                    value = source1.Value + source2.Value;
                    break;
                case Operator.Subtract:
                    value = source1.Value - source2.Value;
                    break;
                case Operator.Multiply:
                    value = source1.Value * source2.Value;
                    break;
                case Operator.Divide:
                    value = source1.Value / source2.Value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SetValueInternal(value);
        }
    }
}