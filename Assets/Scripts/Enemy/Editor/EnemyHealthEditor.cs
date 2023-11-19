using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyHealth))]
public class EnemyHealthEditor : Editor
{
    private int _damagePoint;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(new GUIContent("Damage point", "Use it as testing"), GUILayout.ExpandWidth(false));
        _damagePoint = EditorGUILayout.IntField(_damagePoint);

        EditorGUILayout.EndHorizontal();

        EnemyHealth playerHealth = (EnemyHealth)target;
        if (GUILayout.Button("Damage Enemy"))
        {
            playerHealth.TakeDamage(_damagePoint);
        }
    }
}
