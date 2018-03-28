using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class AutoPlayRelicFrameAnimation : MonoBehaviour {

    public GameObject relicFrameAnimation;


	// Use this for initialization
	void Start () {


    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void EnableRelicFrameAnimation()
    {
        if(relicFrameAnimation)
        {
            relicFrameAnimation.GetComponent<Animator>().enabled = true;
        }
    }


}
