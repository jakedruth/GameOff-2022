using UnityEditor;
using UnityEngine;

namespace JDR.Utils
{
    [CustomPropertyDrawer(typeof(PlusMinusAttribute))]
    public class PlusMinusDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PlusMinusAttribute plusMinus = attribute as PlusMinusAttribute;

            const float padding = 5f;
            float buttonWidth = position.height;

            position.width -= (padding + buttonWidth) * 2;
            int value = EditorGUI.IntField(position, label, property.intValue);

            Rect minusRect = new Rect(position.x + position.width + padding, position.y, buttonWidth, position.height);
            Rect plusRect = new Rect(minusRect.max.x + padding, minusRect.y, buttonWidth, position.height);

            if (GUI.Button(minusRect, "-"))
            {
                if (plusMinus != null && value > plusMinus.clampMin)
                    value--;
            }

            if (GUI.Button(plusRect, "+"))
            {
                if (plusMinus != null && value < plusMinus.clampMax)
                    value++;
            }

            property.intValue = value;
        }
    }
}