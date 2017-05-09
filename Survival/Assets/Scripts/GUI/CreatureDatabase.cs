using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.IO;
public class CreatureDatabase : MonoBehaviour{
    private List<LoadedStats> db = new List<LoadedStats>();
    private JsonData creatureData;

    void Awake()
    {
        creatureData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Creatures.json"));
        ConstructCreatureDB();
    }

    public LoadedStats FindCreatureById(int id)
    {
        for (int i = 0; i < db.Count; i++)
        {
            if (db[i].ID == id)
                return db[i];
        }
        return null;
    }
    public LoadedStats FindCreatureByName(string name)
    {
        for (int i = 0; i < db.Count; i++)
        {
            if (db[i].Name == name)
                return db[i];
        }
        return null;
    }

    public void ConstructCreatureDB()
    {
        for (int i = 0; i < creatureData.Count; i++)
        {
            db.Add(new LoadedStats((int)creatureData[i]["id"], creatureData[i]["name"].ToString(),(int)creatureData[i]["health"], 
                (int)creatureData[i]["spd"], (int)creatureData[i]["atk"], (double)creatureData[i]["atkspd"], 
                (int)creatureData[i]["atkrange"],(int)creatureData[i]["cooldown"], (int)creatureData[i]["cost"]));
        }
    }
}

public class LoadedStats
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int BaseHealth { get; set; }
    public int Speed { get; set; }
    public int AttackDamage { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackRange { get; set; }
    public float Cooldown { get; set; }
    public int Cost { get; set; }
    public Sprite sprite { get; set; }

    public LoadedStats(int id, string name, int health, int spd, int atk, double atkspd, int atkrange, int cd,int cost)
    {
        ID = id;
        Name = name;
        BaseHealth = health;
        Speed = spd;
        AttackDamage = atk;
        AttackSpeed = (float)atkspd;
        AttackRange = atkrange;
        Cooldown = cd;
        Cost = cost;
        sprite = Resources.Load<Sprite>("UI/Textures/" + name + "-icon");
    }
}