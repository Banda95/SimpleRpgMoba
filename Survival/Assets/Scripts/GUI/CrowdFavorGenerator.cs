using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CrowdFavorGenerator : NetworkBehaviour {

    public Text crowdFavorText;
    [SyncVar]
    public int CrowdFavor;

    public float incrementTime = 1;
    public int incrementRate = 1;

    private bool canIncrement = true;

	// Use this for initialization
	void Start () {
        crowdFavorText.text = "500";
        CrowdFavor = 500;
	}
	
	[Server]
	void Update () {
        if(canIncrement)
        {
            StartCoroutine(incrementFavor());
        }	    
	}

    [Command]
    public void CmdSpendFavor(int amount)
    {
        if (!(CrowdFavor < amount))
        {           
            CrowdFavor -= amount;
            crowdFavorText.text = CrowdFavor.ToString();
        }
    }

    IEnumerator incrementFavor()
    {
        canIncrement = false;
        CrowdFavor += incrementRate;
        crowdFavorText.text = CrowdFavor.ToString();
        yield return new WaitForSeconds(incrementTime);
        canIncrement = true;
    }
}
