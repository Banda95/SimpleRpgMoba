using UnityEngine;

public static class PlayerData
{
    public static int OwnedCoin = 0;
    public static int OwnedChest = 0;
    public static int PlayerLevel = 0;
    public static int OwnedGlory = 0;
    public static int MapId = 0;
    public static int MatchWon = 0;
    public static int MatchPlayed = 0;

    public static string Race;

    public static bool LastMatchWon;

    static PlayerData()
    {
        OwnedCoin = PlayerPrefs.GetInt("coin", 0);
        OwnedChest = PlayerPrefs.GetInt("chest", 0);
        PlayerLevel = PlayerPrefs.GetInt("level", 0);
        OwnedGlory = PlayerPrefs.GetInt("glory", 0);
        MapId = PlayerPrefs.GetInt("mapId", 0);
        MatchWon = PlayerPrefs.GetInt("matchWon", 0);
        MatchPlayed = PlayerPrefs.GetInt("matchPlayed", 0);

    }

    public static void Save()
    {
        PlayerPrefs.SetInt("coin", OwnedCoin);
        PlayerPrefs.SetInt("chest", OwnedChest);
        PlayerPrefs.SetInt("level", PlayerLevel);
        PlayerPrefs.SetInt("glory", OwnedGlory);
        PlayerPrefs.SetInt("mapId", MapId);
        PlayerPrefs.SetInt("matchWon", MatchWon);
        PlayerPrefs.SetInt("matchPlayed", MatchPlayed);

        PlayerPrefs.Save();
    }
}