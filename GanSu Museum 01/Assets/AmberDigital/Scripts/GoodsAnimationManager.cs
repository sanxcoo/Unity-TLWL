using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;


/// <summary>
/// 丝绸之路商品路线演示动画管理器
/// </summary>
public class GoodsAnimationManager : MonoBehaviour {

    public enum Route_Orientation
    {
        WEST_TO_EAST = 0,
        EAST_TO_WEST = 1
    }

    public Route_Orientation RouteOrient;

    // 路线
    public GameObject[] prefab_Routes;

    // 货物
    public Sprite[] goods;
    public List<Sprite> activeGoods;

    // 交通工具动画
    //public Object prefab_TrafficTool;

    // 骆驼材质
    public Material mat;

    //
    // 参数检查标志
    //
    private bool already = true;

    // 下次路线生成的延迟时间
    public float minDelay = 0.0f;
    public float maxDelay = 20.0f;
    private float delay = 0.0f;
    private float delayTimer = 0.0f;

	// Use this for initialization
	void Start () {
        activeGoods = new List<Sprite>();

        // Set random seed.
        Random.seed = System.DateTime.Now.Millisecond;

        // Routes exists
        if (prefab_Routes.Length < 1)
            already = false;

        // Goods exists
        if (goods.Length < 1)
            already = false;
	}
	
	// Update is called once per frame
	void Update () {
        // Already
        if (!already)
            return;

        // Update delay timer.
        delayTimer += Time.deltaTime;

        // Ready to generate a new route.
        if(delayTimer >= delay)
        {
            // Generate a new route
            GenerateNewRoute();

            // Setup next delay time.
            delay = Random.Range(minDelay, maxDelay);

            // Reset delay timer.
            delayTimer = 0.0f;
        }
	
	}

    bool GenerateNewRoute()
    {
        Sprite newGood = GetInactiveGood();
        if (null == newGood)
            return false;

        // Create instance of a route prefab.
        GameObject route = Instantiate(prefab_Routes[Random.Range(0, prefab_Routes.Length)]);

        // Setup route animator speed by 10
        route.GetComponent<Animator>().speed = 0.1f;
        float reciprocalSpeed = 1.0f / route.GetComponent<Animator>().speed;

        // Setup animation information for RouteAnimationController.
        float ranTime1 = Random.Range(0.0f, 0.5f);
        float ranTime2 = Random.Range(0.5f, 1.0f);
        route.GetComponent<RouteAnimationController>().startTime = ranTime1;
        route.GetComponent<RouteAnimationController>().endTime = ranTime2;
        route.GetComponent<RouteAnimationController>().speed = route.GetComponent<Animator>().speed;
        route.GetComponent<RouteAnimationController>().duration = route.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;

        // Setup good
        route.GetComponent<RouteInstanceAutoDestroy>().good = newGood;

        // Play in fixed time.
        route.GetComponent<Animator>().PlayInFixedTime("State Default", 0, reciprocalSpeed * route.GetComponent<RouteAnimationController>().startTime * route.GetComponent<RouteAnimationController>().duration);

        //// Add a sprite in goods as 'Image' component to the new route.
        //route.AddComponent<Image>().sprite = goods[Random.Range(0, goods.Length)];

        //// Resize the image as its original size in pixels.
        //route.GetComponent<Image>().SetNativeSize();

        // Setup sprite of goods.
        foreach (Image img in route.GetComponentsInChildren<Image>())
        {
            if(img.name.Equals("Image"))
            {
                //img.sprite = goods[Random.Range(0, goods.Length)];
                img.sprite = newGood;

                // Add to active good.
                activeGoods.Add(newGood);
            }
            else if(img.name.Equals("骆驼 1"))
            {
                img.material = new Material(mat);
            }
            else if (img.name.Equals("骆驼 2"))
            {
                img.material = new Material(mat);
            }

            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.0f);
            img.DOFade(1.0f, 1.0f);
        }

        // Add route to 'this'.
        route.transform.SetParent(transform);
        route.transform.localPosition = Vector3.zero;
        route.transform.localScale = Vector3.one;

        return true;
    }

    Sprite GetInactiveGood()
    {
        Sprite good = null;

        List<int> restIndices = new List<int>();
        for(int i = 0; i < goods.Length; i++)
        {
            if(!activeGoods.Contains(goods[i]))
            {
                restIndices.Add(i);
            }
        }

        if(restIndices.Count > 0)
        {
            good = goods[restIndices[Random.Range(0, restIndices.Count)]];
        }

        return good;
    }

    public void RemoveActiveGood(Sprite good)
    {
        if(good)
            activeGoods.Remove(good);
    }
}
