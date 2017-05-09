using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Motion;
using RAIN.Representation;

[RAINDecision]
public class StatsDecision : RAINDecision
{
    private int _lastRunning = 0;
    public Expression Value = new Expression();
    private int tValue = 0;

    public override void Start(RAIN.Core.AI ai)
    {
        if (Value.IsValid)
        {
            tValue = Value.Evaluate<int>(ai.DeltaTime, ai.WorkingMemory);
        }

        base.Start(ai);
        
        _lastRunning = 0;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.SUCCESS;

        BasicStats basicStats = (BasicStats)ai.Body.GetComponent(typeof(BasicStats));

        if(basicStats.CurrentHealth != tValue)
        {
            return ActionResult.FAILURE;
        }

        for (; _lastRunning < _children.Count; _lastRunning++)
        {
            tResult = _children[_lastRunning].Run(ai);
            if (tResult != ActionResult.SUCCESS)
                break;
        }

        return tResult;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}