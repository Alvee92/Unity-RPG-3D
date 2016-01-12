using UnityEngine;
using System.Collections;

public class Equipement : Item {

	// Use this for initialization
	public int Health,Magic,Armor,Strength,Agility,Wisdom;
	private int[] Var;
	private Characteristic Characteristic;
	public int[] PlayerCharacteristics;
	private string[] SlotName; //the names of the différents slots (Head, chest etc)


	
	void Awake () {
		CharacterContener = GameObject.Find ("Player").transform.Find ("Equipement").gameObject;
		Var = new int[6] {Health,Magic,Strength,Armor,Agility,Wisdom};
		Characteristic = GameObject.Find ("Player").GetComponent<Characteristic> ();
		SlotName = new string[6] {"Health","Magic","Strength","Defense","Agility","Wisdom"};
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override string DisplayCharacteristic()
	{
		string result = "\r";
		for (int i=0; i<Var.Length;i++) {
			if(Var[i] != 0)
			{

				result += SlotName[i] +": " + Var[i]+"\n" ;

			}
		}

		return result;
	}

	public override void SetItem()
	{
		transform.parent = CharacterContener.transform; //put the item in the character content
		CharacterContener.GetComponent<Character> ().Equip (this); //equip it
		PlayerInventory.UpdateContent ();

	}
	public override void UseEffect()
	{
		
	}
	public override void ApplyBonus()
	{

		for (int i=0; i<Var.Length;i++) { //for each characteristic of the item, add it to the player

				Characteristic.PlayerCharacteristics[i] += Var[i];
			}

		Characteristic.MaxHealth=Characteristic.PlayerCharacteristics[0];
		if (GameObject.FindGameObjectWithTag ("Weapon") != null) {
			GameObject.FindGameObjectWithTag ("Weapon").GetComponent<PlayerAttack>().SetSpeed();
		}

	}
	public override void UnApplyBonus()
	{
		for (int i=0; i<Var.Length;i++) {

				Characteristic.PlayerCharacteristics[i] -= Var[i];
			}

		Characteristic.MaxHealth=Characteristic.PlayerCharacteristics[0];
		if (GameObject.FindGameObjectWithTag ("Weapon") != null) {
			GameObject.FindGameObjectWithTag ("Weapon").GetComponent<PlayerAttack>().SetSpeed();
		}
		//GameObject.FindGameObjectWithTag ("Weapon").GetComponent<PlayerAttack> ().SetSpeed (Characteristic.PlayerCharacteristics[4]);

	}

}
