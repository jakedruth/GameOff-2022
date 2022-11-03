using UnityEngine;
using UnityEditor;

namespace JDR.Utils
{
    [CustomPropertyDrawer(typeof(RangedFloat), true)]
    public class RangedFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            SerializedProperty minProp = property.FindPropertyRelative("minimum");
            SerializedProperty maxProp = property.FindPropertyRelative("maximum");

            float minValue = minProp.floatValue;
            float maxValue = maxProp.floatValue;

            float rangeMin = 0;
            float rangeMax = 1;

            MinMaxRangeAttribute[] ranges = (MinMaxRangeAttribute[])fieldInfo.GetCustomAttributes(typeof(MinMaxRangeAttribute), true);
            if (ranges.Length > 0)
            {
                rangeMin = ranges[0].Min;
                rangeMax = ranges[0].Max;
            }

            const float rangeBoundsLabelWidth = 45f;
            const float buffer = 5;

            EditorGUI.BeginChangeCheck();
            Rect rangeBoundsLabel1Rect = new Rect(position);
            rangeBoundsLabel1Rect.width = rangeBoundsLabelWidth;
            minValue = EditorGUI.FloatField(rangeBoundsLabel1Rect, minValue);
            position.xMin += (rangeBoundsLabelWidth + buffer);

            Rect rangeBoundsLabel2Rect = new Rect(position);
            rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth;
            maxValue = EditorGUI.FloatField(rangeBoundsLabel2Rect, maxValue);

            position.xMax -= rangeBoundsLabelWidth + buffer;

            EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);
            if (EditorGUI.EndChangeCheck())
            {
                minProp.floatValue = minValue;
                maxProp.floatValue = maxValue;
            }

            EditorGUI.EndProperty();
        }
    }
}