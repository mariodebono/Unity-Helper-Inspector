using dninosores.UnityEditorAttributes;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.Scripting;

namespace MarioDebono.Inspector
{
    [CustomPropertyDrawer(typeof(InterfaceAttribute), true)]
    public class InterfaceAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                height += EditorGUIUtility.singleLineHeight;
            }

            return height;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            // Check if reference type property.
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                // Get attribute
                var restrictAttribute = attribute as InterfaceAttribute;

                // Draw property
                EditorGUI.BeginProperty(position, label, property);
                property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, restrictAttribute.restrictedType, true);
                EditorGUI.EndProperty();
            }
            else
            {
                // If field is not reference show a nice error
                var previousColor = GUI.color;
                EditorGUI.PrefixLabel(position, label);
                GUI.color = new Color(0.9f, 0.2666f, 0.196f);
                // Show error
                position.x += EditorGUIUtility.labelWidth;

                // draw a nice box
                Rect boxPosition = position;
                //boxPosition.height += EditorGUIUtility.singleLineHeight;
                GUI.Box(boxPosition, "");

                position.width -= EditorGUIUtility.labelWidth;
                position.x += 5;
                position.width -= 5;
                position.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(position, $"Invalid property type '{property.propertyType}'.");
                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.LabelField(position, $"Use type 'UnityEngine.Object'");

                GUI.color = previousColor;
            }

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
        }
    }
}
