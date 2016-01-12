using UnityEngine;
using System.Collections;

public class Consumable : Item {

	public int Health;
	public int stack; //the number of the item in the inventory
	public int MaxStack; //the max stack possible for an item
	private bool added = false; //variable to know if the item has been added
	public bool stackable = false; //if the item is stackable

	private PlayerManager PlayerHealth;

	// Use this for initialization
	void Awake () {
		Player = GameObject.Find ("Player");
		PlayerInventory = Player.transform.FindChild ("Inventory").GetComponent<Inventory> ();
		CharacterContener = Player.transform.FindChild ("Equipement").gameObject;
		PlayerHealth = Player.GetComponent<PlayerManager> ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override string DisplayCharacteristic()
	{
		string result;
		result = "Health: +" + Health + "\n" + "Stack: " + stack.ToString ();

		
		return result;
	}
	public override void SetItem()
	{
		UseEffect(); //use effect
		if(stack > 1)
		{
			stack --;
			PlayerInventory.UpdateContent ();

		}
		else{Destroy (gameObject);
			transform.parent = null;
			PlayerInventory.UpdateContent ();

		}
	}
	
	public override void UseEffect()
	{
		PlayerHealth.AddHealth (Health);
	}
	public override void PickUpItem()
	{
		if (stackable) { //if the item is stackable
			for (int i = stack; i>0; i--) {
				foreach (Item consu in PlayerInventory.Content) { //search in the player inventory the same item
					if (consu.Name == Name) { //if we find the same item
					

						added = consu.AddStack ();

						if(added){stack --;}
						if(stack == 0){Destroy(gameObject);}
						

					
					}
				}
				if (!added && !PlayerInventory.IsFull ()) { // if all the same item in the inventory have stack max 

					transform.parent = PlayerInventory.transform; //put the item in the inventory
					GetComponent<FirstPersonPickUp> ().enabled = false;
					GetComponent<MeshRenderer> ().enabled = false; //disable the mesh renderer and collider
					GetComponent<MeshCollider> ().enabled = false;
					PlayerInventory.UpdateContent ();
					i =0;
				}
			
			} 
		}
	}
	public override bool AddStack()
	{
		if (stack < MaxStack) { //if the number of item is not maxed
			
			stack ++; //add to the stack

			PlayerInventory.UpdateContent ();

			return true;
			
			
		} else {
			return false;
		}
	}

	public override void DropItem()
	{
		if (stack > 1) {
			stack--;
			PlayerInventory.UpdateContent();
		} else {
			transform.parent = null;
			Destroy (gameObject);
			PlayerInventory.UpdateContent ();

		}
	}

	public override void SellItem()
	{
		if (stackable) {
			if(stack >1){
				stack--;
				Player.GetComponent<PlayerManager>().AddGold(price/2);
			}
			else 
			{
				Player.GetComponent<PlayerManager>().AddGold(price/2);
				transform.parent = null;
				Destroy(gameObject);
			}

		}
	}


}
