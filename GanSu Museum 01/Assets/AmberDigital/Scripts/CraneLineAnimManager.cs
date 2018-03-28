using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class CraneLineAnimManager : MonoBehaviour {
    [Header("Target")]
    public GameObject goCrane;

    [Header("Scale")]
    public float minScale = 0.4f;
    public float maxScale = 1.0f;

    [Header("Life time")]
    public float minLife = 2.0f;
    public float maxLife = 10.0f;
    private float lifeTime = 2.0f;

    [Header("Speed")]
    public float minSpeed = 0.1f;
    public float maxSpeed = 1.0f;

    [Header("Spawn delay")]
    public float minDelay = 1.0f;
    public float maxDelay = 10.0f;
    private float delay = 1.0f;

    private bool already = true;
    private bool active = false;

	// Use this for initialization
	void Start () {
        if(!goCrane)
        {
            already = false;
        }

        if (already)
        {
            Spawn();
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Check already.
        if (!already)
            return;

        
        if(active)
        {
            // Update life time.
            lifeTime -= Time.deltaTime;

            // Ready to die.
            if(lifeTime <= 1.0f)
            {
                Die();
            }
        }
        else
        {
            // Update delay.
            delay -= Time.deltaTime;
            if(delay <= 0.0f)
            {
                Spawn();
            }
        }
	}

    void Spawn()
    {
        // Set life time.
        lifeTime = Random.Range(minLife, maxLife);

        // Display crane
        goCrane.GetComponent<Image>().DOFade(1.0f, Mathf.Min(1.0f, lifeTime)).OnComplete(() => NextAction());

        // Set up line speed.
        gameObject.GetComponent<Animator>().speed = Random.Range(minSpeed, maxSpeed);

        // Set active.
        active = true;
    }

    void NextAction()
    {
        if (!active)
            return;

        Tweener twe = null;

        switch(Random.Range(0, 2))
        {
            case 0: // Do nothing.
                {
                    twe = goCrane.GetComponent<Image>().DOFade(goCrane.GetComponent<Image>().color.a, Random.Range(0.0f, 1.0f) * lifeTime);
                }
                break;
            case 1: // Scaling.
                {
                    float tarScale = Random.Range(minScale, maxScale);
                    twe = goCrane.transform.DOScale(new Vector3(tarScale, tarScale, 1.0f), Mathf.Max(5.0f, Random.Range(0.0f, 1.0f) * lifeTime));
                }
                break;
        }

        // Assign next action.
        if (null != twe)
        {
            twe.OnComplete(() => NextAction());
        }
    }

    void Die()
    {
        // Set inactive.
        active = false;

        // Make crane invisible.
        goCrane.GetComponent<Image>().DOFade(0.0f, Mathf.Max(lifeTime, 0.0f));

        // Set delay to spawn
        SetRandomDelay();
    }

    void SetRandomDelay()
    {
        delay = Random.Range(minDelay, maxDelay);
    }
}
