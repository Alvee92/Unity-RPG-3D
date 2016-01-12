using UnityEngine;
using System.Collections;

public class QuestKill : Quest {

	public string ObjectiveTag; //The tag of the ennemy to kill
	public int MaxObjectiveCount; //the number of ennemy to kill
	protected int CurrentObjectiveCount; //the number of ennemy killed


	// Use this for initialization
	void Start () {
		CurrentObjectiveCount = 0;
	}
	
	// Update is called once per frame
	void Update () {

			
	}
	public override void CheckObjective(string tag)
	{
		if (CurrentObjectiveCount < MaxObjectiveCount) {
			if (tag == ObjectiveTag) {

				CurrentObjectiveCount++;

			} 
		}
		if (CurrentObjectiveCount >= MaxObjectiveCount) {
			Cleared = true;
		}
	}
	public override string DisplayObjectivePlayer()
	{
		
		return "Current objective : " + CurrentObjectiveCount + "/" + MaxObjectiveCount;
	}
	public override string DisplayObjectiveNPC()
	{
		
		return "Objective : Kill " +  MaxObjectiveCount + " " + ObjectiveTag;
	}
}
