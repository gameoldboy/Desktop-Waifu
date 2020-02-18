using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ReplaceMaterials))]
public class ReplaceMaterialsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("刷新Shader"))
        {
            ((ReplaceMaterials)target).Repalce();
        }
    }
}