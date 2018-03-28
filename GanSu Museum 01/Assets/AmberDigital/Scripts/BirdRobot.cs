using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;



public class BirdRobot : MonoBehaviour {

    public float lifeTime = 0.0f;

    public float speed = 1.0f;

    public Rect bounds;

    public Vector3 lastDirection;

    private Tweener twe;

    // Use this for initialization
    void Start () {
        //transform.do

        Vector3[] way = new Vector3[Mathf.CeilToInt(lifeTime)];
        Vector3 pos = transform.position;
        for(int i = 0; i < way.Length; i++)
        {
            lastDirection = RandomDirection(lastDirection, Random.Range(20.0f,30.0f));
            pos += lastDirection * speed;
            way[i] = pos;
        }

        twe = transform.DOPath(way, lifeTime).OnComplete(() => Destroy(gameObject));
        //twe = transform.DOPath(way, lifeTime).OnComplete(() => Disappear());

        // Make sure be visible
        gameObject.GetComponent<Image>().DOFade(1.0f, 1.0f);

    }
	
	// Update is called once per frame
	void Update () {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        if(!bounds.Contains(pos))
        {
            twe.Complete();
        }

        if(twe.Elapsed() >= lifeTime - 1.0f)
        {
            gameObject.GetComponent<Image>().DOFade(0.0f, 1.0f);
        }
	}

    // Next animation action.
    void NextAction(float lifeLost)
    {
        //bool bTrans = Random.Range(0, 10) > 4 ? true : false;
        //bool bRot = Random.Range(0, 10) > 4 ? true : false;
        //bool bScale = Random.Range(0, 10) > 4 ? true : false;
        //bool bFade = Random.Range(0, 10) > 4 ? true : false;

        //// Translation
        //if(bTrans)
        //{
        //    Vector3 currentPos = transform.position;
        //    //transform
        //}

        //// Rotation

        //// Scaling

        //// Fading
            
    }

    private Vector3 RandomDirection(Vector3 lastDir, float range /* in degrees */)
    {
        return Quaternion.Euler(0, 0, Random.Range(-Mathf.Abs(range), Mathf.Abs(range))) * lastDir.normalized;
    }

    void Disappear()
    {
        gameObject.GetComponent<Image>().DOFade(0.0f, 1.0f).OnComplete(() => Destroy(gameObject));
    }

    void OnDisable()
    {
        if (gameObject.GetComponentInParent<BirdsManager>())
        {
            gameObject.GetComponentInParent<BirdsManager>().BirdDie();
        }
    }
}
