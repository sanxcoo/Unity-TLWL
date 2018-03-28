using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class PopupRelicWindow : MonoBehaviour {

    public GameObject prefab_Relic = null;

    private GameObject prefabInstance = null;

    public GameObject sensor;
    private bool needToClearActiveRelicList = false;

    // Additional particle effects
    public GameObject particleFullscreen;
    public GameObject prefab_particlePopupUpward;
    private GameObject instance_particlePopupUpward;

    private static int alivePrefabInstCount = 0;    // 当前还在显示的文物实例个数

    // Use this for initialization
    void Awake () {
        if(prefab_Relic)
        {
            prefab_Relic = null;
        }

        if(prefabInstance)
        {
            DestroyImmediate(prefabInstance);
        }
	}

    public bool IsWorking()
    {
        bool isWorking = false;

        if (null == prefab_Relic && null == prefabInstance)
            isWorking = true;

        return isWorking;
    }

    void OnEnable()
    {
        if (null != prefab_Relic)
            return;

        // Check sensor
        if (null == sensor)
            return;

        prefab_Relic = GetComponentInParent<PopupWindowManager>().ActiveNewPrefab();

        if(null == prefab_Relic)
        {
            needToClearActiveRelicList = true;

            // It's no need to be actived.
            gameObject.SetActive(false); 
        }

        if (prefab_Relic)
        {
            prefabInstance = Instantiate(prefab_Relic);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.transform.localPosition = Vector3.zero;
            prefabInstance.transform.localScale = Vector3.one;

            alivePrefabInstCount++;

            Debug.Log("Instantiate prefab:" + prefab_Relic.name + ", " + prefabInstance.name);

            // Additional particles
            if(particleFullscreen)
            {
                particleFullscreen.GetComponent<ParticleSystem>().Play();
            }
            if(prefab_particlePopupUpward)
            {
                instance_particlePopupUpward = Instantiate(prefab_particlePopupUpward);
                instance_particlePopupUpward.transform.SetParent(transform);
                instance_particlePopupUpward.transform.localPosition = Vector3.zero;
                instance_particlePopupUpward.transform.localScale = Vector3.one;

                instance_particlePopupUpward.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DisableAndDestroyPrefab()
    {
        if (prefabInstance)
        {
            Destroy(prefabInstance);
            prefabInstance = null;

            alivePrefabInstCount--;
        }



        if (needToClearActiveRelicList && 0 == alivePrefabInstCount)
        {
            // No relic prefab needs to be actived.
            // Then clear the active relic prefab list, and notify the 'SensorListener' leave dense active mode.
            GetComponentInParent<PopupWindowManager>().ClearActivePrefabList();

            sensor.GetComponent<SensorListener>().LeaveDenseActiveMode();

            needToClearActiveRelicList = false;
        }

        gameObject.SetActive(false);

    }

    public void DelayToDisable()
    {
        if (prefab_Relic)
        {
            // <Notice>
            // Do not disactive prefab while the 'SensorListener' is in dense active mode. 
            // Try to clear the active list of PopupWindowManager when all relic prefabs had been actived already.
            // Otherwise disactive the prefab usually.
            // <!Notice>
            if (sensor)
            {
                if (!sensor.GetComponent<SensorListener>().IsInDenseActiveMode())
                {
                    GetComponentInParent<PopupWindowManager>().DisactivePrefab(prefab_Relic);                }
            }
        }

        prefab_Relic = null;

        FadeDestroy(prefabInstance);

        Destroy(instance_particlePopupUpward);
    }

    private void FadeDestroy(GameObject prefabInst)
    {
        if (prefabInst)
        {
            prefabInst.GetComponent<Animator>().enabled = false;
            prefabInst.transform.DOMove(prefabInst.transform.position, 1.0f).OnComplete(() => DisableAndDestroyPrefab());
            foreach (Image img in prefabInst.GetComponentsInChildren<Image>())
            {
                img.DOFade(0.0f, 1.0f);
            }

            foreach (Text txt in prefabInst.GetComponentsInChildren<Text>())
            {
                txt.DOFade(0.0f, 1.0f);
            }
        }
        else
        {
            DisableAndDestroyPrefab();
        }
    }
}
