using System;
using RSG.Trellis.Signals;
using UnityEngine;
using UnityEngine.Events;

namespace RSG.Trellis
{
    public class ConstantSelectableList<T> : ScriptableObjectWithEmbedded
    {
        [SerializeField] private T[] values = Array.Empty<T>();
        [SerializeField, Embedded("Focused Index")] public IntSignal focusedIndex;
        
        public T[] Values => values;
        public T FocusedElement => focusedIndex.Value >= 0 && focusedIndex.Value < values.Length ? values[focusedIndex.Value] : default;

        public void AddListener(UnityAction call, bool runNow = true)
        {
            focusedIndex.AddListener(call, runNow);
        }

        public void RemoveListener(UnityAction call)
        {
            focusedIndex.RemoveListener(call);
        }
    }
}