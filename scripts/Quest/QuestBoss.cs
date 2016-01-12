using UnityEngine;
using System.Collections;

public class QuestBoss : QuestKill {

	public GameObject BossGameObject;
	// Use this for initialization
	void Start () {
		CurrentObjectiveCount = 0;
		;
	}
	
	// Update is called once per frame
	void Update () {
		if (Given) {
			BossGameObject.SetActive(true);
		}
	}
}
