using RSG.Trellis.Signals;
using UnityEditor;
using UnityEngine;

namespace RSG.Trellis.Editor
{
    [CustomEditor(typeof(Signal), true)]
    public class BaseSignalCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                EditorGUI.BeginDisabledGroup(true);
            }

            base.OnInspectorGUI();

            if (Application.isPlaying)
            {
                EditorGUI.EndDisabledGroup();
            }

            var typedTarget = (Signal)target;
            EditorGUILayout.LabelField("Current value", typedTarget.UntypedValue?.ToString() ?? "<null>");
        }
    }
}