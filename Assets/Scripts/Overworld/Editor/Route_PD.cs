using System;
using System.Reflection.Emit;
using UnityEngine;
using UnityEditor;

// IngredientDrawer
[CustomPropertyDrawer(typeof(Route))]
public class Route_PD : PropertyDrawer
{
    public const float LINE_HEIGHT = 20;

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Get each SerializedProperty
        SerializedProperty routeUnlockedProp = property.FindPropertyRelative("routeUnlocked");
        SerializedProperty nodeAProp = property.FindPropertyRelative("nodeA");
        SerializedProperty nodeBProp = property.FindPropertyRelative("nodeB");
        SerializedProperty dirAProp = property.FindPropertyRelative("directionA");
        SerializedProperty dirBProp = property.FindPropertyRelative("directionB");
        SerializedProperty pathProp = property.FindPropertyRelative("path");

        // Calculate Each rect
        int pathSize = pathProp.arraySize + 5;
        float nodeDirWidth = 65;

        // Route Unlocked
        Rect unlockedRect = new(position.x, position.y, position.width, LINE_HEIGHT);

        // Node A
        Rect nodeARect = new(position.x, unlockedRect.yMax, position.width - nodeDirWidth - 2, LINE_HEIGHT);
        Rect dirARect = new(nodeARect.xMax + 2, nodeARect.y, nodeDirWidth, LINE_HEIGHT);

        // Node B
        Rect nodeBRect = new(position.x, nodeARect.yMax, position.width - nodeDirWidth - 2, LINE_HEIGHT);
        Rect dirBRect = new(nodeBRect.xMax + 2, nodeBRect.y, nodeDirWidth, LINE_HEIGHT);

        // Path
        Rect generatePathRect = new(position.x, nodeBRect.yMax + 10, position.width, LINE_HEIGHT);
        Rect pathRect = new(position.x, generatePathRect.yMax + 2, position.width, pathSize * LINE_HEIGHT);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        label.text = "Route Unlocked";
        EditorGUI.PropertyField(unlockedRect, routeUnlockedProp, label);
        EditorGUI.PropertyField(nodeARect, nodeAProp, GUIContent.none);
        EditorGUI.PropertyField(dirARect, dirAProp, GUIContent.none);
        EditorGUI.PropertyField(nodeBRect, nodeBProp, GUIContent.none);
        EditorGUI.PropertyField(dirBRect, dirBProp, GUIContent.none);

        // Calculate path between both nodes when clicked
        if (GUI.Button(generatePathRect, "Generate Straight Path"))
        {
            LevelNode nodeA = nodeAProp.objectReferenceValue as LevelNode;
            LevelNode nodeB = nodeBProp.objectReferenceValue as LevelNode;

            if (nodeA != null && nodeB != null)
            {
                pathProp.ClearArray();
                pathProp.arraySize += 2;
                pathProp.GetArrayElementAtIndex(0).vector3Value = nodeA.transform.position;
                pathProp.GetArrayElementAtIndex(1).vector3Value = nodeB.transform.position;
            }
        }

        label.text = "Path";
        EditorGUI.PropertyField(pathRect, pathProp, label);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty path = property.FindPropertyRelative("path");
        int pathSize = Mathf.Max(path.arraySize, 1);
        float arraySize = (path.isExpanded ? pathSize + 3.5f : 2) * LINE_HEIGHT;
        return base.GetPropertyHeight(property, label) + arraySize + (3 * LINE_HEIGHT);
    }
}


