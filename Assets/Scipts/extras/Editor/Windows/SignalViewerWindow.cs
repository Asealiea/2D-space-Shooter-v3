using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Trellis.Signals;
using RSG.Trellis.Signals.Derived;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RSG.Trellis.Editor.Windows
{
    
    public class SignalViewerWindow : EditorWindow
    {
        private Dictionary<Object, UnityEditor.Editor> _childEditors = new Dictionary<Object, UnityEditor.Editor>();

        private UnityEditor.Editor GetOrCreateChildEditor(Object o)
        {
            if (!_childEditors.ContainsKey(o))
            {
                _childEditors[o] = UnityEditor.Editor.CreateEditor(o);
            }

            return _childEditors[o];
        }
        
        [MenuItem("RSG/Signals...")]
        public static void OpenWindow()
        {
            var window = CreateWindow<SignalViewerWindow>("RSG signal viewer");
            window.Show();
        }

        private void OnGUI()
        {
            var selectionButtonStyle = new GUIStyle(EditorStyles.miniButton) { fixedWidth = 30 };
            var typeStyle = new GUIStyle(EditorStyles.label) { fixedWidth = 100};
            var nameStyle = new GUIStyle(EditorStyles.label) { fixedWidth = 250};
            var valueStyle = new GUIStyle(EditorStyles.label) { fixedWidth = 250};
            
            var signals = Resources.FindObjectsOfTypeAll<Signal>();
            
            GUILayout.Label("Signals");

            GUILayout.BeginVertical();
            foreach (var signal in signals)
            {
                var isSelected = signal == Selection.activeObject;
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(isSelected ? "*" : "", selectionButtonStyle))
                {
                    Selection.activeObject = signal;
                }

                var typeName = signal.GetType().Name;
                typeName = typeName.Replace("Signal", String.Empty);
                if (signal is IDerivedSignal)
                {
                    typeName += "*";
                }

                GUILayout.Label(typeName, typeStyle);
                GUILayout.Label(signal.name, nameStyle);
                var valueLabel = signal.UntypedValue?.ToString() ?? "<none>";
                if (signal is IDerivedSignal derived)
                {
                    var dependeeNames = derived.Dependees.Select(d => d.name).ToArray();

                    valueLabel += $" (from {string.Join(", ", dependeeNames)})";
                }
                GUILayout.Label(valueLabel, valueStyle);
                GUILayout.EndHorizontal();
            }

            if (Selection.activeObject is Signal)
            {
                GUILayout.Space(10);
                GUILayout.Label("Edit signal");
                var editor = GetOrCreateChildEditor(Selection.activeObject);
                editor.OnInspectorGUI();
            }
            
            GUILayout.EndVertical();
        }
        
        private void OnDestroy()
        {
            foreach (var editor in _childEditors.Values.ToArray())
            {
                DestroyImmediate(editor);
            }
        }
    }
}