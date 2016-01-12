using UnityEngine;
using System.Collections;

public class Weapon : Item {

	public GameObject AssociatedWeapon;
	private PlayerController PlayerController;
	// Use this for initialization
	void Awake () {
		CharacterContener = GameObject.Find ("Player").transform.Find ("Equipement").gameObject;
		PlayerController = GameObject.Find ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override string DisplayCharacteristic()
	{
		string result;
		result = "Dammage: " + AssociatedWeapon.GetComponent<PlayerAttack> ().Dammage
			+"\nPrice: "+ price;

		
		return result;
	}

	public override void SetItem()
	{
		transform.parent = CharacterContener.transform;
		CharacterContener.GetComponent<Character> ().Equip (this);
		GameObject AssociatedWeap = Instantiate(AssociatedWeapon) as GameObject; //create a version a the Weapon item
		if (PlayerController.HoldWeapon) {
			AssociatedWeap.transform.parent = GameObject.FindGameObjectWithTag ("Melee").transform; //put it in the melee

			AssociatedWeap.transform.localPosition = new Vector3 (-0.029f, 0.105f, 0.049f);
			AssociatedWeap.transform.localRotation = Quaternion.Euler(342.8103f,240.5572f,88.73794f);

			
		} else {
			AssociatedWeap.transform.parent = GameObject.FindGameObjectWithTag ("Back").transform; //put it in the melee
			AssociatedWeap.transform.localPosition = new Vector3 (0.05421448f, 0.3341688f, -0.1539937f);
			AssociatedWeap.transform.localRotation = Quaternion.Euler(79.88541f,240.7424f,61.1235f);


		}
		AssociatedWeap.GetComponent<PlayerAttack> ().SetSpeed ();

		PlayerInventory.UpdateContent ();
	}

	public override void UseEffect()
	{
		
	}
	public override void ApplyBonus ()
	{
	}
}
