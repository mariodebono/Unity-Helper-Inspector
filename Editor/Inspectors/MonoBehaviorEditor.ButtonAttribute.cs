using Codice.CM.SEIDInfo;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MarioDebono.Inspector
{

    public partial class MonoBehaviorEditor : Editor
    {
        bool buttonHeaderSet = false;

        private void ClearButtonHeader()
        {
            buttonHeaderSet = false;
        }

        private void ProcessButtonAttribute(MethodInfo method)
        {
            ButtonAttribute buttonAttribute = Attribute.GetCustomAttribute(method, typeof(ButtonAttribute)) as ButtonAttribute;

            if (buttonAttribute != null)
            {
                if (!buttonHeaderSet)
                {
                    buttonHeaderSet = true;
                    EditorGUILayout.Space(10);
                    EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);
                }
                var parameters = method.GetParameters();
                object[] methodParams;

                if (!methodParameters.TryGetValue(method, out methodParams))
                {
                    methodParams = new object[parameters.Length];
                    methodParameters.Add(method, methodParams);
                }

                if (parameters.Length > 0)
                {

                    var rectHeight = EditorGUIUtility.singleLineHeight; // foldout height
                    rectHeight += 10; // additional padding around
                    rectHeight += 3; // additional 3 for foldout height;
                    if (buttonOpen)
                    {
                        rectHeight += (EditorGUIUtility.singleLineHeight + 3) * parameters.Length; // all parameters with 3 space
                    }

                    var position = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, rectHeight);

                    GUI.Box(position, "");
                    position.y += 5; // for padding inside box

                    Rect labelRect = position;
                    labelRect.width = EditorGUIUtility.labelWidth;
                    labelRect.height += 3;


                    Rect foldoutPosition = position;
                    foldoutPosition.width = EditorGUIUtility.labelWidth;
                    foldoutPosition.height = EditorGUIUtility.singleLineHeight + 3;

                    buttonOpen = EditorGUI.Foldout(foldoutPosition, buttonOpen, GUIContent.none, true);

                    Rect labelPosition = position;
                    labelPosition.x += 14;
                    labelPosition.height += 3;
                    EditorGUI.PrefixLabel(labelPosition, new GUIContent(buttonAttribute.text), EditorStyles.boldLabel);

                    Rect fieldPosition = position;
                    fieldPosition.x = EditorGUIUtility.currentViewWidth - 5 - 100;
                    fieldPosition.height = EditorGUIUtility.singleLineHeight + 3;
                    fieldPosition.width = 100;

                    if (GUI.Button(fieldPosition, "Invoke"))
                    {
                        foreach (UnityEngine.Object methodTarget in targets)
                            method.Invoke(methodTarget, methodParams);
                    }

                    // DRAW PARAMETERS
                    if (buttonOpen)
                    {
                        //position.y += EditorGUIUtility.singleLineHeight + 3;
                        Rect parameterPos = position;
                        parameterPos.y += 5;
                        parameterPos.x += 5;
                        parameterPos.width = EditorGUIUtility.currentViewWidth - parameterPos.x - 5;
                        parameterPos.height = EditorGUIUtility.singleLineHeight;

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var param = parameters[i];
                            object methodParam = methodParams[i];
                            parameterPos.y += EditorGUIUtility.singleLineHeight + 3;

                            switch (param.ParameterType.Name)
                            {
                                case "String":
                                    methodParam = EditorGUI.TextField(parameterPos, param.Name, methodParam as string);
                                    break;

                                case "Int32":
                                    if (methodParam == null) methodParam = 0;
                                    methodParam = EditorGUI.IntField(parameterPos, param.Name, (int)methodParam);
                                    break;

                                case "Single":
                                    if (methodParam == null) methodParam = 0f;
                                    methodParam = EditorGUI.FloatField(parameterPos, param.Name, (float)methodParam);
                                    break;

                                default:
                                    if (param.ParameterType.IsSubclassOf(typeof(UnityEngine.Object)))
                                    {
                                        if (methodParam == null) methodParam = default;
                                        methodParam = EditorGUI.ObjectField(parameterPos, param.Name, (UnityEngine.Object)methodParam, param.ParameterType, true);
                                    }
                                    else
                                    {
                                        if (methodParam == null) methodParam = default;//Activator.CreateInstance(param.ParameterType);

                                        EditorGUI.LabelField(parameterPos, $"{param.Name}", $"Not Supported ({param.ParameterType.Name})");
                                        //Debug.Log($"{i}: {param.Name} | {param.ParameterType.Name}");
                                    }

                                    break;
                            }

                            methodParams[i] = methodParam;
                        }

                        methodParameters[method] = methodParams;
                    }

                }
                else
                {
                    if (GUILayout.Button(buttonAttribute.text, new[] { GUILayout.Height(EditorGUIUtility.singleLineHeight + 6) }))
                    {
                        foreach (UnityEngine.Object methodTarget in targets)
                            method.Invoke(methodTarget, null);
                    }
                }


            }
        }
    }
}
