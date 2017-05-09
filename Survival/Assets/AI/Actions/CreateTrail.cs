using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Motion;
using RAIN.Representation;
[RAINAction]
public class CreateTrail : RAINAction
{

    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// The target to build the trail.
    /// </summary>
    public Expression Target = new Expression();

    /// <summary>
    /// Public Expressions are editable in the Behavior Editor
    /// FleeTargetVariable is the name of the variable that the result will be assigned to
    /// *Don't use quotes when typing in the variable name
    /// </summary>
    public Expression LastPositionVariable = new Expression();

    private GameObject tTarget = null;

    public override void Start(RAIN.Core.AI ai)
    {

        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (Target.IsValid)
            tTarget = Target.Evaluate<GameObject>(ai.DeltaTime, ai.WorkingMemory);

        if (tTarget == null)
            return ActionResult.FAILURE;

        
        ai.WorkingMemory.SetItem<Vector3>("LastKnownPosition", tTarget.transform.position);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}