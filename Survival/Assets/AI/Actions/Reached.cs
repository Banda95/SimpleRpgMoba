using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Motion;
using RAIN.Representation;

[RAINDecision]
public class Reached : RAINDecision
{

    public Expression Enemy = new Expression();
    public Expression ActivationDistance = new Expression();

    private float tDistance = 0f;
    private GameObject tEnemy;

    private int _lastRunning = 0;

    public override void Start(RAIN.Core.AI ai)
    {
        if (ActivationDistance.IsValid)
        {
            tDistance = ActivationDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
        }

        base.Start(ai);

        _lastRunning = 0;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.SUCCESS;

        if (Enemy.IsValid && Enemy != null)
        {
            tEnemy = Enemy.Evaluate<GameObject>(ai.DeltaTime, ai.WorkingMemory);
            Vector3 distanceVector = ai.Kinematic.Position - tEnemy.transform.position;
            distanceVector.y = 0;

            if (distanceVector.magnitude > tDistance)
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