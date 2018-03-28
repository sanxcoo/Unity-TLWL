using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class BirdsManager : MonoBehaviour {

    public GameObject prefab_bird;

    //public List<GameObject> birds;
    [Range(0, 100)]
    public int maxBirds = 10;
    private int birdsCount = 0;

    public float minLife = 1.0f;
    public float maxLife = 10.0f;

    public float minSpeed = 1.0f;
    public float maxSpeed = 10.0f;

    public Image imgRectBounds;
    private Rect bounds;

    public float minDelay = 0.0f;
    public float maxDelay = 10.0f;
    private float delay = 0.0f;
    private float delayTimer = 0.0f;

    private bool already = true;

	// Use this for initialization
	void Start () {
        //
        // Check params
        //
        if (null == prefab_bird)
            already = false;

        if (minDelay > maxDelay)
            already = false;

        if (null == imgRectBounds)
            already = false;
        else
        {
            Vector3 pos = imgRectBounds.transform.position;
            Vector3 scale = imgRectBounds.transform.localScale;
            Vector2 size = imgRectBounds.rectTransform.sizeDelta;
            size = new Vector2(size.x * scale.x, size.y * scale.y);

            bounds = new Rect(pos.x - size.x / 2.0f, pos.y - size.y / 2.0f, size.x, size.y);
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Check already
        if (!already)
            return;

        // Update delay timer
        delayTimer += Time.deltaTime;

        // Add bird if any.
        if (delayTimer >= delay)
        {
            AddBird();

            // Ready for the next bird born.
            delay = Random.Range(minDelay, maxDelay);
            delayTimer = 0.0f;
        }
	}

    private void AddBird()
    {
        if (birdsCount >= maxBirds)
            return;

        GameObject instBird = Instantiate(prefab_bird);

        // Add the bird to this gameObject.
        instBird.transform.SetParent(transform);

        // Set scale.
        float scale = Random.Range(0.4f, 1.0f);
        instBird.transform.localScale = new Vector3(scale, scale, 1.0f);

        // Set position.
        instBird.transform.position = new Vector3(Random.Range(bounds.xMin, bounds.xMax), 
            Random.Range(bounds.yMin, bounds.yMax), 
            transform.position.z);

        // Set BirdRobot.
        instBird.GetComponent<BirdRobot>().lifeTime = Random.Range(minLife, maxLife);
        instBird.GetComponent<BirdRobot>().speed = Random.Range(minSpeed, maxSpeed);
        instBird.GetComponent<BirdRobot>().bounds = bounds;
        instBird.GetComponent<BirdRobot>().lastDirection = Vector3.right;


        // Add bird to birds list.
        birdsCount++;
    }

    public void BirdDie()
    {
        birdsCount--;
    }
}
