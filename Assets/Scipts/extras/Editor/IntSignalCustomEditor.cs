using RSG.Trellis.Signals;
using UnityEditor;

namespace RSG.Trellis.Editor
{
    [CustomEditor(typeof(ManualSignal<int>), true)]
    public class IntSignalCustomEditor : ManualSignalCustomEditor<int>
    {
        protected override int RenderField(int current)
        {
            return EditorGUILayout.IntField("New value", current);
        }
    }
}