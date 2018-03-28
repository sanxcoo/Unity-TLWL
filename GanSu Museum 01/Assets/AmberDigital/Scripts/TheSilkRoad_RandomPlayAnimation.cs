using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TheSilkRoad_RandomPlayAnimation : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Random.seed = System.DateTime.Now.Millisecond;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetRandomDelay()
    {
        // Idle time base : 5s
        GetComponent<Animator>().SetFloat("IdleTimeSpeedMultiplier", Random.Range(0.2f, 0.5f)); // 10 - 25s
    }
}
