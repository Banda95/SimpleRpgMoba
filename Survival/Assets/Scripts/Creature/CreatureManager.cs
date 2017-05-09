using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;


public class CreatureManager: NetworkBehaviour{

    private Dictionary<uint,Transform> creatureIdList = new Dictionary<uint, Transform>();
    private int unitsCount = 0;

    public void registerCreature(Transform t, uint netId)
    {
        creatureIdList.Add(netId, t);
    }

    public Transform getCreatureById(uint id)
    {
        if(creatureIdList[id])
            return creatureIdList[id];

        return null;
    }

    public void removeCreatureById(uint id)
    {
        if (creatureIdList[id])
            creatureIdList.Remove(id);
    }
}
