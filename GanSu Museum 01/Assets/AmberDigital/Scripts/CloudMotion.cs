using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class CloudMotion : MonoBehaviour {

    public float speedMultiplier = 1.0f;
    public float maxDelayTime = 5.0f;

    private bool triggered = false;
    //private Color mainColor;

    public Tweener transTweener;

	// Use this for initialization
	void Start () {
        transTweener = transform.DOLocalMoveX(-3500.0f, 120.0f)
            .SetDelay(0.0f /*5 * UnityEngine.Random.Range(0.0f, 1.0f)*/)
            .SetEase(Ease.Linear)
            .OnComplete(() => Destroy(this.gameObject));

        //mainColor = GetComponent<Image>().color;
	}

    //void OnDestroy()
    //{
    //    DestroyImmediate(this);
    //}
	
	// Update is called once per frame
	void Update () {

        if(!triggered)
        {
            if(transform.localPosition.x < -1700.0f)
            {
                GetComponent<Image>().DOFade(0.0f, transTweener.Duration(false) * (1.0f - transTweener.ElapsedPercentage(false)));
                triggered = true;
            }
        }

	}
}
