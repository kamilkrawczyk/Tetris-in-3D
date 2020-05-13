using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static Spawner _instance;
    public static Spawner Instance
    {
        get { return _instance; }
    }



    public bool canSpawn;

    public GameObject[] cubes;
    private Vector3 spawnpoint;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnpoint = transform.position;
    }

   
    private GameObject GetRandomBlock()
    {
        int rnd = Random.Range(0, cubes.Length);
        return cubes[rnd];
    }
    public void SpawnNewBlock()
    {
        if(canSpawn)
        {
            GameObject blockToSpawn = GetRandomBlock();
            GameObject newBlock = Instantiate(blockToSpawn, spawnpoint, blockToSpawn.transform.rotation);
            
            foreach(Transform child in newBlock.transform) //start moving
            {
                if(child.GetComponent<BlockController>() != null)
                {
                    child.GetComponent<BlockController>().isActive = true;
                }
                else if(child.GetComponent<Clone>() != null)
                {
                    child.GetComponent<Clone>().isActive = true;
                }
            }
        }      
    }
}
