using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RSG.Trellis.Signals
{
    public class RuntimeListSignal<T> : Signal<T[]>
        where T : ScriptableObject
    {
        [SerializeField] private T[] initialValues = Array.Empty<T>();

        private void OnValidate()
        {
            Value = initialValues;
            NotifyDerived();
        }

        protected override void ReInitialize()
        {
            base.ReInitialize();
            Value = initialValues;
        }

        public void Modify(Action<List<T>> doModification)
        {
            var list = initialValues.ToList();
            doModification(list);
            SetValueInternal(list.ToArray());
        }
    }
}