using UnityEngine;
using System.Collections;

public class Character : Inventory{

	public Item[] EquippedItems; //the equipped items of the player	
	private string[] SlotName; //the names of the différents slots (Head, chest etc)

	public bool CharActivate = false; //activate the window

	private Inventory PlayerInventory;  //reference to th player inventory script

	public bool WeaponEquiped = false;


	// Use this for initialization
	void Awake () {
		EquippedItems = new Item[6];
		SlotName = new string[6] {"Head","Chest","Leg","Foot","Hand","Weapon"};
		PlayerManager = GameObject.Find ("Player").GetComponent<PlayerManager> ();
		PlayerInventory = GameObject.Find ("Player").transform.FindChild("Inventory").GetComponent<Inventory> ();
		MainWindow = new Rect ((Screen.width/2)-515, 10, 300, 540); //the size of the window
	}
	
	// Update is called once per frame
	void Update () {
		//PlayerInventory.CharacterFound = CharActivate;
	}

	public void Equip(Item item) //Equip an item selected in the inventory
	{
		switch (item.ItemType)
		{
		case "Head":
			if(EquippedItems[0] != null){UnEquip(EquippedItems[0]);} //unequip the current one
			EquippedItems[0] = item; //equip this item
			item.ApplyBonus();
			break;
		case "Chest":
			if(EquippedItems[1] != null){UnEquip(EquippedItems[1]);}
			EquippedItems[1] = item;
			item.ApplyBonus();
			break;
		case "Leg":
			if(EquippedItems[2] != null){UnEquip(EquippedItems[2]);}
			EquippedItems[2] = item;
			item.ApplyBonus();
			break;
		case "Foot":
			if(EquippedItems[3] != null){UnEquip(EquippedItems[3]);}
			EquippedItems[3] = item;
			item.ApplyBonus();
			break;
		case "Hand":
			if(EquippedItems[4] != null){UnEquip(EquippedItems[4]);}
			EquippedItems[4] = item;
			item.ApplyBonus();
			break;
		case "Weapon":
			if(EquippedItems[5] != null){UnEquip(EquippedItems[5]);}
			EquippedItems[5] = item;
			WeaponEquiped = true;
			item.ApplyBonus();
			break;
		
	}
}

	void OnGUI()
	{
		GUI.skin = skin;
		if(CharActivate) //activate the character window
		{
			MainWindow = GUI.Window (1, MainWindow, DisplayCharacter, "");
			//now adjust to the group. (0,0) is the topleft corner of the group.
			GUI.BeginGroup (new Rect (0,0,100,100));
			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();
			
			
		}

		if(IsOver) { //if the mouse in over an item
			ItemInfoWindow = new Rect (Input.mousePosition.x+15,Screen.height-Input.mousePosition.y+15, 250, 240); 
			ItemInfoWindow = GUI.Window (5, ItemInfoWindow, DisplayItem, "");
			
		}
		
	}


	void DisplayCharacter(int windowID)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.Label("", "Divider");
		GUILayout.Label("Equipement");
		GUILayout.Label("", "Divider");
		GUILayout.Label ("Level: "+PlayerManager.Level, "PlainText");
		for(int j = 0; j<3; j++)
		{
			GUILayout.BeginHorizontal(); //print the slots 2 by lines
			for(int i =j*2;i<j*2 +2; i++)
			{
				if(EquippedItems[i] == null) //when there is no equiped item, print the name of the slot
				{
					GUILayout.Button (SlotName[i], "ShortButton", GUILayout.Width (100), GUILayout.Height (100));


				}
				else{ //else print the icon of the item
					if(GUILayout.Button (new GUIContent(EquippedItems[i].Icon, i.ToString()), "ShortButton", GUILayout.Width (100), GUILayout.Height (100))) 
					{
						UnEquip(EquippedItems[i]); //if the player click it will unequip the item
						IsOver = false;
					}
					if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip) { //to know if the mouse is over an item
						if (lastTooltip != "")
						{
							IsOver = false;
						}
						if (GUI.tooltip != "")
						{
							IsOver = true;
							ItemOver = EquippedItems[i];
						}
						lastTooltip = GUI.tooltip;
					}
				
				}
			}
			GUILayout.EndHorizontal();
		}


		GUILayout.EndVertical();
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}
	public void UnEquip(Item item) //Un equip an item
	{
		item.UnApplyBonus (); //unapply the bonus of the item
		item.transform.parent = PlayerInventory.transform; //put back the item in the inventory
		for (int i = 0; i<EquippedItems.Length; i++) {
			if (EquippedItems [i] == item) {
				if (EquippedItems [i].ItemType == "Weapon") { //if it's a weapon whe have to destroy the Weapon version
					Destroy (GameObject.FindGameObjectWithTag("Weapon"));
				}
				EquippedItems [i] = null;
				WeaponEquiped = false;
			}
			PlayerInventory.UpdateContent ();
		}
	}



}
