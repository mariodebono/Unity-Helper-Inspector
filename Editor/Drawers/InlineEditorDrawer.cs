using dninosores.UnityEditorAttributes;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace MarioDebono.Inspector
{
    [CustomPropertyDrawer(typeof(InlineEditorAttribute), true)]
    public class InlineEditorDrawer : PropertyDrawer
    {
        List<SerializedProperty> properties = new List<SerializedProperty>();
        const float SPACE_EXPAND = 5;
        const float SPACE_BETWEEN = 3;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {

            float height = base.GetPropertyHeight(property, label);

            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                if (property.isExpanded)
                {
                    foreach (SerializedProperty a in property)
                    {
                        height += EditorGUI.GetPropertyHeight(a, true);
                        height += SPACE_BETWEEN; //add some space between
                    }


                    height -= SPACE_BETWEEN; //remove end Space
                    height += (SPACE_EXPAND * 2);
                }

                return height;
            }

            height += SPACE_EXPAND * 2; // space above and below

            if (property.isExpanded && properties.Count > 0)
            {
                properties.ForEach(p =>
                    {
                        height += EditorGUI.GetPropertyHeight(p, true);
                        height += SPACE_BETWEEN; //add some space between

                    });

                height -= SPACE_BETWEEN; //remove end Space
                height += SPACE_EXPAND; // space after foldout
            }
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                position.height = EditorGUI.GetPropertyHeight(property, label);
                
                EditorGUI.BeginChangeCheck();
                
                EditorGUI.PropertyField(position, property, label, true);
                
                if (EditorGUI.EndChangeCheck())
                    property.serializedObject.ApplyModifiedProperties();
                
                return;
            }


            GUI.Box(position, GUIContent.none);

            position.y += SPACE_EXPAND; // space before field

            Rect foldoutPosition = position;
            foldoutPosition.width = EditorGUIUtility.labelWidth;
            foldoutPosition.height = EditorGUIUtility.singleLineHeight;

            if (property.objectReferenceValue != null)
                property.isExpanded = EditorGUI.Foldout(foldoutPosition, property.isExpanded, GUIContent.none, true);

            if (property.objectReferenceValue == null)
            {
                property.isExpanded = false;
            }

            Rect labelPosition = position;
            labelPosition.x += 5;
            labelPosition.width = EditorGUIUtility.labelWidth;

            EditorGUI.PrefixLabel(labelPosition, label);

            Rect fieldPosition = position;
            fieldPosition.x += EditorGUIUtility.labelWidth;
            fieldPosition.height = EditorGUIUtility.singleLineHeight;
            fieldPosition.width = EditorGUIUtility.currentViewWidth - fieldPosition.x - 5;

            EditorGUI.BeginChangeCheck();

            EditorGUI.PropertyField(fieldPosition, property, GUIContent.none);

            if (property.isExpanded && property.objectReferenceValue != null)
            {
                Rect childPosition = position;

                childPosition.x += 10;
                childPosition.y += EditorGUIUtility.singleLineHeight + SPACE_EXPAND;

                // correct label space for children
                var labelWidth = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = labelWidth - 10; // as we added 10 before

                SerializedObject so = new SerializedObject(property.objectReferenceValue);

                properties.Clear();

                var iterator = so.GetIterator();
                if (iterator.NextVisible(true))
                {
                    do
                    {
                        var propertyField = so.FindProperty(iterator.propertyPath);
                        if (propertyField.name == "m_Script") continue;

                        properties.Add(propertyField);

                        float height = EditorGUI.GetPropertyHeight(propertyField, true);

                        Rect box = childPosition;
                        box.height = height;
                        box.width = EditorGUIUtility.currentViewWidth - box.x - 5; // keep 5 from the edge

                        Rect fieldChildPosition = childPosition;
                        fieldChildPosition.width = EditorGUIUtility.currentViewWidth - fieldChildPosition.x - 5;
                        fieldChildPosition.height = height;

                        EditorGUI.BeginChangeCheck();
                        EditorGUI.PropertyField(fieldChildPosition, propertyField, new GUIContent(propertyField.displayName));

                        if (EditorGUI.EndChangeCheck())
                            propertyField.serializedObject.ApplyModifiedProperties();

                        childPosition.y += height + SPACE_BETWEEN;

                    }
                    while (iterator.NextVisible(false));

                }

                EditorGUIUtility.labelWidth = labelWidth;
            }

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();


        }
    }
}
