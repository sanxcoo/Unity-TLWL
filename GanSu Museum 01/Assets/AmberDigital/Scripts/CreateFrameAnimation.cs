using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class CreateFrameAnimation : MonoBehaviour {

    //public AnimationClip animationClip;
    [HideInInspector]
    public float animationTime = 1.0f;
    public float animFrameRate = 25;

    [HideInInspector]
    public string animFilePath = "Assets/";

    //public List<Texture2D> frames;
    public List<Sprite> frames;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
