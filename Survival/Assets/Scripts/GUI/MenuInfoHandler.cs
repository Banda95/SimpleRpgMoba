using UnityEngine;
using UnityEngine.UI;

public class MenuInfoHandler : MonoBehaviour {

    private Text matchWon;
    private Text matchPlayed;
    private Text glory;
    private Text coins;
    private Text chests;

    void Awake()
    {
        matchWon = GetComponentsInChildren<Text>()[0];
        matchPlayed = GetComponentsInChildren<Text>()[1];
        glory = GetComponentsInChildren<Text>()[2];
        coins = GetComponentsInChildren<Text>()[3];
        chests = GetComponentsInChildren<Text>()[4];

        UpdateValues();
    }

    public void UpdateValues()
    {
        matchWon.text = "Match won: " + PlayerData.MatchWon;
        matchPlayed.text = "Match played: " + PlayerData.MatchPlayed;
        glory.text = "Player glory: " + PlayerData.OwnedGlory;
        coins.text = "Coins: " + PlayerData.OwnedCoin;
        chests.text = "Chests: " + PlayerData.OwnedChest;
    }

}
