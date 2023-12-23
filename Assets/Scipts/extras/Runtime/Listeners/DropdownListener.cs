using System;
using System.Linq;
using RSG.Trellis.Signals;
using TMPro;
using UnityEngine;

namespace RSG.Trellis.Listeners
{
    public class DropdownListener<T> : MonoBehaviour where T : struct, Enum, IConvertible
    {
        [SerializeField] private EnumSignal<T> signal;
        [SerializeField] private TMP_Dropdown dropDown;

        protected void ResetOptions()
        {
            var values = Enum.GetNames(typeof(T));
            dropDown.options.Clear();
            dropDown.options.AddRange(values.Select(v => new TMP_Dropdown.OptionData(v)));
        
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
    
        private void OnEnable()
        {
            OnChanged();
        }

        private void OnDisable()
        {
            signal.RemoveListener(OnChanged);
        }
        
        private void OnChanged()
        {
            dropDown.value = (int)(ValueType)signal.Value;
        }
    }
}