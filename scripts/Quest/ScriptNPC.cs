using UnityEngine;
using System.Collections;

public class ScriptNPC : QuestManager {

	private bool ShowGUI = false; //show the GUI
	public string Name;  //The name of the NPC
	public string MessageNPC; //the message ofthe NPC (i.e what he says to the player)
	private GameObject Player; //reference to the player
	private QuestManager PlayerQuestManager;  //reference to the qest manager script of the player
	private Quest[] PlayerListQuest; //the list of quest of the player

	
	// Use this for initialization
	void Awake () {


	}
	void Start()
	{
		ListQuest = transform.FindChild("Quest Giver").GetComponentsInChildren<Quest>();
		TotalPages = ListQuest.Length / 10; //we print 10 quests per pages
		Player = GameObject.Find ("Player");
		PlayerManager = Player.GetComponent<PlayerManager> ();
		PlayerQuestManager = Player.transform.FindChild ("Quest Manager").GetComponent<QuestManager> ();
		PlayerListQuest = PlayerQuestManager.GetComponentsInChildren<Quest> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ShowGUI) {
			if (!PlayerManager.IsPaused) {
				if (Input.GetKeyDown ("e")) {
					activate = true;
					PlayerManager.IsPaused = true;
					thisPause = true;
				}
				
			}
		}
			else if(PlayerManager.IsPaused && thisPause)
			{
				if(Input.GetKeyDown("e"))
				{				

					activate = false;
				}
				PlayerManager.IsPaused = activate;
				thisPause = activate;
				ShowGUI = !activate;
				
			}
		
	}
	void OnTriggerEnter(Collider col)
	{
		
		if(col.gameObject.tag == "Player") //if the player enter in the colider of the NPC
		{

			ShowGUI = true; //show the GUI messgae
			CheckQuest();
			
		}
		
	}
	void OnTriggerExit(Collider col)
	{

		if(col.gameObject.tag == "Player") //if the player get out of the trigger
		{
			ShowGUI = false;  //if a quest was selected, desable it
			activate = false;  //desactivate the GUI message
			
		}
	}
	void OnGUI()
	{

		float width1 = Screen.width * 0.5f - (165 * 0.5f); //configuring the message GUI
		float width2 = Screen.width * 0.5f - (185 * 0.5f);
		if(ShowGUI)
		{
			if (transform.name.Length <= 7) {
				GUI.Box (new Rect (width1, 200, 165, 44), Name + "\nPress E to speak.");
			} else {
				GUI.Box (new Rect (width2, 200, 185, 44), Name +"\nPress E to speak.");
				
			}
		}
		GUI.skin = skin; //eneble the skin
		if(activate) //if the player has talked ti the npc
		{
			ShowGUI = false; //disable the open message
			windowRect0 = GUI.Window (4, windowRect0, Questmanager, ""); //show the information of the quest
			GUI.BeginGroup (new Rect (0,0,100,100));
			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();

			if (showquest) //if we have chosen a quest
			{
				windowRect0 = GUI.Window (4, windowRect0, ShowQuest, ""); //show the information of the quest
				GUI.BeginGroup (new Rect (0,0,100,100));
				// End the group we started above. This is very important to remember!
				GUI.EndGroup ();
			}
			else{
				showquest = false;
				}
		}
	}
	 protected override void Questmanager(int windowID ) 
	{
		
		PlayerQuestManager.UpdateList (); //update the quest list of the player
		UpdateList (); //update the quest list of the npc

		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.Label("", "Divider");
		GUILayout.Label(Name);
		GUILayout.Label("", "Divider");
		GUILayout.Label("", "Divider");
		GUILayout.Label(MessageNPC, "PlainText"); //Write here the message of the NPC
		GUILayout.Label("", "Divider");
		if (ListQuest.Length > 0) {
			GUILayout.Box ("Chose your quest");
			//GUILayout.Label("", "Divider");
			GUILayout.Label ("", "Divider");
			for (int i = (PageQuest-1)*9; i<(PageQuest )*9 && i<ListQuest.Length; i++) { //loop to print the 10 first quests
				Quest q = ListQuest [i];
				if (GUILayout.Button (q.Title, "ShortButton")) { //for each quest a button to select it
				
					showquest = true; //show the quest
					currentQuest = q; //set the current quest shown
					ShowQuest (0);
				}
			}
		}
			GUILayout.Label("", "Divider");
			GUILayout.Label("Current Quest:", "PlainText");
			GUILayout.Label("", "Divider");
			foreach (Quest q in PlayerListQuest) { // this loop print each quest of the player linked to this NPC
				if (q.Receiver == Name) { //if the NPC is the receiver of the quest (i.e the player must give back the quest to this npc)
					if (GUILayout.Button (q.Title, "ShortButton")) { //here we chose the quest to show, to either finish it or show details
						
						showquest = true; //show the quest
						currentQuest = q; //set the current quest shown
						ShowQuest (0);
					}
				}
			}

		
		//GUILayout.Label("", "Divider");//-------------------------------- custom
		GUILayout.Label("", "Divider");//-------------------------------- custom
		GUILayout.EndVertical();
		
		
		
		
		
		
		GUILayout.BeginArea (new Rect (25 , 500, 400, 200));
		GUILayout.BeginHorizontal ();
		if (PageQuest > 1) { //to print the previous button, the page must be different than the 0-th
			if (GUILayout.Button ("Previous page", "ShortButton", GUILayout.Width (200))) {
				PageQuest--;
			}
		}
		if (PageQuest < TotalPages) { //to print the next button, the page must be lesser than the last
			if (GUILayout.Button ("Next page", "ShortButton", GUILayout.Width (200))) {
				PageQuest++;
			}
		}
		GUILayout.EndHorizontal();
		
		
		if(GUILayout.Button("Close"))
		{
			activate = false;
		}
		GUILayout.EndArea ();
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}

	protected override void ShowQuest(int windowID)
	{
		if (currentQuest.Given && !currentQuest.IsCleared) { //if the quest isn't cleared and the quest has been given to the player
			base.ShowQuest (4) ; //show the information about the quest (i.e the quest isn't finished yet so it's impossible to finish it)
		} else { 
			GUILayout.BeginVertical ();
			GUILayout.Space (8);
			GUILayout.Label ("", "Divider");//-------------------------------- custom
			GUILayout.Label (currentQuest.Title);
			GUILayout.Label ("", "Divider");
			GUILayout.Label ("", "Divider");
			GUILayout.Label (currentQuest.Text, "PlainText");
			GUILayout.Label ("", "Divider");
			GUILayout.Label ("", "Divider");
			GUILayout.Label (currentQuest.DisplayObjectiveNPC (), "PlainText");
			GUILayout.Label("", "Divider");
			GUILayout.Label("", "Divider");
			GUILayout.Label (currentQuest.DisplayReward (), "PlainText");

			GUILayout.BeginArea (new Rect (25, 500, 400, 200));
			if (currentQuest.IsCleared) { //if the quest is finished

				if (GUILayout.Button ("Finish")) { //we show the finish button
					showquest = false;
					currentQuest.GetReward (); //give the reward of the quest
					Destroy (currentQuest.gameObject); //destroy the quest
				}
			} else {
				if (GUILayout.Button ("Accept")) { //else it means that the NPC is giving a quest
					showquest = false;
					GiveQuest (currentQuest); //move the quest to the player Quest Manager
					currentQuest.Given = true;
				}
			}
				if (GUILayout.Button ("Return")) {
					showquest = false;
				}
		
				GUILayout.EndArea ();
				GUILayout.EndVertical ();
				GUI.DragWindow (new Rect (0, 0, 10000, 10000));
			
		}
	}
	public void GiveQuest(Quest q)
	{
		q.transform.parent = PlayerQuestManager.transform;
	}
	public override void UpdateList()
	{
		ListQuest = transform.FindChild("Quest Giver").GetComponentsInChildren<Quest>();
		PlayerListQuest = PlayerQuestManager.GetComponentsInChildren<Quest> ();
		
	}

	public void CheckQuest()
	{
		UpdateList ();
		foreach(Quest q in PlayerListQuest)
		{

			q.CheckObjective(Name); //check is the quest q is cleared
			
		}
	}
}
