using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Motion;
using RAIN.Representation;

[RAINAction]
public class EnemyNear : RAINAction
{
    public Expression Enemy = new Expression();
    public Expression ActivationDistance = new Expression();

    float tDistance = 0f;
    GameObject tEnemy;

    public override void Start(RAIN.Core.AI ai)
    { 
        
        if(ActivationDistance.IsValid)
            {
                tDistance = ActivationDistance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);
            }
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        
        if (Enemy.IsValid && Enemy != null)
        {
            tEnemy = Enemy.Evaluate<GameObject>(ai.DeltaTime, ai.WorkingMemory);
            Vector3 distanceVector = ai.Kinematic.Position - tEnemy.transform.position;
            distanceVector.y = 0;

            if (distanceVector.magnitude < tDistance)
                return ActionResult.SUCCESS;

        }

        return ActionResult.FAILURE;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}