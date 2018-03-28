using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RouteAnimationController : MonoBehaviour {
    [Header("Camel Material")]
    public bool EnableCamel1 = true;
    public bool Camel1FlipX = false;
    [Space]
    public bool EnableCamel2 = true;
    public bool Camel2FlipX = false;

    [Header("Animation information")]
    [Tooltip("Normalized time (0,1)")]
    public float startTime = 0.0f; // [0.0f, 1.0f)
    [Tooltip("Normalized time (0,1)")]
    public float endTime = 1.0f; // (0.0f, 1.0f]

    // animator speed
    public float speed = 1.0f;
    public float duration = 1.0f;

	// Use this for initialization
	void Start () {

        // Saturate start and end time.
        endTime = Mathf.Clamp01(Mathf.Max(endTime + 0.1f, startTime + 0.1f));
        startTime = Mathf.Clamp01(startTime - 0.1f);

        GetComponent<RouteInstanceAutoDestroy>().endTime = endTime;

	}
	
	// Update is called once per frame
	void Update () {
        foreach (Image img in GetComponentsInChildren<Image>(true))
        {
            if (img.name.Equals("骆驼 1"))
            {
                img.gameObject.SetActive(EnableCamel1);
                img.material.SetFloat("_FlipX", Camel1FlipX ? 1.0f : 0.0f);
            }
            else if (img.name.Equals("骆驼 2"))
            {
                img.gameObject.SetActive(EnableCamel2);
                img.material.SetFloat("_FlipX", Camel2FlipX ? 1.0f : 0.0f);
            }
        }
    }
}
