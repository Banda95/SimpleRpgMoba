using UnityEngine;
using System.Collections;

public class ArmyHandler : MonoBehaviour {

    private CreatureDatabase db;
    private ArmyPanelHandler slots;
    private Vector3 localPos;

    void Awake()
    {
        db = GetComponent<CreatureDatabase>();
        slots = FindObjectOfType<ArmyPanelHandler>();
        localPos = transform.localPosition;
    }

    void Start()
    {
        Toggle(false);
    }


    public bool AddCreature(string name)
    {
        return slots.AddCreature(db.FindCreatureByName(name));
    }

    public void Toggle(bool b)
    {
        //This is not a good solution, but it works...
        if (b)
            transform.localPosition = localPos;
        else
            transform.localPosition = new Vector3(1000, 1000);

    }
}
