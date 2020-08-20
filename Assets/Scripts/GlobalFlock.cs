using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GlobalFlock : MonoBehaviour
{
    public GameObject[] butterflyPrefab;
    public float areaSize = 3;
    public float butrerSize = 0.5f;
    public int numButterflies = 15;
    public Flock[] allButterfly;
    //public GameObject goalPosGameobject;
    public Vector3 goalPos = Vector3.zero;
    public float speedOffset = 2.0f;

    private List<int> deadButterList = new List<int>();
    private int deadMin = 1;
    private int spawnNum = 5;
    // Use this for initialization
    void Start()
    {
        deadButterList.Clear();
        allButterfly = new Flock[numButterflies];
        //goalPos = gameObject.transform.position;

        for (int i = 0; i < numButterflies; i++)
        {
            Vector3 pos = gameObject.transform.position + new Vector3(Random.Range(-areaSize, areaSize),
                Random.Range(-areaSize, areaSize),
                -areaSize);

            int rdm = Random.Range(0, butterflyPrefab.Length);
            var obj = (GameObject)Instantiate(butterflyPrefab[rdm], pos, Quaternion.identity);
            allButterfly[i] = obj.GetComponent<Flock>();
            allButterfly[i].transform.parent = gameObject.transform;
            allButterfly[i].transform.localPosition = Vector3.zero;
            allButterfly[i].transform.localScale = Vector3.one * butrerSize;
            allButterfly[i].Init(this, i, butrerSize);
            allButterfly[i].delegateDestoryFlock = DeadFlock;
        }
        InvokeRepeating("SpawnFlock", 0.0f, 1.0f);
    }

    private void DeadFlock(int index)
    {
        if (deadButterList.Contains(index))
            return;
        deadButterList.Add(index);
    }

    private void SpawnFlock()
    {
        if (deadButterList.Count == 0)
        {
            return;
        }
        if (deadButterList.Count >= allButterfly.Length - deadMin)
        {
            int spawnIndex = 0;
            for (int i = 0; i < deadButterList.Count; i++)
            {
                allButterfly[deadButterList[i]].Play(true);
                deadButterList.RemoveAt(i);
                i--;
                spawnIndex++;
                if (spawnIndex >= spawnNum)
                {
                    break;
                }
            }
        }
        else
        {
            allButterfly[deadButterList[0]].Play(true);
            deadButterList.RemoveAt(0);
        }
    }
}