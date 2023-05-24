using System;
using UnityEngine;

namespace MarioDebono.Inspector
{
    public class ButtonAttribute : Attribute
    {
        public readonly string text;
        public ButtonAttribute(string text = "button")
        {
            this.text = text;
        }
    }
}
