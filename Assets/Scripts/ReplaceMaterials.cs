using UnityEngine;

public class ReplaceMaterials : MonoBehaviour
{
    public Transform Character;

    public Shader shader;
    public void Repalce()
    {
        if (shader == null) return;
        foreach (Transform t in Character)
        {
            var materials = t.GetComponent<SkinnedMeshRenderer>().sharedMaterials;
            foreach (var m in materials)
            {
                m.shader = shader;
            }
        }
    }
}