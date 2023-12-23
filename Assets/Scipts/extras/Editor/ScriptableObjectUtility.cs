using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RSG.Trellis.Editor
{
    public static class ScriptableObjectUtility
    {
        public static void HydrateEmbedded(ScriptableObject target)
        {
            var hasChanges = false;
            var fields = GetEmbedFields(target);

            var targetPath = AssetDatabase.GetAssetPath(target);
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<EmbeddedAttribute>();
                var currentValue = field.GetValue(target) as Object;
                
                var path = currentValue != null ? AssetDatabase.GetAssetPath(currentValue) : "";
                
                if (targetPath != path)
                {
                    var value = ScriptableObject.CreateInstance(field.FieldType);
                    field.SetValue(target, value);
                    
                    AssetDatabase.AddObjectToAsset(value, target);
                    currentValue = value;
                    hasChanges = true;
                }

                var expectedName = $"{target.name} {attr.ChildName ?? field.Name}";
                if (currentValue && currentValue.name != expectedName)
                {
                    currentValue.name = expectedName;
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }

        public static void ClearEmbeds(ScriptableObject target)
        {
            var fields = GetEmbedFields(target);

            var targetPath = AssetDatabase.GetAssetPath(target);
            foreach (var field in fields)
            {
                var currentValue = field.GetValue(target) as Object;
                var path = currentValue != null ? AssetDatabase.GetAssetPath(currentValue) : "";
                
                if (targetPath == path)
                {
                    AssetDatabase.RemoveObjectFromAsset(currentValue);
                    field.SetValue(target, null);
                }
            }
            
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }

        public static IEnumerable<FieldInfo> GetEmbedFields(ScriptableObject target)
        {
            var fields = target.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(field => field.GetCustomAttribute<EmbeddedAttribute>() != null)
                .ToArray();
            return fields;
        }
    }
}