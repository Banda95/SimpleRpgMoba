using UnityEngine;
using UnityEngine.Networking;


public class PlayerController : NetworkBehaviour {

    private BasicStats stats;
    private CreatureManager manager;

    void Awake()
    {

        stats = GetComponent<BasicStats>();
        stats.CurrentHealth = stats.BaseHealth;
        stats.Name = "Player";
        stats.Level = 1;
        stats.ExpToNextLevel = stats.Level * 20;

        manager = FindObjectOfType<CreatureManager>();
        stats.UniqueId = GetComponent<NetworkIdentity>().netId.Value;
        manager.registerCreature(transform, stats.UniqueId);
    }

    void Update()
    {
        
    }

    public void SetTeam(TeamName team)
    {
        stats.Team = team;
    }

    public void ProcessExp(int amount)
    {
        int exp = stats.ExpToNextLevel;
        exp -= amount;
        if (exp <= 0)
        {
            LevelUp();
            exp = stats.Level * 20 + (exp);
        }
        stats.ExpToNextLevel = exp;
    }

    public void LevelUp()
    {
        stats.Level++;
    }

}

