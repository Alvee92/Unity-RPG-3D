using UnityEngine;
using System.Collections;
using System;

public abstract class Item : MonoBehaviour {


	public string Name; //the name of the item
	public Texture2D Icon;  //the icon to display

	protected GameObject Player;//reference to the player
	protected Inventory PlayerInventory; //reference to the inventory
	protected GameObject CharacterContener; //reference to the equipped item 


	public string ItemType;
	public bool IsSingle = true;

	public int price;

	// Use this for initialization
	void Start() {
		Player = GameObject.Find ("Player");
		PlayerInventory = Player.transform.FindChild ("Inventory").GetComponent<Inventory> ();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void PickUpItem() //when the player pick up the item
	{
		Start ();
		if (!PlayerInventory.IsFull ()) { //if there a no same item in the inventory

			transform.parent = PlayerInventory.transform;
			GetComponent<FirstPersonPickUp>().enabled = false;
			GetComponent<MeshRenderer> ().enabled = false;
			GetComponent<MeshCollider> ().enabled = false;
			PlayerInventory.UpdateContent();
		} else {
			Debug.Log ("inventory full");
		}


	}
	public abstract void SetItem (); //this method is called whe, the player click on an item in the inventory, it will either equip the item or use the effetc depend on the nature of the item

	public abstract void UseEffect ();


	public abstract string DisplayCharacteristic ();
	
	public virtual void DropItem()
	{
		transform.parent = null;
		//transform.position = Player.transform.FindChild("PlayerDropItem").transform.position;

		//GetComponent<FirstPersonPickUp>().enabled = true; //enable the script and the component of the ddropped item
		//GetComponent<MeshRenderer> ().enabled = true;
		//GetComponent<MeshCollider> ().enabled = true;
		Destroy (gameObject);
		PlayerInventory.UpdateContent ();
	}
	public virtual bool AddStack()
	{
		return true;
	}

	public virtual void SellItem()
	{
		Player.GetComponent<PlayerManager> ().AddGold (price / 2);
		transform.parent = null;
		Destroy (gameObject);
	}
	public virtual void ApplyBonus()
	{
	}
	public virtual void UnApplyBonus()
	{
	}


}
