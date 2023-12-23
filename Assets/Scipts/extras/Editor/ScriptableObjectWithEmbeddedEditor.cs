using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RSG.Trellis.Editor
{
    [CustomEditor(typeof(ScriptableObjectWithEmbedded), true)]
    public class ScriptableObjectWithEmbeddedEditor : UnityEditor.Editor
    {
        private readonly HashSet<string> _collapsed = new HashSet<string>();
        private Dictionary<Object, UnityEditor.Editor> _childEditors = new Dictionary<Object, UnityEditor.Editor>();

        private UnityEditor.Editor GetOrCreateChildEditor(Object o)
        {
            if (!_childEditors.ContainsKey(o))
            {
                _childEditors[o] = CreateEditor(o);
            }

            return _childEditors[o];
        }

        private void OnDestroy()
        {
            foreach (var editor in _childEditors.Values.ToArray())
            {
                DestroyImmediate(editor);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var obj = (ScriptableObject)target;
            if (GUILayout.Button("Update embedded fields"))
            {
                ScriptableObjectUtility.HydrateEmbedded(obj);
            }
            
            if (GUILayout.Button("Detach embedded fields"))
            {
                ScriptableObjectUtility.ClearEmbeds(obj);
            }

            foreach (var field in ScriptableObjectUtility.GetEmbedFields(obj))
            {
                var value = field.GetValue(obj) as Object;
                if (!value)
                    continue;
                
                var expanded = EditorGUILayout.BeginFoldoutHeaderGroup(!_collapsed.Contains(field.Name), field.Name);
                if (expanded)
                {
                    EditorGUI.indentLevel++;

                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.ObjectField("Object", value, value.GetType());
                    EditorGUI.EndDisabledGroup();
                    _collapsed.Remove(field.Name);

                    // Bug: Selecting a child editor will be blank until you click away and back
                    var childEditor = GetOrCreateChildEditor(value);
                    childEditor.OnInspectorGUI();

                    EditorGUI.indentLevel--;
                }
                else
                {
                    _collapsed.Add(field.Name);
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
        }
    }
}