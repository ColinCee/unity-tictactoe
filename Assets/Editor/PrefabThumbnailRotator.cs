using UnityEngine;
using UnityEditor;
using System.IO;

public class PrefabThumbnailRotator : EditorWindow
{
    private string targetFolder = "Assets/";

    [MenuItem("Tools/Rotate Prefab Thumbnails")]
    public static void ShowWindow()
    {
        GetWindow<PrefabThumbnailRotator>("Prefab Thumbnail Rotator");
    }

    void OnGUI()
    {
        GUILayout.Label("Rotate Prefab Thumbnails", EditorStyles.boldLabel);
        
        targetFolder = EditorGUILayout.TextField("Target Folder", targetFolder);
        
        if (GUILayout.Button("Select Folder"))
        {
            string selectedPath = EditorUtility.OpenFolderPanel("Select Prefab Folder", "Assets", "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                // Convert absolute path to relative project path
                targetFolder = "Assets" + selectedPath.Substring(Application.dataPath.Length);
            }
        }

        if (GUILayout.Button("Rotate Prefabs in Folder"))
        {
            RotatePrefabsInFolder(targetFolder);
        }
    }

    void RotatePrefabsInFolder(string folderPath)
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError("Invalid folder path: " + folderPath);
            return;
        }

        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        
        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = PrefabUtility.LoadPrefabContents(assetPath);

            // Rotate the root GameObject
            prefab.transform.rotation = Quaternion.Euler(-90, 0, 0);

            // Save the modified prefab
            PrefabUtility.SaveAsPrefabAsset(prefab, assetPath);
            PrefabUtility.UnloadPrefabContents(prefab);

            // Force Unity to update the thumbnail
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);

            // Additional steps to refresh the thumbnail
            EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<GameObject>(assetPath));
            AssetDatabase.SaveAssets();
        }

        AssetDatabase.Refresh();
        Debug.Log($"Rotated {prefabGuids.Length} prefabs in {folderPath}");

        // Force Unity to repaint the project window
        EditorApplication.RepaintProjectWindow();
    }
}
