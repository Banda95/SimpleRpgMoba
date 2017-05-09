using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreatureSpawner : NetworkBehaviour
{
    public List<GameObject> creatureList;

    public TeamName team;

    private CreatureManager manager;
    private Text UnitsText;
    private Transform spawnParent;
    private CreatureDatabase db;
    private CrowdFavorGenerator favorGen;
    private bool canSpawn = true;
    private int unitsCount = 0;
    private const int MAX_UNITS = 100;

    // Use this for initialization
    void Awake()
    {
        if (localPlayerAuthority)
        {
            GameObject PlayerA;
            GameObject PlayerB = null;

            db = FindObjectOfType<CreatureDatabase>();
            favorGen = FindObjectOfType<CrowdFavorGenerator>();
            manager = FindObjectOfType<CreatureManager>();

            spawnParent = GameObject.Find("Minions").transform;
            UnitsText = GameObject.Find("UnitsCount").GetComponent<Text>();

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            PlayerA = players[0];
            if (players.Length == 2)
                PlayerB = players[1];

            if (PlayerA.GetComponent<NetworkIdentity>().hasAuthority)
            {
                team = PlayerA.GetComponent<BasicStats>().Team;
            }
            else if (PlayerB != null && PlayerB.GetComponent<NetworkIdentity>().hasAuthority)
            {
                team = PlayerB.GetComponent<BasicStats>().Team;
            }
        }
    }

    [Command]
    public void CmdSpawnCreature(LoadedStats stats)
    {
        if (unitsCount >= MAX_UNITS)
        {
            //TODO display: no more place for other units.
            return;
        }

        if (favorGen.CrowdFavor < stats.Cost)
        {

            //TODO display: not enough crowd favor.
            return;
        }
        favorGen.CmdSpendFavor(stats.Cost);

        GameObject obj;
        canSpawn = false;
        Vector3 position = new Vector3(10, 1, 5);
        obj = (GameObject)Instantiate(creatureList[stats.ID], transform.position + position, Quaternion.identity);
        obj.transform.SetParent(spawnParent);
        BasicStats bs = obj.GetComponent<BasicStats>();
        bs.Team = team;
        bs.BaseHealth = stats.BaseHealth;
        bs.AttackDamage = stats.AttackDamage;
        bs.AttackSpeed = stats.AttackSpeed;
        bs.AttackRange = stats.AttackRange;
        bs.Speed = stats.Speed;
        bs.Name = stats.Name; 
        unitsCount++;
        UnitsText.text = unitsCount + "/" + MAX_UNITS;

        NetworkServer.SpawnWithClientAuthority(obj, transform.GetComponent<NetworkIdentity>().clientAuthorityOwner);
        bs.UniqueId = GetComponent<NetworkIdentity>().netId.Value;
        manager.registerCreature(transform, bs.UniqueId);
    }
}
