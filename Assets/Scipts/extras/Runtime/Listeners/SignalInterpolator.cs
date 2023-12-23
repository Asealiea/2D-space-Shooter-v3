using RSG.Trellis.Signals;
using TMPro;
using UnityEngine;

namespace RSG.Trellis.Listeners
{
    public class SignalInterpolator : MonoBehaviour
    {
        [SerializeField] private bool syncOnStart = true;
        [SerializeField] private Signal[] signals;
        [SerializeField] private TMP_Text target;
        [SerializeField, Multiline] private string template;

        private object[] _signalValues;

        private void OnEnable()
        {
            _signalValues = new object[signals.Length];
            
            foreach (var signal in signals)
            {
                signal.AddListener(OnChanged, syncOnStart);
            }
        }

        private void OnDisable()
        {
            foreach (var signal in signals)
            {
                signal.RemoveListener(OnChanged);
            }

            _signalValues = new object[signals.Length];
        }

        private void OnChanged()
        {
            for (var i = 0; i < signals.Length; i++)
                _signalValues[i] = signals[i].UntypedValue;

            target.text = string.Format(template, args: _signalValues);
        }
    }
}