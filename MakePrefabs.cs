//Copyright (c) Melissa Osorio & Mercedes Senties. All Rights reserved

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MakePrefabs : EditorWindow
{
    private Dictionary<GameObject, bool> _objectSelections = new Dictionary<GameObject, bool>();
    private Vector2 scrollPosition = Vector2.zero;


    [MenuItem("Tools/Make Scene Objects Prefabs")]
    static void Init()
    {
        //Make pop up window
        MakePrefabs tool = (MakePrefabs)GetWindow(typeof(MakePrefabs));
        tool.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Scene objects:", EditorStyles.boldLabel);

        //Find all objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        //Begin the scrollview
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - 50));

        //Display a toggle for each object in the scene
        foreach (GameObject obj in allObjects)
        {
            bool isSelected;
            if (!_objectSelections.TryGetValue(obj, out isSelected))
            {
                isSelected = false;
                _objectSelections[obj] = isSelected;
            }

            _objectSelections[obj] = GUILayout.Toggle(isSelected, obj.name);
        }

        //Make a button to make the selected objects prefabs
        bool okBtn = GUILayout.Button("Make prefabs", GUILayout.Width(100), GUILayout.Height(30));
        if (okBtn) {MakeSelectedObjectsPrefabs();}

        //End the scrollview
        EditorGUILayout.EndScrollView();
    }

    private void MakeSelectedObjectsPrefabs()
    {
        //Check if the "Prefabs" folder exists
        string folderPath = "Assets/Prefabs";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        }

        //Loop through all objects and check if they are selected and not already prefabs
        foreach (GameObject obj in _objectSelections.Keys)
        {
            bool isSelected = _objectSelections[obj];
            if (isSelected && PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.NotAPrefab)
            {
                //If the object is not a prefab, create a prefab for it and replace the object in the scene for the prefab
                GameObject newPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(obj, folderPath + "/" + obj.name + ".prefab", InteractionMode.UserAction);
                Debug.Log(obj.name + " was not a prefab and has been saved as a new prefab.");
            }
        }
    }
}
