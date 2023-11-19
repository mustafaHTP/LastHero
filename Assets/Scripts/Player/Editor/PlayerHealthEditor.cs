using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerHealth))]
public class PlayerHealthEditor : Editor
{
    private int _damagePoint;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Damage point", "Use it as testing"), GUILayout.ExpandWidth(false));
        _damagePoint = EditorGUILayout.IntField(_damagePoint);

        EditorGUILayout.EndHorizontal();

        PlayerHealth playerHealth = (PlayerHealth)target;
        if (GUILayout.Button("Damage Player"))
        {
            playerHealth.TakeDamage(_damagePoint);
        }
    }
}
