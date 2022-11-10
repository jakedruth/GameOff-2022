using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VoxOnImport : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        if (assetPath.Contains("/VOX/") && assetPath.Contains(".obj"))
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            if (modelImporter != null)
                HandleModelImporter(modelImporter);
        }
    }

    void HandleModelImporter(ModelImporter modelImporter)
    {
        // Model Tab
        modelImporter.globalScale = 1.25f;
        modelImporter.importBlendShapes = false;
        modelImporter.importVisibility = false;
        modelImporter.importLights = false;
        modelImporter.importCameras = false;

        // Rig Tab
        modelImporter.animationType = ModelImporterAnimationType.None;

        // Animation Tab
        modelImporter.importAnimation = false;

        // Materials Tab
        // N/A
    }
}
