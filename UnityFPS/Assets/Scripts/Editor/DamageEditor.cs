using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Damager))]
public class DamageEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();
    }
}
