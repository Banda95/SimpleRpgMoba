using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SelectWaypointRoute : RAINAction
{

    private BasicStats basicStats;
    private GameObject routeA, routeB;

    public override void Start(RAIN.Core.AI ai)
    {
        basicStats = (BasicStats)ai.Body.GetComponent(typeof(BasicStats));
        routeA = GameObject.Find("RouteA");
        if (!routeA)
            Debug.Log("not route A");
        routeB = GameObject.Find("RouteB");
        if (!routeB)
            Debug.Log("not route B");

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if(basicStats.Team == TeamName.TeamA)
            ai.WorkingMemory.SetItem<GameObject>("Route", routeA);
        else
            ai.WorkingMemory.SetItem<GameObject>("Route", routeB);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}