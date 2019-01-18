using UnityEngine;
using UnityEditor;
using Gamekit2D;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(FinalKill))]
public class FinalKillEditor : Editor {

    
    static BoxBoundsHandle s_BoxBoundsHandle = new BoxBoundsHandle();
    static Color s_EnabledColor = Color.yellow;// + Color.grey;

    SerializedProperty m_DamageProp;
    SerializedProperty m_OffsetProp;
    SerializedProperty m_SizeProp;
    SerializedProperty m_OffsetBasedOnSpriteFacingProp;
    SerializedProperty m_SpriteRendererProp;
    SerializedProperty m_CanHitTriggersProp;
    SerializedProperty m_ForceRespawnProp;
    SerializedProperty m_IgnoreInvincibilityProp;
    SerializedProperty m_HittableLayersProp;
    //SerializedProperty m_OnDamageableHitProp;
    //SerializedProperty m_OnNonDamageableHitProp;
    SerializedProperty m_OnFinalKillProp;
    SerializedProperty m_DisableDamageAfterHit;

    void OnEnable()
    {
        m_DamageProp = serializedObject.FindProperty("damage");
        m_OffsetProp = serializedObject.FindProperty("offset");
        m_SizeProp = serializedObject.FindProperty("size");
        m_OffsetBasedOnSpriteFacingProp = serializedObject.FindProperty("offsetBasedOnSpriteFacing");
        m_SpriteRendererProp = serializedObject.FindProperty("spriteRenderer");
        m_CanHitTriggersProp = serializedObject.FindProperty("canHitTriggers");
        m_ForceRespawnProp = serializedObject.FindProperty("forceRespawn");
        m_IgnoreInvincibilityProp = serializedObject.FindProperty("ignoreInvincibility");
        m_HittableLayersProp = serializedObject.FindProperty("hittableLayers");
        //m_OnDamageableHitProp = serializedObject.FindProperty("OnDamageableHit");
        //m_OnNonDamageableHitProp = serializedObject.FindProperty("OnNonDamageableHit");
        m_OnFinalKillProp = serializedObject.FindProperty("OnFinalKill");
        m_DisableDamageAfterHit = serializedObject.FindProperty("disableDamageAfterHit");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_DamageProp);
        EditorGUILayout.PropertyField(m_OffsetProp);
        EditorGUILayout.PropertyField(m_SizeProp);
        EditorGUILayout.PropertyField(m_OffsetBasedOnSpriteFacingProp);
        EditorGUILayout.PropertyField(m_DisableDamageAfterHit);
        if (m_OffsetBasedOnSpriteFacingProp.boolValue)
            EditorGUILayout.PropertyField(m_SpriteRendererProp);
        EditorGUILayout.PropertyField(m_CanHitTriggersProp);
        EditorGUILayout.PropertyField(m_ForceRespawnProp);
        EditorGUILayout.PropertyField(m_IgnoreInvincibilityProp);
        EditorGUILayout.PropertyField(m_HittableLayersProp);
        //EditorGUILayout.PropertyField(m_OnDamageableHitProp);
        //EditorGUILayout.PropertyField(m_OnNonDamageableHitProp);
        EditorGUILayout.PropertyField(m_OnFinalKillProp);

        serializedObject.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        FinalKill damager = (FinalKill)target;

        if (!damager.enabled)
            return;

        Matrix4x4 handleMatrix = damager.transform.localToWorldMatrix;
        handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
        handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, damager.transform.position.z));
        using (new Handles.DrawingScope(handleMatrix))
        {
            s_BoxBoundsHandle.center = damager.offset;
            s_BoxBoundsHandle.size = damager.size;

            s_BoxBoundsHandle.SetColor(s_EnabledColor);
            EditorGUI.BeginChangeCheck();
            s_BoxBoundsHandle.DrawHandle();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(damager, "Modify Damager");

                damager.size = s_BoxBoundsHandle.size;
                damager.offset = s_BoxBoundsHandle.center;
            }
        }
    }

}
