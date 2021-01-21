using UnityEditor;
using UnityEngine;

/// <summary>
/// A calss for editing enemies count through editor
/// </summary>
public class EnemiesEditorManager : EditorWindow
{
    private string minEnemies;
    private string maxEnemies;

    [MenuItem("Tools/Enemies Manager")]
    public static void ShowWindow()
    {
        GetWindow<EnemiesEditorManager>("Enemies Manager");
    }

    private void Awake()
    {
        minEnemies = PlayerPrefs.GetInt(PrefsNames.ENEMIES_MIN_VALUE, 2).ToString();
        maxEnemies = PlayerPrefs.GetInt(PrefsNames.ENEMIES_MAX_VALUE, 4).ToString();
    }                

    private void OnGUI()
    {
        GUILayout.Label("Set min enemies planets count (including player, so enemies will be one less)");
        minEnemies = EditorGUILayout.TextField("Min enemies count: ", minEnemies);

        if (GUILayout.Button("Set"))
        {
            if (int.Parse(maxEnemies) <= int.Parse(minEnemies))
            {
                Debug.Log("Max could not be less or equal min!");
                return;
            }

            PlayerPrefs.SetInt(PrefsNames.ENEMIES_MIN_VALUE, int.Parse(minEnemies));
        }

        GUILayout.Space(10);

        GUILayout.Label("Set max enemies planets count (including player, so enemies will be one less)");
        maxEnemies = EditorGUILayout.TextField("Max enemies count: ", maxEnemies);

        if (GUILayout.Button("Set"))
        {
            if (int.Parse(maxEnemies) <= int.Parse(minEnemies))
            {
                Debug.Log("Max could not be less or equal min!");
                return;
            }

            PlayerPrefs.SetInt(PrefsNames.ENEMIES_MAX_VALUE, int.Parse(maxEnemies));
        }

        GUILayout.Space(10);

        GUILayout.Label("Current min enemies planets count: " + PlayerPrefs.GetInt(PrefsNames.ENEMIES_MIN_VALUE, 2));
        GUILayout.Label("Current max enemies planets count: " + PlayerPrefs.GetInt(PrefsNames.ENEMIES_MAX_VALUE, 4));
    }
}
