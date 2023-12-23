using RSG.Trellis.Signals;
using UnityEditor;

namespace RSG.Trellis.Editor
{
    [CustomEditor(typeof(ManualSignal<string>), true)]
    public class StringSignalCustomEditor : ManualSignalCustomEditor<string>
    {
        protected override string RenderField(string current)
        {
            return EditorGUILayout.TextField("New value", current);
        }
    }
}