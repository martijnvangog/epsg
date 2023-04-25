using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CoordinateSytem))]
[CanEditMultipleObjects]
public class CoordinateSystemSetting : Editor
{

    int _choiceIndex = 0;
    string[] options = new string[1];
    SerializedProperty code;
    void OnEnable()
    {
        code = serializedObject.FindProperty("code");
        List<string> choiseList = new List<string>();
        choiseList.Add(1.ToString());
        choiseList.Add(2.ToString());
        options = choiseList.ToArray();

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _choiceIndex = EditorGUILayout.Popup(_choiceIndex, options);
        serializedObject.ApplyModifiedProperties();
    }
}