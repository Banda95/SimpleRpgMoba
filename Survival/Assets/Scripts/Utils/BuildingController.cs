using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class BuildingController : NetworkBehaviour, IDamageable {

    [SyncVar]
    private BasicStats stats;
    private CreatureManager manager;
    
    [Server]
    void Awake () {

        manager = FindObjectOfType<CreatureManager>();

        stats = GetComponent<BasicStats>();
        stats.CurrentHealth = stats.BaseHealth;
    }
	
    [Server]
    public void DealDamage(int amount, bool isPlayer)
    {
        stats.CurrentHealth -= amount;
        if (stats.CurrentHealth <= 0)
        {
            if (isPlayer)
            {
                //TODO process crowd favor give away.
            }
            if (tag == "Fortress")
            {
                //TODO stats.team -> lose; ohter win.
                PlayerData.LastMatchWon = false;
                SceneManager.LoadScene("PostGame");
            }
            Destroy(this.gameObject);
            
        }
    }
}
