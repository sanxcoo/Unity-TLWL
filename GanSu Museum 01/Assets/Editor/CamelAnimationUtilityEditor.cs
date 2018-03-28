using UnityEngine.UI;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CamelAnimationUtility))]
public class CamelAnimationUtilityEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        CamelAnimationUtility tar = target as CamelAnimationUtility;

        tar.GetComponent<Image>().material.SetFloat("_FlipX", tar.FlipX ? 1.0f : 0.0f);
        tar.GetComponent<Image>().material.SetFloat("_FlipY", tar.FlipY ? 1.0f : 0.0f);
    }
}
