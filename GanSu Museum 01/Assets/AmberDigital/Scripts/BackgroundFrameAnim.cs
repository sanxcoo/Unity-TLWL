using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BackgroundFrameAnim : MonoBehaviour {

    
    //public ArrayList 
    public float duration = 1.0f;
    public bool animEnabled = true;

    public Material animMaterial;

    private bool animValid = false;
    public int currIndex = 0;

    private float timer = 0.0f;

    private int nFramePerSec = 0;


    public List<Texture2D> frames;


    // Use this for initialization
    void Start () {
        if (frames.Count > 0)
        {
            animValid = true;
            frames.Sort((s1, s2) => s1.name.CompareTo(s2.name));
        }
        else
            animValid = false;

        nFramePerSec = Mathf.FloorToInt(frames.Count / duration);

        //ArrayList list = new ArrayList();
        //foreach (Texture2D t in frames)
        //{
        //    list.Add(t);
        //}

        //Debug.Log("List length:" + list.Count.ToString());
        //list.Sort();

        //for (int i = 0; i < frames.Length; i++)
        //{
        //    frames[i] = list[i] as Texture2D;
        //}


    }
	
	// Update is called once per frame
	void Update () {
        if(animEnabled && animValid)
        {
            if(animMaterial)
            {
                currIndex = Mathf.FloorToInt(nFramePerSec * timer) % frames.Count;
                animMaterial.mainTexture = frames[currIndex];

                timer += Time.fixedDeltaTime;
                if(timer > duration)
                {
                    //currIndex = (currIndex + 1) % frames.Length;
                    timer -= duration;
                }
            }
        }
	}
}
