using UnityEngine;
using System.Collections;

public class QuestMeet : Quest {

	public string TextReceiver;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void CheckObjective(string name)
	{

		if (name == Receiver) {

			Text = TextReceiver;
			Cleared = true;
				

		}
	}
	public override string DisplayObjectivePlayer()
	{
		
		return "Current objective : Meet " + Receiver +".";
	}
	public override string DisplayObjectiveNPC()
	{
		
		return "Objective : Meet " + Receiver +".";
	}
}
