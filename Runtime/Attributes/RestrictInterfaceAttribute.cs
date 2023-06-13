using System;
using UnityEngine;

namespace MarioDebono.Inspector
{
    /// <summary>
    /// An Attribute that will restrict to objects that implement an interface
    /// </summary>
    public class InterfaceAttribute : PropertyAttribute
    {
        public Type restrictedType { get; private set; }
        public InterfaceAttribute(Type type)
        {
            this.restrictedType = type;
        }
    }
}
