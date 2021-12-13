using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TDZone))]
public class TDzonecustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        TDZone tdZone = (TDZone)target;
        tdZone.florname = GUILayout.TextField(tdZone.florname);
        if (GUILayout.Button("Create New Floor"))
        {
            tdZone.OnDo();
        }
        if (GUILayout.Button("Create New Trigger"))
        {
            tdZone.OnTrigCreate();
        }
        if (GUILayout.Button("Change Ceiling Shadow Materials"))
        {
            tdZone.OnCeilingTint();
        }
        DrawDefaultInspector();
    }
}
