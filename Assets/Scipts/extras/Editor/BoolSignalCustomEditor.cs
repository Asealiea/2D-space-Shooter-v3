using RSG.Trellis.Signals;
using UnityEditor;

namespace RSG.Trellis.Editor
{
    [CustomEditor(typeof(ManualSignal<bool>), true)]
    public class BoolSignalCustomEditor : ManualSignalCustomEditor<bool>
    {
        protected override bool RenderField(bool current)
        {
            return EditorGUILayout.Toggle("New value", current);
        }
    }
}