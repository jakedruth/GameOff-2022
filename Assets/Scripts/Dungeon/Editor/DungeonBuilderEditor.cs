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
    static private readonly string pathKey = "Prefabs/Interactable/Key";
    static private readonly string pathLockedDoor = "Prefabs/Interactable/LockedDoor";

    [MenuItem("GameObject/Dungeon Helper/Switch and Door", false, -1)]
    public static void CreateSwitchAndDoor()
    {
        // Get instances
        Switch switchInstance = Resources.Load<Switch>(pathSwitch);
        Door doorInstance = Resources.Load<Door>(pathDoor);

        // Instantiate game objects
        Vector3 point = GetSpawnPoint();
        Switch s = PrefabUtility.InstantiatePrefab(switchInstance) as Switch;
        s.transform.position = point + Vector3.back * 2;
        s.transform.parent = Selection.activeTransform;
        s.transform.SetAsLastSibling();

        Door d = PrefabUtility.InstantiatePrefab(doorInstance) as Door;
        d.transform.position = point + Vector3.forward * 2;
        d.transform.parent = Selection.activeTransform;
        d.transform.SetAsLastSibling();

        string key = GenerateUniqueKey();

        s.name = $"switch {key}";
        d.name = $"door {key}";

        // link switch to door
        UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(s.onActivate, d.SetDoorState, true);
    }

    [MenuItem("GameObject/Dungeon Helper/Key and Locked Door", false, -1)]
    public static void CreateKeyAndLockedDoor()
    {
        // Get instances
        Key keyInstance = Resources.Load<Key>(pathKey);
        Door doorInstance = Resources.Load<Door>(pathLockedDoor);

        // Instantiate game objects
        Vector3 point = GetSpawnPoint();
        Key k = PrefabUtility.InstantiatePrefab(keyInstance) as Key;
        k.transform.position = point + Vector3.back * 2;
        k.transform.parent = Selection.activeTransform;
        k.transform.SetAsLastSibling();

        Door d = PrefabUtility.InstantiatePrefab(doorInstance) as Door;
        d.transform.position = point + Vector3.forward * 2;
        d.transform.parent = Selection.activeTransform;
        d.transform.SetAsLastSibling();

        string key = GenerateUniqueKey();

        k.name = $"Key {key}";
        d.name = $"L_Door {key}";
    }

    private static Vector3 GetSpawnPoint()
    {
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

        return point;
    }

    private static string GenerateUniqueKey()
    {
        // Generate unique (hopefully) keys
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
        const int keyLength = 4;
        string key = "";
        for (int i = 0; i < keyLength; i++)
            key += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];

        return key;
    }
}
