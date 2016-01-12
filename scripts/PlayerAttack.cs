using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	//The script attached to a weapon, alow the player to attack
	//public bool canAttack = true;
	public float Speed ; // the speed of the weapon
	public float Dammage; //The dammege of the weapon
	PlayerManager Playermanager; //reference to the Player manager script
	public AudioClip HitSound; //sound when hitting
	public bool Attack =false;




	void Awake()
	{
		Playermanager = GameObject.Find ("Player").GetComponent<PlayerManager>();
		GameObject.Find ("Player").GetComponent<PlayerController> ().speed = Speed; //Set the speed of the animation Attack, in the Player Controller script (the one which controle the animator)



	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


		
	}
	void OnTriggerEnter(Collider col) //When the player attack, the collider of the weapon has to touch an ennemy to apply dammege
	{
		if (col.tag != "Player") { //if the weapon do not touch the player (i.e touch another game obkect than the player)
			if(Attack){
				float SendDammage = (Dammage + Playermanager.Level)+ (Dammage * GameObject.Find("Player").GetComponent<Characteristic> ().PlayerCharacteristics [2] * 2 / 100); 
				//calcultae the dammage by the player, equal to: the dammage of the weapon + the player lever, plus the dammege of the weapon multiplied by 5% of the pkayer strenght
				col.SendMessage ("ApplyDammage", SendDammage, SendMessageOptions.DontRequireReceiver); //apply the dammage to the target
				Attack = false;
		}
	}
}
	public void SetSpeed()
	{	
		GameObject.Find ("Player").GetComponent<PlayerController> ().speed = Speed+GameObject.Find ("Player").GetComponent<Characteristic> ().PlayerCharacteristics [4]/100.0f;
		//Reajust the speed of the weapon, depending on the agility of the player (either if the player add a skill point or an equipement give agility)
		//the basic unit of this speed is 1, and each agility point add 0.01 to the speed.
	}

	









}
