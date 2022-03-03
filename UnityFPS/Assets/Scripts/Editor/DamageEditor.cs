using UnityEditor.IMGUI.Controls;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Damager))]
public class DamageEditor : Editor
{
    static BoxBoundsHandle s_BoxBoundsHandle = new BoxBoundsHandle();
    static Color s_EnabledColor = Color.green + Color.gray;


    SerializedProperty m_Damage;
    SerializedProperty m_HittableLayers;
    SerializedProperty m_OnDamageableHit;
    private void OnEnable()
    {

        m_HittableLayers = serializedObject.FindProperty("hittableLayers");
        m_Damage = serializedObject.FindProperty("damage");
        m_OnDamageableHit = serializedObject.FindProperty("OnDamageableHit");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        

        Damager myDamager = (Damager)target;
        myDamager.size = EditorGUILayout.Vector2Field("Size", myDamager.size);
        myDamager.offset = EditorGUILayout.Vector2Field("Offset", myDamager.offset);
        //myDamager.hittableLayers = EditorGUILayout.LabelField("hittableLayers", myDamager.hittableLayers.GetType().ToString());

    

        EditorGUILayout.PropertyField(m_HittableLayers);
        EditorGUILayout.PropertyField(m_Damage);
        EditorGUILayout.PropertyField(m_OnDamageableHit);

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        Damager myDamager = (Damager)target;
        if (!myDamager.enabled)
        {
            return;
        }

        Matrix4x4 handleMatrix = myDamager.transform.localToWorldMatrix;
        handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0),new Vector4(1f,1f,0f,1f)));
        handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(2, new Vector4(0f,0f,1f,myDamager.transform.position.z));

        using(new Handles.DrawingScope(handleMatrix))
        {
            s_BoxBoundsHandle.size = myDamager.size;
            s_BoxBoundsHandle.center = myDamager.offset;

            s_BoxBoundsHandle.SetColor(s_EnabledColor);
            EditorGUI.BeginChangeCheck();
            s_BoxBoundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(myDamager, "Modify Damager");

                myDamager.size = s_BoxBoundsHandle.size;
                myDamager.offset = s_BoxBoundsHandle.center;
            }

        }

    }
}
