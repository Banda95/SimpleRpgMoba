using UnityEngine;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour
{
    public KeyCode armyKeyCode = KeyCode.U;

    private ArmyHandler army;

    private bool armyCanChange = true;
    private bool armyShowing = false;

    private Vector3 selectionStart;
    private int unitsMask;
    private int groundMask;
    private int buildingMask;

    private List<GameObject> selectedUnits;
    private Transform selectionBox;
    private Vector2 startMouse;
    private TeamName Team = TeamName.None;

    void Start()
    {

        army = FindObjectOfType<ArmyHandler>();

        unitsMask = LayerMask.GetMask("Unit");
        groundMask = LayerMask.GetMask("Terrain");
        buildingMask = LayerMask.GetMask("Buildings");

        selectedUnits = new List<GameObject>();

        selectionBox = transform.Find("Selection_Box");

        if (GameObject.Find("Player"))
        {
            Team = GameObject.Find("Player").GetComponent<BasicStats>().Team;
        }
    }

    void Update()
    {
        if(Team == TeamName.None)
        {
            if(GameObject.Find("Player"))
            {
               Team = GameObject.Find("Player").GetComponent<BasicStats>().Team;
            }
        }
        #region UIPanel
        if (Input.GetKeyDown(armyKeyCode) && armyCanChange)
        {
            armyShowing = !armyShowing;
            armyCanChange = false;
            army.Toggle(armyShowing);
        }
        if (Input.GetKeyUp(armyKeyCode) && !armyCanChange)
        {
            armyCanChange = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, buildingMask))
            {
                if (hit.transform.GetComponent<BasicStats>().Team == Team)
                {   //Click on my buildings.          
                    switch (hit.transform.tag)
                    {
                        case "Barrack":
                            toggleArmy();
                            break;
                        case "Smithy":
                            toggleSmithy();
                            break;
                        case "Fortress":
                            toggleFortress();
                            break;
                    }
                }
            }
        }

        #endregion

        #region UnitsSelection

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, groundMask))
            {
                selectionStart = hit.point;
            }

            if (Physics.Raycast(ray, out hit, 100, unitsMask))
            {
                clearSelection();
                selectedUnits.Add(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<CreatureController>().Select();
            }
            else
            {
                clearSelection();
            }
            startMouse = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(startMouse, Input.mousePosition) > 10)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, groundMask))
                {
                    selectionBox.localScale = new Vector3(100, 1, hit.point.z - selectionStart.z);
                    selectionBox.position = new Vector3(selectionStart.x, selectionStart.y, selectionStart.z + (selectionBox.lossyScale.z / 2));
                    var tmp = selectionBox.GetComponent<Rigidbody>().SweepTestAll(new Vector3(hit.point.x - selectionStart.x, 0, 0), Mathf.Abs(hit.point.x - selectionStart.x));

                    for (int i = 0; i < tmp.Length; i++)
                    {
                        if (tmp[i].collider.GetComponent<CreatureController>())
                        {
                            // if (tmp[i].collider.GetComponent<BasicStats>().Team == player.team?)
                            selectedUnits.Add(tmp[i].transform.gameObject);
                            tmp[i].collider.GetComponent<CreatureController>().Select();
                        }
                    }
                    selectionBox.localScale = Vector3.zero;
                    selectionBox.position = Vector3.zero;
                }
            }
        }
        #endregion

        #region UnitsMovement
        if (Input.GetMouseButton(1) && selectedUnits.Count > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, groundMask))
            {
                //Move
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (selectedUnits[i] && selectedUnits[i].GetComponent<BasicStats>().Team == Team)
                        selectedUnits[i].GetComponent<CreatureController>().CmdSetDestination(hit.point);
                }
            }

            if (Physics.Raycast(ray, out hit, 100, unitsMask))
            {
                //Attack if enemy
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (selectedUnits[i] && selectedUnits[i].GetComponent<BasicStats>().Team != hit.transform.gameObject.GetComponent<BasicStats>().Team
                           && (selectedUnits[i].GetComponent<BasicStats>().Team == Team))
                        selectedUnits[i].GetComponent<CreatureController>().CmdSetEnemy(hit.transform.GetComponent<BasicStats>().UniqueId);
                }
            }

            if (Physics.Raycast(ray, out hit, 100, buildingMask))
            {
                //Attack if enemy
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (selectedUnits[i] && selectedUnits[i].GetComponent<BasicStats>().Team != hit.transform.gameObject.GetComponent<BasicStats>().Team
                           && (selectedUnits[i].GetComponent<BasicStats>().Team == Team))
                        selectedUnits[i].GetComponent<CreatureController>().CmdSetEnemy(hit.transform.GetComponent<BasicStats>().UniqueId);
                }

            }
        }
        #endregion
    }

    public bool checkSelection()
    {
        return selectedUnits.Count == 0;
    }

    private void clearSelection()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if(selectedUnits[i])
                selectedUnits[i].GetComponent<CreatureController>().Deselect();
        }
        selectedUnits.Clear();
    }

    void OnGUI()
    {
        if (Input.GetMouseButton(0) && Vector2.Distance(startMouse, Input.mousePosition) > 10)
        {
            GUI.Box(new Rect(startMouse.x, Screen.height - startMouse.y, Input.mousePosition.x - startMouse.x, -(Input.mousePosition.y - startMouse.y)), "");
        }
    }

    private void toggleArmy()
    {
        armyShowing = true;
        army.Toggle(armyShowing);
    }

    private void toggleFortress()
    {

    }

    private void toggleSmithy()
    {

    }  
}
