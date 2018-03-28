using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupWindowManager : MonoBehaviour {

    // Relics
    public GameObject[] relics;

    // Current active objects
    public List<GameObject> activeRelics;

    // Random generator
    private System.Random ranGenerator;



    




    // Use this for initialization
    void Start()
    {
        activeRelics = new List<GameObject>();
        ranGenerator = new System.Random(System.DateTime.Now.Millisecond);
    }

    //// Update is called once per frame
    //void Update () {

    //}

    // Active a new prefab.
    public GameObject ActiveNewPrefab()
    {
        if (relics.Length < 1)
            return null;

        // Get the rest relic indices
        List<int> indexContainer = new List<int>();
        for(int i = 0; i < relics.Length; i++)
        {
            if(!activeRelics.Contains(relics[i]))
            {
                indexContainer.Add(i);
            }
        }

        // Get a relic index from the rest indices
        int index = (0 == indexContainer.Count) ? -1 : indexContainer[ranGenerator.Next(indexContainer.Count)];

        // Add active prefab
        if(index > -1)
        {
            activeRelics.Add(relics[index]);
        }

        return (index < 0) ? null : relics[index];
    }

    // Inactive prefab
    public void DisactivePrefab(GameObject prefab)
    {
        if (null == prefab)
            return;

        activeRelics.Remove(prefab);
    }

    public void ClearActivePrefabList()
    {
        activeRelics.Clear();
    }
}
