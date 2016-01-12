using UnityEngine;
using System.Collections;

public class Quest : MonoBehaviour {

	public string Title ;
	public string Text ;
	public string Giver;
	public string Receiver;
	public bool Given = false;
	protected bool Cleared =false;
	public bool DisplayObjective = false;
	public int Gold = 0;
	public int Xp = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


		


	public virtual void CheckObjective(string tag)
	{
	}
	public virtual string DisplayObjectivePlayer()
	{
		return "";
	}
	public virtual string DisplayObjectiveNPC()
	{
		return "";
	}
	public string DisplayReward()
	{

		return  "Reward: " + Gold + " gold and " + Xp + " xp.";
	}
	public void GetReward()
	{
		GameObject.Find ("Player").GetComponent<PlayerManager> ().AddXp (Xp);
		GameObject.Find ("Player").GetComponent<PlayerManager> ().AddGold(Gold);
	}
	public bool IsCleared{
		get
		{
			return this.Cleared;
		}
	}


}