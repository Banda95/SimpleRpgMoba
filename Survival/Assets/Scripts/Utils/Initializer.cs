using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Initializer : NetworkBehaviour
{
    public GameObject Environment;
    public GameObject BaseAFather;
    public List<GameObject> BaseAComponents;
    public GameObject BaseBFather;
    public List<GameObject> BaseBComponents;

    private GameObject PlayerA;
    private GameObject PlayerB;

    private CreatureManager manager;

    void Start()
    {
        manager = GetComponent<CreatureManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length == 0)
                return;

            PlayerA = players[0];
            PlayerA.GetComponent<PlayerController>().SetTeam(TeamName.TeamA);


            PlayerB = players[0]; //TMP.
            if (players.Length == 2)
            {
                PlayerB = players[1];
                PlayerB.GetComponent<PlayerController>().SetTeam(TeamName.TeamB);
            }

            GameObject obj;
            obj = Instantiate(BaseAFather);
            NetworkServer.SpawnWithClientAuthority(obj, PlayerA);

            obj = Instantiate(BaseBFather);
            NetworkServer.SpawnWithClientAuthority(obj, PlayerB);

            for (int i = 0; i < BaseAComponents.Count; i++)
            {
                obj = Instantiate(BaseAComponents[i]);
                NetworkServer.SpawnWithClientAuthority(obj, PlayerA);
                BasicStats stats = obj.GetComponent<BasicStats>();
                stats.UniqueId = obj.GetComponent<NetworkIdentity>().netId.Value;
                manager.registerCreature(transform, stats.UniqueId);
            }



            for (int i = 0; i < BaseBComponents.Count; i++)
            {
                obj = Instantiate(BaseBComponents[i]);
                NetworkServer.SpawnWithClientAuthority(obj, PlayerB);
                BasicStats stats = obj.GetComponent<BasicStats>();
                stats.UniqueId = obj.GetComponent<NetworkIdentity>().netId.Value;
                manager.registerCreature(transform, stats.UniqueId);
            }
            RpcSetSpawner();

            transform.GetComponent<Initializer>().enabled = false;
        }
    }

    [ClientRpc]
    private void RpcSetSpawner()
    {
        PlayerData.Race = "Darks";

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
            return;

        PlayerA = players[0];
        if (players.Length == 2)
            PlayerB = players[1];

        if (PlayerA.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            setUpUnits();
            if (PlayerA.GetComponent<BasicStats>().Team == TeamName.TeamA)
                GameObject.Find("Slots-Panel").GetComponent<ArmyPanelHandler>().SetSpawner(GameObject.Find("BarrackA(Clone)"));
            else
                GameObject.Find("Slots-Panel").GetComponent<ArmyPanelHandler>().SetSpawner(GameObject.Find("BarrackB(Clone)"));
        }
        else if (PlayerB != null && PlayerB.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            setUpUnits();
            if (PlayerB.GetComponent<BasicStats>().Team == TeamName.TeamA)
                GameObject.Find("Slots-Panel").GetComponent<ArmyPanelHandler>().SetSpawner(GameObject.Find("BarrackA(Clone)"));
            else
                GameObject.Find("Slots-Panel").GetComponent<ArmyPanelHandler>().SetSpawner(GameObject.Find("BarrackB(Clone)"));
        }
    }

    private void setUpUnits()
    {
        ArmyHandler army = FindObjectOfType<ArmyHandler>();
        switch (PlayerData.Race)
        {
            case "Darks":
                army.AddCreature("skeletonDark");
                army.AddCreature("skeletonFresh");
                army.AddCreature("skeletonMage");
                army.AddCreature("skeletonNormal");
                break;
            case "Humans":
                army.AddCreature("CaveWorm");
                army.AddCreature("Warrior");
                army.AddCreature("Necromancer");
                break;
        }
    }
}
