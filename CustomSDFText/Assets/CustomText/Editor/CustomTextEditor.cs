using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CustomText)), CanEditMultipleObjects]
public class CustomTextEditor : Editor
{
    protected SerializedProperty m_Text;
    protected SerializedProperty m_Size;
    protected SerializedProperty m_IsOrthographic;
    protected SerializedProperty m_UpdadeConfig;
    protected SerializedProperty m_Mesh;



    void OnEnable()
    {
        m_Text = serializedObject.FindProperty("Text");
        m_Size = serializedObject.FindProperty("Size");
        m_IsOrthographic = serializedObject.FindProperty("IsOrthographic");
        m_UpdadeConfig = serializedObject.FindProperty("UpdadeConfig");
        m_Mesh = serializedObject.FindProperty("m_Mesh");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_Mesh);
        EditorGUILayout.PropertyField(m_UpdadeConfig);

        EditorGUILayout.PropertyField(m_IsOrthographic);
        EditorGUILayout.PrefixLabel(String.Format("Text (w: {0:F2}, h: {1:F2})", ((CustomText)target).Width, ((CustomText)target).Height));
        m_Text.stringValue = EditorGUILayout.TextArea(m_Text.stringValue, GUILayout.MinHeight(85f), GUILayout.MaxHeight(200f));
        EditorGUILayout.PropertyField(m_Size, new GUIContent("Character Size"));

        serializedObject.ApplyModifiedProperties();
    }
}
