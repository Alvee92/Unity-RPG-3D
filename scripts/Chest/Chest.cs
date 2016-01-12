using UnityEngine;
using System.Collections;
using System;

public class Chest : Inventory {


	bool opened = false; //if the chest is open
	bool showGUI = false; //show the GUI
	//bool IsOver = false;

	GameObject Player; //reference to the player
	GameObject PlayerInventory; //reference to the player inventory
	GameObject Gold; //reference to the gold item




	// Use this for initialization

	void Awake()
	{

		Player = GameObject.Find ("Player");
		PlayerManager = Player.GetComponent<PlayerManager> ();
		PlayerInventory = Player.transform.FindChild ("Inventory").gameObject;
		Content = transform.FindChild ("Contents").GetComponentsInChildren<Item> ();
		MainWindow = new Rect ((Screen.width/2)-615, 10, 400, 540); //the size of the window

		/*try{
		Gold = Contents.transform.FindChild ("Gold").gameObject; //if the chest containts gold we get it
		}
		catch(Exception e){
		}*/
		//ListContents =Contents.GetComponentsInChildren<Transform> ();

		/*foreach(Transform item in ListContents)
		{
			if(item.tag == "Item")
			{
				SendMessage ("AddItem",item, SendMessageOptions.DontRequireReceiver); //add all the items of the contents in the chest

			}

		}*/




	}

	void Start () {
	
	}
	void OnTriggerEnter(Collider col)
	{

//		Debug.Log (Inventory.name + Inventory.tag);
		if(col.gameObject.tag == "Player")
		   {
			showGUI = true;
			}

	}
	void OnTriggerExit(Collider col)
	{
		
		//		Debug.Log (Inventory.name + Inventory.tag);
		if(col.gameObject.tag == "Player")
		{
			showGUI = false;
		}
		
	}

	void OnGUI ()
	{
		//This is where we draw a box telling the Player how to pick up the item.


		
		if (showGUI) {
			if (!opened) { //if the chest is not opened show the message
				
				GUI.color = new Color(1, 1, 1, 0.7f);
				float width1 = Screen.width * 0.5f - (165 * 0.5f);
				float width2 = Screen.width * 0.5f - (185 * 0.5f);
				if (transform.name.Length <= 7) {
					GUI.Box (new Rect (width1, 200, 165, 22), "Press E to open the chest.");

				} else {
					GUI.Box (new Rect (width2, 200, 185, 22), "Press E to open the chest.");

				}
			}
			
		}
		GUI.skin = skin; //aply the skin
		if (activate) {

			MainWindow = GUI.Window (2, MainWindow, DisplayChest, "");
			GUI.BeginGroup (new Rect (0,0,100,100));
			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();
		}
		if(IsOver) {
			//Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			ItemInfoWindow = new Rect (Input.mousePosition.x+15,Screen.height-Input.mousePosition.y+15, 250, 240);
			ItemInfoWindow = GUI.Window (5, ItemInfoWindow, DisplayItem, "");
			
		}

	}

	// Update is called once per frame
	void Update () {


		if (!PlayerManager.IsPaused) { //if the game is'nt paused
			if (showGUI) { //if the player is in the collider of the chest

				if (Input.GetKeyDown ("e")) { 
					activate = !activate;  //activate /desactivate
					PlayerInventory.GetComponent<Inventory> ().activate = activate; //activate/desactivate in the same time the inventory
					PlayerInventory.GetComponent<Inventory> ().InteractionFound = activate; //inform the Inventory script that a chest is open
					PlayerInventory.GetComponent<Inventory> ().thisInteraction = transform.FindChild("Contents").transform; //inform the Inventory script which chest is open
					PlayerInventory.GetComponent<Inventory> ().InteractionWindow = this; ////inform the Inventory script with wich window to interact
					thisPause = true; //this script paused the game
					PlayerManager.IsPaused = true; //pause the game
			

					if (!opened) { //if the chest han never been opened

						GetComponent<Animation> ().Play (); //play the animation
						foreach (AnimationState state in GetComponent<Animation>()) {
							state.speed = 1;

						}
						opened = true;
					}
				}
				if(!activate)
				{
					PlayerInventory.GetComponent<Inventory> ().InteractionFound = false; //inform the inventory script to close the window

				}
			}
		} else if (PlayerManager.IsPaused && thisPause) { //else if the game is paused, and this script paused the game, we can unpause
			if (showGUI) {
				
				if (Input.GetKeyDown ("e")) {
					activate = !activate;
					PlayerInventory.GetComponent<Inventory> ().activate = activate;
					thisPause = false;
					PlayerManager.IsPaused = false;
				}
				PlayerManager.IsPaused = activate; //unpause
				thisPause = activate;

			}
		}
	}



	void DisplayChest (int windowsID)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.Label("", "Divider");
		GUILayout.Label("Chest");
		GUILayout.Label("", "Divider");

		for(int j = 0; j<3; j++)
		{
			GUILayout.BeginHorizontal();
			for(int i =j*3;i<j*3 +3 && i<Content.Length; i++)
			{
				if(GUILayout.Button (new GUIContent(Content[i].Icon,i.ToString()), "ShortButton", GUILayout.Width (100), GUILayout.Height (100)))
				{

					Content[i].PickUpItem();
					UpdateContent();
					IsOver = false;
				}
				if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip) {
					if (lastTooltip != "")
					{
						IsOver = false;
					}
					if (GUI.tooltip != "")
					{
						IsOver = true;
						ItemOver = Content[i];
					}
					lastTooltip = GUI.tooltip;
				}
				
				
				
					
				}
			GUILayout.EndHorizontal();

			}
			
		
		
		GUILayout.EndVertical();
		GUI.DragWindow (new Rect (0,0,10000,10000));


		

	}

	public override void UpdateContent()
	{
		Content = GetComponentsInChildren<Item>();

	}

	public override void Sell(Item item)
	{
		item.transform.parent = transform.FindChild ("Contents");
	}


}
