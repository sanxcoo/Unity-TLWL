using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class RandomCloudAnimation : MonoBehaviour {

    public GameObject[] clouds;

    public float minInterval = 1.0f;
    public float maxInterval = 10.0f;

    private float timeToBorn = 5.0f;
    private float timer = 0.0f;

    private System.Random ranIndex;
    private int nextIndex = 0;

    // Use this for initialization
    void Start () {
        //GameObject go = Instantiate(clouds[0]);
        //go.transform.SetParent(this.transform);

        // Set up random seed
        Random.seed = System.DateTime.Now.Millisecond;

        // cloud index
        ranIndex = new System.Random(System.DateTime.Now.Millisecond);

        // Set the first time to born a new cloud
        timeToBorn = Random.Range(0.0f, maxInterval);
    }
	
	// Update is called once per frame
	void Update () {
        // Check clouds
        if (clouds.Length < 1)
            return;

        // Update timer
        timer += Time.deltaTime;

        // Create a new cloud, if any.
        if(timer > timeToBorn)
        {
            // Instantiate cloud randomly.
            GameObject go = Instantiate(clouds[nextIndex]);
            go.transform.SetParent(this.transform);
            go.GetComponent<CloudMotion>().speedMultiplier = Random.Range(0.9f, 1.1f);
            go.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, Random.Range(0.6f, 1.0f));

            // Reset timer & timeToBorn
            timeToBorn = Random.Range(minInterval, maxInterval);
            timer = 0.0f;

            // Get next cloud index
            if(clouds.Length > 1)
            {
                int newIndex = nextIndex;
                int count = 1;

                do
                {
                    newIndex = ranIndex.Next(0, clouds.Length);

                    if(newIndex != nextIndex)
                    {
                        nextIndex = newIndex;
                        break;
                    }

                    count++;
                    if (count > 10)
                        break;

                } while (newIndex == nextIndex);
            }
        }
	}
}
