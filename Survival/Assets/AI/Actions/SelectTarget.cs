using UnityEngine;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;
using RAIN.Perception.Sensors;
using RAIN.Entities.Aspects;

[RAINAction]
public class SelectTarget : RAINAction
{
	protected IList<RAINAspect> playerSensed;
	protected RAIN.Core.AI ai;
	protected float visual_range;
	protected VisualSensor sensor;
    private BasicStats basicStats;

    public override void Start(RAIN.Core.AI ai)
	{
		
		base.Start(ai);
		this.ai = ai;
		basicStats = (BasicStats)ai.Body.GetComponent(typeof(BasicStats));
		this.sensor = ai.Senses.GetSensors(null, true)[0] as VisualSensor;
		this.visual_range = sensor.Range;

       
    }
	
	public override ActionResult Execute(RAIN.Core.AI ai)
	{
       
		sensor.Sense("Creature", RAINSensor.MatchType.ALL);
		playerSensed = sensor.Matches;
		ai.WorkingMemory.SetItem<GameObject>("FinalTarget", processPlayers(playerSensed));
		
		return ActionResult.RUNNING;
	}
	
	public override void Stop(RAIN.Core.AI ai)
	{
		base.Stop(ai);
	}
	
	
	protected GameObject processPlayers(IList<RAINAspect> matches){
		int minScore = 90000;
		RAINAspect maxAspect = null;

		for (int i = 0; i < matches.Count; i++) {
			BasicStats entityStats = matches[i].Entity.Form.GetComponent<BasicStats>();
            if (entityStats.Team == basicStats.Team)
            {
                //Skip teammates or dead ones.
                continue;
            }
			int localScore = 0;
			int dist = getDistanceEnum (matches [i]);
			localScore = dist;
			localScore = entityStats.CurrentHealth;

			if(localScore < minScore)
			{
				minScore = localScore;
				maxAspect = matches[i];
			}
		}
		if (maxAspect == null)
			return null;

		return maxAspect.Entity.Form;
		
	}//select creature by priority
	
	
	public int getDistanceEnum(RAINAspect a){
		float distance = Vector3.Distance (ai.Kinematic.Position, a.Position);
		//if object is 2/3 to farthest from agent, return
		//Debug.Log (g.name + "distance, visual range from" + type + distance.ToString() + ", " + visual_range.ToString());
		if( distance>= visual_range/2) return 1;
		//if within 1/3rd of sensing distance
		if(distance <= visual_range/2) return 2;
		else{return 0;}
		
	}//get distance enum
}