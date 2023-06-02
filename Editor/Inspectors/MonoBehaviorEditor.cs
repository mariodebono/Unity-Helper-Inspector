using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MarioDebono.Inspector
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public partial class MonoBehaviorEditor : Editor
    {
        Dictionary<MethodInfo, object[]> methodParameters = new Dictionary<MethodInfo, object[]>();
        private bool buttonOpen;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            foreach (var target in targets)
            {
                IEnumerable<MethodInfo> methods = target.GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                ClearButtonHeader();
                foreach (MethodInfo method in methods)
                {
                    ProcessButtonAttribute(method);
                }
            }
        }
    }
}
