using RAIN.Action;
using RAIN.Navigation;
using RAIN.Navigation.Graph;
using RAIN.Navigation.NavMesh;
using RAIN.Representation;
using System.Collections.Generic;
using UnityEngine;

[RAINAction]
public class RandomLocation : RAINAction
{
    public Expression Target = new Expression();

    public Expression Distance = new Expression();

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        // Get any graphs at our AI's position
        List<RAINNavigationGraph> tGraphs = NavigationManager.Instance.GraphForPoint(ai.Kinematic.Position);
        if (tGraphs.Count == 0)
            return ActionResult.FAILURE;

        // Pretty much guaranteed it will be a navigation mesh
        NavMeshPathGraph tGraph = (NavMeshPathGraph)tGraphs[0];

        // Look for polys within the given distance
        float tDistance = 20;
        if (Distance.IsValid)
            tDistance = Distance.Evaluate<float>(ai.DeltaTime, ai.WorkingMemory);

        // We'll use the recently added oct tree for this
        List<NavMeshPoly> tPolys = new List<NavMeshPoly>();
        tGraph.PolyTree.GetCollisions(new Bounds(ai.Kinematic.Position, Vector3.one * tDistance * 2), tPolys);

        if (tPolys.Count == 0)
            return ActionResult.FAILURE;

        // Pick a random node
        NavMeshPoly tRandomPoly = tPolys[UnityEngine.Random.Range(0, tPolys.Count - 1)];

        // If the user set a Target variable, use it
        if (Target.IsVariable)
            ai.WorkingMemory.SetItem<Vector3>(Target.VariableName, tRandomPoly.Position);
        // Otherwise just use some default
        else
            ai.WorkingMemory.SetItem<Vector3>("randomLocation", tRandomPoly.Position);

        return ActionResult.SUCCESS;
    }
}