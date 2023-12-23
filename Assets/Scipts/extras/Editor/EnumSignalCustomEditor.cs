using System;
using UnityEditor;

namespace RSG.Trellis.Editor
{
    public class EnumSignalCustomEditor<T> : ManualSignalCustomEditor<T>
        where T : Enum
    {
        protected override T RenderField(T current)
        {
            return (T)EditorGUILayout.EnumPopup("New value", current);
        }
    }
}