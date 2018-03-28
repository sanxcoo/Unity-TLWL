using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GenerateGussian : MonoBehaviour
{

    public float radius = 1;

    public Image img;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(img)
        {
            img.material.SetFloat("_BlurRadius", radius);
        }
    }
}
