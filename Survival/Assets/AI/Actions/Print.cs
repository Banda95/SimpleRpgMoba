using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Motion;
using RAIN.Representation;

[RAINAction]
public class Print : RAINAction
{
    public Expression PrintingInfo = new Expression();

    string tInfo = "";
    public override void Start(RAIN.Core.AI ai)
    {
        if (PrintingInfo.IsValid)
            tInfo = PrintingInfo.Evaluate<string>(ai.DeltaTime, ai.WorkingMemory);
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {

        

        Debug.LogWarning(tInfo);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}