using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;


[CustomEditor(typeof(GenerateGussian))]
//[CanEditMultipleObjects]
public class GenerateGussianEditor : Editor
{

    //   public int radius = 5;
    //   //public float sigma = 1 / 3.0f;

    //   private bool bDisplay_Gussian = true;
    //   private float[] gussian;

    //public override void OnInspectorGUI()
    //   {
    //       //base.OnInspectorGUI();

    //       // radius
    //       //radius = EditorGUILayout.IntField("Radius", radius);
    //       radius = EditorGUILayout.IntSlider("Radius", radius, 1, 10);

    //       // sigma
    //       //sigma = EditorGUILayout.Slider("Sigma", sigma, 0, 3.0f * radius);

    //       // Generate gussian values.
    //       Gussian();

    //       EditorGUILayout.Space();

    //       // Display gussian values
    //       bDisplay_Gussian = EditorGUILayout.Foldout(bDisplay_Gussian, "Gussian");
    //       if (bDisplay_Gussian)
    //       {
    //           string msg = "";
    //           for(int n = 0, i = -radius; i <= radius; i++)
    //           {
    //               for(int j = -radius; j <= radius; j++, n++)
    //               {
    //                   msg += gussian[n].ToString("f9") + ",";
    //               }
    //               msg += "\n";
    //           }
    //           //EditorGUILayout.HelpBox(msg, MessageType.None);
    //           EditorGUILayout.TextArea(msg);
    //       }
    //   }



    //   // 而为高斯函数
    //   private void Gussian()
    //   {
    //       int diamet = radius * 2 + 1;
    //       gussian = new float[diamet * diamet];

    //       float sigma = radius / 3.0f;
    //       float sigma2 = 2.0f * sigma * sigma;
    //       float sigmap = sigma2 * Mathf.PI;

    //       for(int n = 0, i = -radius; i <= radius; i++)
    //       {
    //           int i2 = i * i;
    //           for(int j = -radius; j <= radius; j++, n++)
    //           {
    //               gussian[n] = Mathf.Exp(-(i2 + j * j) / sigma2) / sigmap;
    //           }
    //       }

    //       // Normalization
    //       float sum = 0.0f;
    //       for(int i = 0; i < gussian.Length; i++)
    //       {
    //           sum += gussian[i];
    //       }
    //       if (sum.Equals(1.0f))
    //           return;
    //       for(int i = 0; i < gussian.Length; i++)
    //       {
    //           gussian[i] /= sum;
    //       }
    //   }


    //SerializedProperty radius;
    //SerializedProperty img;

    //void OnEnable()
    //{
    //    radius = serializedObject.FindProperty("radius");
    //    img = serializedObject.FindProperty("img");
    //}

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        GenerateGussian tar = target as GenerateGussian;
        tar.radius = EditorGUILayout.Slider("Blur radius", tar.radius, 0, 15);
        tar.img = (Image)EditorGUILayout.ObjectField("Image", tar.img, typeof(Image), true);
        if (tar.img.material)
        {
            tar.img.material.SetFloat("_BlurRadius", tar.radius);
        }



        //// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        //serializedObject.Update();


        //EditorGUILayout.Slider(radius, 0, 15, "Blur radius");


        //EditorGUILayout.PropertyField(img, new GUIContent("Image"));

        //// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //serializedObject.ApplyModifiedProperties();


    }

}
