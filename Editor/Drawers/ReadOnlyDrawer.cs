using dninosores.UnityEditorAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MarioDebono.Inspector
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute), true)]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        private PropertyDrawer drawer;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (drawer != null)
            {
                return drawer.GetPropertyHeight(property, label);
            }
            else
            {

                return EditorGUI.GetPropertyHeight(property, label, property.hasChildren && property.isExpanded);
            }
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var wasEnabled = GUI.enabled;
            GUI.enabled = false;

            drawer = PropertyDrawerFinder.FindDrawerForProperty(property);
            
            if (drawer != null)
            {
                drawer.OnGUI(position, property, label);
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
            
            GUI.enabled = wasEnabled;
        }
    }
}
