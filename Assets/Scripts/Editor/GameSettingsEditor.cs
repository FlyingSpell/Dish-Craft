using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameSettings))]
public class GameSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if(GUILayout.Button("Delete all saves"))
        {
            PlayerPrefs.DeleteAll();

            Debug.Log("All saves was successfully deleted!");
        }
    }
}