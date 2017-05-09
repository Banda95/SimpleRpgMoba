using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class ArmyPanelHandler : NetworkBehaviour
{

    public GameObject slot;
    public GameObject creatureButton;
    public GameObject spawner;


    private int occupiedSlots = 0;
    private const int MAX_ITEM = 16;

    private List<LoadedStats> currentArmy = new List<LoadedStats>();

    public bool AddCreature(LoadedStats st)
    {
        if (currentArmy.Contains(st))
            return false;

        if (occupiedSlots == MAX_ITEM)
            return false;

        occupiedSlots++;

        GameObject sl, ite;
        sl = (GameObject)Instantiate(slot, transform.position, Quaternion.identity);
        sl.transform.SetParent(gameObject.transform);
        //TODO: understand why this is needed...(otherwise is set to 0 0 0).
        sl.transform.localScale = Vector3.one;

        ite = (GameObject)Instantiate(creatureButton, transform.position, Quaternion.identity);
        ite.transform.SetParent(sl.transform);
        ite.transform.localScale = Vector3.one;

       ite.GetComponent<Image>().sprite = st.sprite;

       ite.GetComponent<Button>().onClick.AddListener(() =>
       {
           spawner.GetComponent<CreatureSpawner>().CmdSpawnCreature(st);
           StartCoroutine(WaitCd(st.Cooldown, ite));
       });

        currentArmy.Add(st);
        return true;
    }

    public void SetSpawner(GameObject s)
    {
        spawner = s;
    }

    public List<LoadedStats> GetCurrentArmy()
    {
        return currentArmy;
    }

    IEnumerator WaitCd(float cd, GameObject button)
    {
        button.SetActive(false);
        yield return new WaitForSeconds(cd);
        button.SetActive(true);
    }
}

