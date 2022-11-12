using System.Globalization;
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonBuilderEditor : EditorWindow
{
    static private readonly string pathSwitch = "Prefabs/Interactable/Switch";
    static private readonly string pathDoor = "Prefabs/Interactable/Door";


    [MenuItem("GameObject/Dungeon Helper/Switch and Door", false, -1)]
    public static void CreateSwitchAndDoor()
    {
        // Get instances
        Switch switchInstance = Resources.Load<Switch>(pathSwitch);
        Door doorInstance = Resources.Load<Door>(pathDoor);

        // Get spawn point
        Camera cam = SceneView.GetAllSceneCameras().First();
        Ray ray = new(cam.transform.position, cam.transform.forward);
        Plane plane = new(Vector3.up, Vector3.zero);

        Vector3 point = Vector3.zero;
        if (plane.Raycast(ray, out float enter))
        {
            point = ray.GetPoint(enter);
            point.x = Mathf.RoundToInt(point.x);
            point.y = Mathf.RoundToInt(point.y);
            point.z = Mathf.RoundToInt(point.z);
        }

        // Instantiate game objects
        Switch s = PrefabUtility.InstantiatePrefab(switchInstance) as Switch;
        s.transform.position = point + Vector3.back * 2;
        s.transform.parent = Selection.activeTransform;
        s.transform.SetAsLastSibling();

        Door d = PrefabUtility.InstantiatePrefab(doorInstance) as Door;
        d.transform.position = point + Vector3.forward * 2;
        d.transform.parent = Selection.activeTransform;
        d.transform.SetAsLastSibling();

        // Generate unique (hopefully) keys
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        const int keyLength = 4;
        string key = "";
        for (int i = 0; i < keyLength; i++)
            key += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];

        s.name = $"switch {key}";
        d.name = $"door {key}";

        // link switch to door
        UnityEditor.Events.UnityEventTools.AddPersistentListener(s.onActivate, d.OpenDoor);
    }
}
