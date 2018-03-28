using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class RouteInstanceAutoDestroy : MonoBehaviour {

    [HideInInspector]
    public float endTime = 1.0f;
    private Animator animator;

    [HideInInspector]
    public Sprite good;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        // Check if animator exists.
        if (null == animator)
            return;

        //  When animator is stopped, detroy this gameObject.
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= endTime)
        {
            transform.DOMove(transform.position, 1.0f).OnComplete(() => Die());

            foreach (Image img in GetComponentsInChildren<Image>(true))
            {
                img.DOFade(0.0f, 1.0f);
            }
        }
    }

    void Die()
    {
        // Remove active good
        if(GetComponentInParent<GoodsAnimationManager>())
        {
            GetComponentInParent<GoodsAnimationManager>().RemoveActiveGood(good);
        }

        // Destroy game object.
        Destroy(gameObject);
    }
}
