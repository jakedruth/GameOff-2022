using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddPathWizard : EditorWindow
{

    [MenuItem("GameOff-2022/AddPathWizard")]
    private static void ShowWindow()
    {
        var window = GetWindow<AddPathWizard>();
        window.titleContent = new GUIContent("Add Path");
        window.Show();
    }

    public LevelNode nodeA;
    public LevelNode nodeB;

    private void OnGUI()
    {
        GUILayout.Space(5f);
        GUILayout.Label("Calculate a path between two level nodes");
        GUILayout.Space(10f);
        EditorGUI.indentLevel = 5;
        nodeA = EditorGUILayout.ObjectField("Node A", nodeA, typeof(LevelNode), true) as LevelNode;
        nodeB = EditorGUILayout.ObjectField("Node B", nodeB, typeof(LevelNode), true) as LevelNode;
        if (GUILayout.Button("Calculate and add path"))
        {
            // TODO: Handle an undo when creating a new path
            // When a path is created in this method, hitting undo (ctrl+z) does nothing
            // Also, the overworld manager gizmo does not update until clicking away first
            OverworldManager manager = FindObjectOfType<OverworldManager>();
            Path newPath = new();
            newPath.GenerateStraightPath(nodeA, nodeB);
            manager.paths.Add(newPath);
        }
    }
}
