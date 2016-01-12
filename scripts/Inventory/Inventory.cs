using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

	public GUISkin skin ; //the skin
	protected Rect MainWindow = new Rect ((Screen.width/2)-225, 10, 610, 540); //the size of the window
	protected Rect DropItemWindow = new Rect ((Screen.width/2)-120, 160, 400, 250); //the size of the window
	public Item[] Content; //the content of the inventory
	public int MaxContent = 10; //the max size of the inventory
	public bool activate = false; //activate the window
	protected bool thisPause = false; //if this script paused th game


	protected int PageInventory = 1;  //the number of the page which contain quest
	protected float TotalPages = 1; //total number of pages 


	protected PlayerManager PlayerManager; //refeerence to the PlayerManager script
	protected Character characterScript;
	protected Characteristic characteristicScript;
	public Inventory InteractionWindow;
	public Transform thisInteraction;
	protected Rect ItemInfoWindow ; //the size of the window



	public bool InteractionFound = false;

	protected string lastTooltip = " ";
	protected bool IsOver = false;
	protected Item ItemOver;
	protected bool dropItem = false;


	// Use this for initialization
	void Awake () {
	

		Content = GetComponentsInChildren<Item>();
		PlayerManager = GameObject.Find ("Player").GetComponent<PlayerManager> ();
		characterScript = GameObject.Find ("Player").transform.Find ("Equipement").GetComponent<Character> ();
		characteristicScript = GameObject.Find ("Player").GetComponent<Characteristic> ();
		
		TotalPages = Mathf.Ceil(Content.Length / 10.0f) ; //we write 10 item per page




	}
	
	// Update is called once per frame
	void Update () {
		if(!PlayerManager.IsPaused) //if the game is'nt paused
		{
			if(Input.GetKeyDown("i")) //if we open the Quest manager
			{
				activate = true;  //show it 
				characterScript.CharActivate = true; //activate the character window
				characteristicScript.activate = true; //activate the character window
				PlayerManager.IsPaused = true; //pause the game
				thisPause = true; //inform that this script paused the game
			}
			
		}
		else if(PlayerManager.IsPaused && thisPause) //else if the game is paused, and this script paused the game, we can unpause
		{
			if(Input.GetKeyDown("i")) //close the quest manager
			{
				activate = false;
				characterScript.CharActivate = false;
				characteristicScript.activate = false; //activate the character window

			}
			PlayerManager.IsPaused = activate; //unpause
			thisPause = activate;
			
		}
	
	}

	public bool IsFull()
	{
		if (Content.Length >= MaxContent) {
			return true;
		} else {
			return false;
		}
	}



	void OnGUI()
	{

		if(activate) //activate the quest manager
		{
			GUI.skin = skin;
			MainWindow = GUI.Window (0, MainWindow, DisplayInventory, "");
			//now adjust to the group. (0,0) is the topleft corner of the group.

			

		}
		if(IsOver) {
			//Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			ItemInfoWindow = new Rect (Input.mousePosition.x+15,Screen.height-Input.mousePosition.y+15, 250, 240+30);
			ItemInfoWindow = GUI.Window (5, ItemInfoWindow, DisplayItem, "");
			if(!activate){IsOver = false;}

		}
		if(dropItem) {
			//Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			IsOver = !dropItem;
			DropItemWindow = GUI.Window (6, DropItemWindow, DropItem, "");
			if(!activate){dropItem = false;}
			
		}

	}
	public virtual void DisplayItem(int windowID)
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (8);
		GUILayout.Label ("", "Divider");
		GUILayout.Label (ItemOver.Name);
		GUILayout.Label ("", "Divider");
		GUILayout.Label(ItemOver.DisplayCharacteristic(),"PlainText");
		GUILayout.Label("Sell price: "+ItemOver.price/2,"PlainText");
		GUILayout.EndVertical ();
	}

	public void DropItem(int windowID)
	{
		thisPause = false;
		GUILayout.BeginVertical ();
		GUILayout.Space (8);
		GUILayout.Label ("", "Divider");
		GUILayout.Label ("Drop Item");
		GUILayout.Label ("", "Divider");
		GUILayout.Label("Do you really want to drop "+ItemOver.Name,"TextArea");
		GUILayout.BeginHorizontal ();

		if (GUILayout.Button ("Yes", "ShortButton", GUILayout.Width (100))) {
			dropItem = false;
			ItemOver.DropItem();
			thisPause = true;


			}
		if (GUILayout.Button ("No", "ShortButton", GUILayout.Width (100))) {
			dropItem = false;
			thisPause = true;
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndVertical ();
	}

	void DisplayInventory(int windowID)
	{
		GUILayout.BeginVertical ();
		GUILayout.Space (8);
		GUILayout.Label ("", "Divider");
		GUILayout.Label ("Inventory");
		GUILayout.Label ("", "Divider");

		int temp = (PageInventory - 1) * 10; //variable to know what is the number of the item to print
		for (int j = 1; j<3; j++) { //we wirte two sets of 5 item per page
			GUILayout.BeginHorizontal (); //begin the first set
			for (int i = temp; i<(((PageInventory )*10)/2)*j && i<Content.Length; i++) { //write the first 5 items (or the 5 next in funtion of j)
				Item item = Content [i];

				if (GUILayout.Button (new GUIContent(item.Icon,i.ToString()), "ShortButton", GUILayout.Width (100), GUILayout.Height (100))) {
				
					if (Input.GetMouseButtonUp(0)) {
						{
							IsOver = false;
							if (InteractionFound) { //if an intaraction window is found (chest or seller)
								InteractionWindow.Sell(item); // it will sell the item if the interaction is a seller, or put the item in a chest
								InteractionWindow.UpdateContent ();
								UpdateContent ();
							} else {
								item.SetItem(); //equip the item
								UpdateContent ();
							}
						}
					}
					if (Input.GetMouseButtonUp(1)) { //if right click, drop the item
						ItemOver = item;
						dropItem = true;
						IsOver = false;




					}
			
				}
				if (Event.current.type == EventType.Repaint && GUI.tooltip != lastTooltip) {
					if(!dropItem){
					if (lastTooltip != "")
					{
						IsOver = false;
					}
					if (GUI.tooltip != "")
					{
						IsOver = true;
						ItemOver = item;
					}
					lastTooltip = GUI.tooltip;
				}
			}
			}
				GUILayout.EndHorizontal ();
				GUILayout.Label ("", "Divider");
				temp = (((PageInventory) * 10) / 2) * j;
			}
		GUILayout.Label ("Page " + PageInventory + "/" + TotalPages, "PlainText");
		GUILayout.Label ("", "Divider");
			
			GUILayout.BeginArea (new Rect (25, 420, 560, 200));


			GUILayout.BeginHorizontal ();
			if (PageInventory > 1) { //to print the previous button, the page must be different than the 0-th
				if (GUILayout.Button ("Previous page", "ShortButton", GUILayout.Width (200))) {
					PageInventory--;
				}
			}
			if (PageInventory < TotalPages) { //to print the next button, the page must be lesser than the last
				if (GUILayout.Button ("Next page", "ShortButton", GUILayout.Width (200))) {
					PageInventory++;
				}
			}
			GUILayout.EndHorizontal ();
			if (GUILayout.Button ("Return")) {
				activate = false;
				if (InteractionFound) {
					InteractionWindow.activate = false;
					
				} else {
					characterScript.CharActivate = false;
					characteristicScript.activate = false; //activate the character window

				}
			}
		
			GUILayout.EndArea ();
			GUILayout.EndVertical ();
			GUI.DragWindow (new Rect (0, 0, 10000, 10000));
	}

	public virtual void UpdateContent()
	{
		Content = GetComponentsInChildren<Item>();
		TotalPages = Mathf.Ceil(Content.Length / 10.0f) ;

	}

	public virtual void Sell(Item item)
	{

	}
	

}
