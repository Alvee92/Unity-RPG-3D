using UnityEngine;
using System.Collections;
using System;

public class Seller : Inventory {
	
	
	bool showGUI = false; //show the GUI
	//bool IsOver = false;
	
	GameObject Player; //reference to the player
	GameObject PlayerInventory; //reference to the player inventory
	GameObject Gold; //reference to the gold item
	
	public string NPCName;
	
	
	// Use this for initialization
	
	void Awake()
	{
		
		Player = GameObject.Find ("Player");
		PlayerManager = Player.GetComponent<PlayerManager> ();
		PlayerInventory = Player.transform.FindChild ("Inventory").gameObject;
		Content = transform.FindChild ("Contents").GetComponentsInChildren<Item> ();
		MainWindow = new Rect ((Screen.width/2)-615, 10, 400, 540); //the size of the window
		

		
		
		
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
		
		
		
		if (showGUI && !activate) {
				
				//GUI.color = new Color(1, 1, 1, 0.7f);
				float width1 = Screen.width * 0.5f - (165 * 0.5f);
				float width2 = Screen.width * 0.5f - (185 * 0.5f);
				if (transform.name.Length <= 7) {
					GUI.Box (new Rect (width1, 200, 200, 22), "Press E to talk to "+ NPCName);
					
				} else {
				GUI.Box (new Rect (width2, 200, 200, 22), "Press E to talk to "+ NPCName);
					
				}

			
		}
		GUI.skin = skin; //aply the skin
		if (activate) {
			
			MainWindow = GUI.Window (7, MainWindow, DisplayChest, "");
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
					PlayerInventory.GetComponent<Inventory> ().InteractionFound = activate; //inform the Inventory script that a seller is open
					PlayerInventory.GetComponent<Inventory> ().thisInteraction = transform.FindChild("Contents").transform; //inform the Inventory script which seller is open
					PlayerInventory.GetComponent<Inventory> ().InteractionWindow = this; ////inform the Inventory script with wich window to interact
					thisPause = true; //this script paused the game
					PlayerManager.IsPaused = true; //pause the game
					
					

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
		if (!activate) {
			IsOver = false;
		}
	}
	
	
	
	void DisplayChest (int windowsID)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.Label("", "Divider");
		GUILayout.Label("Seller");
		GUILayout.Label("", "Divider");
		
		for(int j = 0; j<3; j++)
		{
			GUILayout.BeginHorizontal();
			for(int i =j*3;i<j*3 +3 && i<Content.Length; i++)
			{
				if(GUILayout.Button (new GUIContent(Content[i].Icon,i.ToString()), "ShortButton", GUILayout.Width (100), GUILayout.Height (100)))
				{
					if(PlayerManager.BuyItem(Content[i]))
						if(!Content[i].IsSingle)
					{
						Item Clone = Instantiate(Content[i]) as Item;
						Clone.name = Content[i].Name;
						//Content[i].IsSingle = true;
						//Content[i]=Clone;

						Clone.PickUpItem();
						//Clone.transform.parent = transform.FindChild("Contents").transform;
					}
					else{Content[i].PickUpItem();}
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

	public override void DisplayItem(int windowID)
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (8);
		GUILayout.Label ("", "Divider");
		GUILayout.Label (ItemOver.name);
		GUILayout.Label ("", "Divider");
		GUILayout.Label(ItemOver.DisplayCharacteristic(),"PlainText");
		GUILayout.Label("Price: "+ItemOver.price,"PlainText");

		GUILayout.EndVertical ();
	}
	
	public override void UpdateContent()
	{
		Content = transform.FindChild ("Contents").GetComponentsInChildren<Item>();
		
	}
	public override void Sell(Item item)
	{
		item.SellItem ();//(item);
	}
}
