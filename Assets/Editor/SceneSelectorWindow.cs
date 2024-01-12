using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectorWindow : EditorWindow
{
    private int selectedSceneIndex = 0;

    [MenuItem("Window/Scene Selector")]
    public static void ShowWindow()
    {
        SceneSelectorWindow window = GetWindow<SceneSelectorWindow>("Scene Selector");
        window.SetupPosition();
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        GUILayout.Label("Select Scene", EditorStyles.boldLabel);

        string[] sceneNames = GetSceneNames();
        selectedSceneIndex = EditorGUILayout.Popup(selectedSceneIndex, sceneNames, EditorStyles.toolbarPopup);

        if (GUILayout.Button("Load Scene", EditorStyles.toolbarButton))
        {
            LoadSelectedScene();
        }

        GUILayout.EndHorizontal();
    }

    private void SetupPosition()
    {
        // Get the position of the account control tab
        Rect accountControlTabPosition = typeof(EditorApplication).GetMethod("GetWindowToolbarPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { }).GetType().GetProperty("accountControlRect").GetValue(null, null) is Rect ? (Rect)typeof(EditorApplication).GetMethod("GetWindowToolbarPosition", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { }).GetType().GetProperty("accountControlRect").GetValue(null, null) : default;

        // Set the position of the window to the right of the account control tab
        position = new Rect(accountControlTabPosition.x + accountControlTabPosition.width, accountControlTabPosition.y, 250, 100);
    }

    private string[] GetSceneNames()
    {
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene");
        string[] sceneNames = new string[sceneGuids.Length];

        for (int i = 0; i < sceneGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
            sceneNames[i] = GetSceneHierarchyName(path);
        }

        return sceneNames;
    }

    private string GetSceneHierarchyName(string path)
    {
        string[] folders = path.Split('/');
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(folders[folders.Length - 1]);
        return string.Join("/", folders, 0, folders.Length - 1) + "/" + sceneName;
    }

    private void LoadSelectedScene()
    {
        if (EditorUtility.DisplayDialog("Load Scene", "Do you want to load the selected scene?", "Yes", "No"))
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:Scene")[selectedSceneIndex]);
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
