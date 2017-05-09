using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class ResourceSpawner : NetworkBehaviour {
    public List<GameObject> ResourceList;
    [HideInInspector]
    public int resourceCount = 0;
    public  int RESOURCE_MAX = 100;
    public Vector3 rangeVector = Vector3.zero;

    [Server]
    void Update()
    {
        if (resourceCount < RESOURCE_MAX)
        {
            SpawnSomeResource(RESOURCE_MAX - resourceCount);
            resourceCount = RESOURCE_MAX;
        }
    }


    private void SpawnSomeResource(int amount)
    {
        for (int i = 1; i <= amount; i++)
        {
            int index = Random.Range(0, ResourceList.Count);
            Vector3 position = new Vector3(Random.Range(-rangeVector.x, rangeVector.x), Random.Range(-rangeVector.y, rangeVector.y), Random.Range(-rangeVector.z, rangeVector.z));
            GameObject obj = (GameObject)Instantiate(ResourceList[index], transform.position + position, Quaternion.identity);
            obj.transform.SetParent(transform);
            NetworkServer.Spawn(obj);
        }
    }
}
