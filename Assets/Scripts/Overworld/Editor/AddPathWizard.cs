using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using JDR.ExtensionMethods;

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

    public CompassDirection cd1;
    public CompassDirection cd2;

    private void OnGUI()
    {
        GUILayout.Space(5f);
        GUILayout.Label("Calculate a path between two level nodes");
        nodeA = EditorGUILayout.ObjectField("Node A", nodeA, typeof(LevelNode), true) as LevelNode;
        nodeB = EditorGUILayout.ObjectField("Node B", nodeB, typeof(LevelNode), true) as LevelNode;
        //GUILayout.Space(10f);
        HandlePathCreation();

        GUILayout.Space(30f);
        GUILayout.Label("Swap two trails on Node A");
        cd1 = (CompassDirection)EditorGUILayout.EnumPopup("trail 1", cd1);
        cd2 = (CompassDirection)EditorGUILayout.EnumPopup("trail 2", cd2);
        if (GUILayout.Button("Swap Trails", GUILayout.Width(200), GUILayout.Height(25)))
        {
            bool success = nodeA.SwapTrails(cd1, cd2);
            if (!success)
                Debug.LogWarning("The trails were not swapped successfully");
        }
    }

    private void HandlePathCreation()
    {
        if (GUILayout.Button("Calculate and add path", GUILayout.Width(200), GUILayout.Height(25)))
        {
            // TODO: Handle an undo when creating a new path
            // When a path is created in this method, hitting undo (ctrl+z) does nothing
            // Also, the overworld manager gizmo does not update until clicking away first

            // Use reflection to get access to the private list of paths
            OverworldManager manager = FindObjectOfType<OverworldManager>();
            Type t = typeof(OverworldManager);
            FieldInfo fieldInfo = t.GetField("paths", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<Path> paths = (List<Path>)fieldInfo.GetValue(manager);

            // Create a new path
            Path newPath = new();
            newPath.GenerateStraightPath(nodeA, nodeB);

            // Check to see if the path exists first
            bool validPath = true;
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i][0] == newPath[0] &&
                    paths[i][^1] == newPath[^1])
                {
                    validPath = false;
                    Debug.LogWarning($"The path between these nodes already exists at index [{i}]");
                    break;
                }
            }

            if (!validPath)
                return;

            paths.Add(newPath);
            int index = paths.Count - 1;

            // Set the trail data for nodeA
            Trail trailA = nodeA.GetTrail((nodeB.transform.position - nodeA.transform.position).ToClosestCompassDirection());
            trailA.pathIndex = index;
            trailA.targetNode = nodeB;
            trailA.invertPath = false;

            // Set the trail data for nodeB
            Trail trailB = nodeB.GetTrail((nodeA.transform.position - nodeB.transform.position).ToClosestCompassDirection());
            trailB.pathIndex = index;
            trailB.targetNode = nodeA;
            trailB.invertPath = true;
        }
    }
}
