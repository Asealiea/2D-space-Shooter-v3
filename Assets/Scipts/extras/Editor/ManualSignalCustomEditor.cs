using RSG.Trellis.Signals;
using UnityEditor;
using UnityEngine;

namespace RSG.Trellis.Editor
{
    public abstract class ManualSignalCustomEditor<T> : BaseSignalCustomEditor
    {
        private T _editorValue;

        private bool _isInstantiated = false;
        
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) 
                return;
            
            if (!_isInstantiated)
            {
                _editorValue = ((ManualSignal<T>)target).Value;
                _isInstantiated = true;
            }

            EditorGUILayout.Space();
            GUILayout.Label("Runtime");
            var previousValue = _editorValue;
            _editorValue = RenderField(_editorValue);

            GlobalEditorOptions.AutoSet = EditorGUILayout.ToggleLeft("Set immediately (global option)", GlobalEditorOptions.AutoSet);
            if (GlobalEditorOptions.AutoSet &&!previousValue.Equals(_editorValue))
            {
                ((ManualSignal<T>)target).SetValue(_editorValue);
            }
            
            if (GUILayout.Button("Set (runtime)"))
            {
                ((ManualSignal<T>)target).SetValue(_editorValue);
            }
        }

        protected abstract T RenderField(T current);
    }
}