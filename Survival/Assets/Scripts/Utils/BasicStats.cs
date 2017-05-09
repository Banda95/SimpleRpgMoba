using UnityEngine;


public enum TeamName
{
    TeamA,
    TeamB,
    None
}


public class BasicStats : MonoBehaviour {
    
    public int BaseHealth = 1;

    [HideInInspector]
    public int CurrentHealth = 1;


    public int Speed = 1;
    public int AttackDamage = 1;
    public float AttackSpeed = 0.5f;
    public float AttackRange = 1;
    public string Name = "Unkown";
    public TeamName Team = TeamName.TeamA;

    
    [HideInInspector]
    public int Level = 1;
    [HideInInspector]
    public int ExpToNextLevel = 20;
    [HideInInspector]
    public uint UniqueId = 0;


}
