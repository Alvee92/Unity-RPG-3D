using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class QuestManager : MonoBehaviour {

	protected Rect windowRect0 = new Rect ((Screen.width/2)-225, 10, 450, 610);
	protected int PageQuest = 1;  //the number of the page which contain quest
	public int TotalPages;
	public GUISkin skin ;
	public bool activate = false;  //activate the windows or not
	protected Quest[] ListQuest;  //conatain the list of the quests
	protected Quest currentQuest; //the current quest
	protected bool showquest = false; //show the current quest
	protected PlayerManager PlayerManager; //refeerence to the PlayerManager script
	public bool thisPause = false; //if this script paused the game


	// Use this for initialization
	void Awake () {
		//Questmanager = transform.FindChild ("Quest Manager").gameObject;
		ListQuest = GetComponentsInChildren<Quest>();
		TotalPages = ListQuest.Length / 9 +1; //we print 9 quests per pages
		PlayerManager = GameObject.Find ("Player").GetComponent<PlayerManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(!PlayerManager.IsPaused) //if the game is'nt paused
		{
			if(Input.GetKeyDown("m")) //if we open the Quest manager
			{
				activate = true;  //show it 
				PlayerManager.IsPaused = true; //pause the game
				thisPause = true; //inform that this script paused the game
			}

		}
		else if(PlayerManager.IsPaused && thisPause) //else if the game is paused, and this script paused the game, we can unpause
		{
			if(Input.GetKeyDown("m")) //close the quest manager
			{
				activate = false;
			}
			PlayerManager.IsPaused = activate; //unpause
			thisPause = activate;

		}



	}




	public void CheckQuest(string tag)
	{

		foreach(QuestKill q in ListQuest)
		{
			q.CheckObjective(tag); //check is the quest q is cleared

		}
	}
	void OnGUI()
	{
		GUI.skin.font = skin.font;
		GUI.Label (new Rect(Screen.width-250, 200, 250, 250), DisplayObjectiveOnScreen());
			


		if(activate) //activate the quest manager
		{
			GUI.skin = skin;
			windowRect0 = GUI.Window (4, windowRect0, Questmanager, "");
			//now adjust to the group. (0,0) is the topleft corner of the group.
			GUI.BeginGroup (new Rect (0,0,100,100));
			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();

			if (showquest) //if we have chosen a quest
			{
				GUI.skin = skin;
				windowRect0 = GUI.Window (4, windowRect0, ShowQuest, ""); //show the information of the quest
				GUI.BeginGroup (new Rect (0,0,100,100));
				// End the group we started above. This is very important to remember!
				GUI.EndGroup ();
			}
		}
		else
		{
			showquest = false; //close the quest manager even if a quest is shown
		}

	}

	protected virtual void Questmanager (int windowID ) 
	{
		
		
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.Label("", "Divider");
		GUILayout.Label("Quest");
		GUILayout.Box("Chose your quest");
		GUILayout.Label("", "Divider");
		GUILayout.Label("", "Divider");
		for (int i = (PageQuest-1)*9; i<(PageQuest )*9 && i<ListQuest.Length; i++) {
			Quest q = ListQuest[i];
			GUILayout.BeginHorizontal();
			q.DisplayObjective = GUILayout.Toggle(q.DisplayObjective,"",GUILayout.Width (10));


				if(GUILayout.Button (q.Title, "ShortButton"))
			{
				showquest = true; //show the quest
				currentQuest = q; //set the current quest shown
				ShowQuest(0);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Label("", "Divider");//-------------------------------- custom
		GUILayout.Label ("Page " + PageQuest+"/" + TotalPages, "PlainText");
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

	protected virtual void ShowQuest(int windowsID)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.Label("", "Divider");//-------------------------------- custom
		GUILayout.Label(currentQuest.Title);
		GUILayout.Label("", "Divider");
		GUILayout.Label("", "Divider");
		GUILayout.Label(currentQuest.Text,"PlainText");
		GUILayout.Label("", "Divider");
		GUILayout.Label("", "Divider");
		GUILayout.Label (currentQuest.DisplayObjectivePlayer (), "PlainText");
		GUILayout.Label("", "Divider");
		GUILayout.Label("", "Divider");
		GUILayout.Label (currentQuest.DisplayReward (), "PlainText");
		GUILayout.BeginArea (new Rect (25 , 500, 400, 200));
		if (GUILayout.Button ("Return")) {
			showquest = false;
		}

		GUILayout.EndArea ();
		GUILayout.EndVertical();
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}
	public virtual void UpdateList()
	{
		ListQuest = GetComponentsInChildren<Quest>();

	}

	public string DisplayObjectiveOnScreen()
	{
		string text = "";
		foreach (Quest q in ListQuest) {
			if(q.DisplayObjective)
				text += q.Title +"\n" +q.DisplayObjectivePlayer() +"\n\n";
		}
		return text;
	}


}
