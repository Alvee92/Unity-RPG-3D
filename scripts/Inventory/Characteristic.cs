using UnityEngine;
using System.Collections;

public class Characteristic : Inventory {

	private string[] SlotName; //the names of the différents slots (Head, chest etc)
	public int[] PlayerCharacteristics;

	private int SkillPoint = 1000;
	public int MaxHealth;
	public int Magic;
	public int Strength;
	public int Defense;
	public int Agility;
	public int Wisdom;

	// Use this for initialization
	void Awake () {
		PlayerManager = GameObject.Find ("Player").GetComponent<PlayerManager> ();
		MainWindow = new Rect ((Screen.width/2)+375, 10, 300, 540); //the size of the window
		SlotName = new string[6] {"Health","Magic","Strength","Defense","Agility","Wisdom"};
		PlayerCharacteristics = new int[6] {MaxHealth,Magic,Strength,Defense,Agility,Wisdom};


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUI.skin = skin;
		if(activate) //activate the quest manager
		{
			MainWindow = GUI.Window (9, MainWindow, DisplayCharacteristic, "");
			//now adjust to the group. (0,0) is the topleft corner of the group.
			GUI.BeginGroup (new Rect (0,0,100,100));
			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();
			
			
		}
		
		if(IsOver) {
			//Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			ItemInfoWindow = new Rect (Input.mousePosition.x-15-350,Screen.height-Input.mousePosition.y-120, 350, 340);
			ItemInfoWindow = GUI.Window (8, ItemInfoWindow, DisplayInfo, "");
			
		}
		
	}
	public virtual void DisplayInfo(int windowID)
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (8);
		GUILayout.Label ("", "Divider");
		GUILayout.Label ("Info");
		GUILayout.Label ("", "Divider");
		GUILayout.Label ("Health 1 skill point for 10 health point"+"\nWisdom 2 skill points for 1 Wisdom point", "PlainText");
		GUILayout.EndVertical ();
	}
	void DisplayCharacteristic(int windowID)
	{
		GUILayout.BeginVertical();
		GUILayout.Space(8);
		GUILayout.Label("", "Divider");
		GUILayout.Label("Characteristic");
		GUILayout.BeginHorizontal(); //print the slots 2 by lines
		GUILayout.Label ("Skill points: "+SkillPoint.ToString(), "PlainText");
		if (GUILayout.Button (new GUIContent("?","Info"), "ShortButton", GUILayout.Width (50), GUILayout.Height (20))) {


		}
		if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip) {
			if (lastTooltip != "")
			{
				IsOver = false;
			}
			if (GUI.tooltip != "")
			{
				IsOver = true;
			}
			lastTooltip = GUI.tooltip;
		}
		GUILayout.EndHorizontal ();
		GUILayout.Label("", "Divider");
		for(int i = 0; i<SlotName.Length; i++)
		{
			GUILayout.BeginHorizontal(); //print the slots 2 by lines

			if(GUILayout.Button (new GUIContent(SlotName[i], i.ToString()), "ShortButton", GUILayout.Width (100), GUILayout.Height (55))) 
					{
				if(SkillPoint>0)
				{
					DisplayPoint(i);
					IsOver = false;
				}
					}


			GUILayout.Label("\n"+PlayerCharacteristics[i],"TextArea");
			GUILayout.EndHorizontal();
					
				}
			
			

		
		
		GUILayout.EndVertical();
		GUI.DragWindow (new Rect (0,0,10000,10000));
	}

	public void AddSkillPoint()
	{
		SkillPoint += 4;
	}

	public void DisplayPoint(int Id)
	{
		if (Id == 5 && SkillPoint > 2) {
			Wisdom++;
			SkillPoint -= 2;
			PlayerCharacteristics [5] = Wisdom;
		}
		else if (Id == 0) {
			MaxHealth += 10;
			PlayerManager.UpdateManager ();
			PlayerManager.AddHealth (10);
			SkillPoint--;
			PlayerCharacteristics [0] = MaxHealth;
			} 
		 else if(Id!= 5){
			PlayerCharacteristics [Id]++;
			SkillPoint--;

		}
		if (GameObject.FindGameObjectWithTag ("Weapon") != null) {
			GameObject.FindGameObjectWithTag ("Weapon").GetComponent<PlayerAttack>().SetSpeed();
		}

	}




}
