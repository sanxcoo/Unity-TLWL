using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CamelAnimationUtility : MonoBehaviour {

    public bool FlipX = false;
    public bool FlipY = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        // Update materials
        //GetComponent<Image>().material.SetFloat("_FlipX", FlipX ? 1.0f : 0.0f);
        //GetComponent<Image>().material.SetFloat("_FlipY", FlipY ? 1.0f : 0.0f);
	
	}
}
