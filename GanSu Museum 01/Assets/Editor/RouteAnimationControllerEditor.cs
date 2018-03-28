using UnityEngine.UI;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RouteAnimationController))]
public class RouteAnimationControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UpdateComponents();
    }

    void UpdateComponents()
    {
        RouteAnimationController tar = target as RouteAnimationController;

        foreach(Image img in tar.GetComponentsInChildren<Image>(true))
        {
            if(img.name.Equals("骆驼 1"))
            {
                img.gameObject.SetActive(tar.EnableCamel1);
                img.material.SetFloat("_FlipX", tar.Camel1FlipX ? 1.0f : 0.0f);
            }
            else if (img.name.Equals("骆驼 2"))
            {
                img.gameObject.SetActive(tar.EnableCamel2);
                img.material.SetFloat("_FlipX", tar.Camel2FlipX ? 1.0f : 0.0f);
            }
        }
    }
}
