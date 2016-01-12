using UnityEngine;
using System.Collections;

public class FirstPersonPickUp : MonoBehaviour {

	public GUISkin InstructionBoxSkin ; //The skin to use. Default one is 'OtherSkin' under the 'Resources' folder.
	public KeyCode ButtonToPress = KeyCode.E; //The button to press when picking up the item.
	public float PickUpDistance = 1.7f; //The distance from where the Item can be picked up. Remember that this is relative to the center of the Item and the center of the Player.
	
	//These store information about the Item, if we can pick it up, the Player and the distance to the Player.
	private bool canPickUp = false;
	private Item theItem ;
	private Transform thePlayer  ;
	private float dist = 9999f;
	// Use this for initialization
	void Awake () {
		theItem = GetComponent<Item>();
		thePlayer = GameObject.Find ("Player").transform;

		
		if (InstructionBoxSkin == null)
		{
			//InstructionBoxSkin = Resources.Load("OtherSkin", GUISkin) as Texture;
		}
	}
	void OnGUI ()
	{
		//This is where we draw a box telling the Player how to pick up the item.
		GUI.skin = InstructionBoxSkin;
		GUI.color = new Color(1, 1, 1, 0.7f);
		
		if (canPickUp == true)
		{
			if (transform.name.Length <= 7)
			{
				GUI.Box (new Rect (Screen.width*0.5f-(165*0.5f), 200, 165, 22), "Press E to pick up " + transform.name + ".");
			}
			else
			{
				GUI.Box (new Rect (Screen.width*0.5f-(185*0.5f), 200, 185, 22), "Press E to pick up " + transform.name + ".");
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if (thePlayer != null)
		{
			//This is where we enable and disable the Players ability to pick up the item based on the distance to the player.
			dist =  Vector3.Distance(thePlayer.position, transform.position);
			if (dist <= PickUpDistance)
			{
				canPickUp = true;
			}
			else
			{
				canPickUp = false;
			}
			
			//This is where we allow the player to press the ButtonToPress to pick up the item.
			if (Input.GetKeyDown(ButtonToPress) && canPickUp == true)
			{
				theItem.PickUpItem();

			}

		}
	}

	void OnDrawGizmosSelected () 
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, PickUpDistance);
	}

}
