using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {


	//int MaxHealth ;
	private int Health; //current health
	private int Gold = 100; //current gold
	public Text textgold; //print the gold value
	public Text texthealth; //print the health value
	private float Xp = 0; //current xp
	int MaxXp = 100; // Xp to up level
	public int Level = 1; //current level
	public  bool IsPaused ; //if this script pause the game

	private Characteristic PlayerCharacteristic; //reference to the caracteristics

	public AudioClip Hurt1; //the sounds of hurt
	public AudioClip Hurt2;
	public AudioClip Hurt3;
	public AudioClip Hurt4;
	public AudioClip Hurt5;
	int ChoseHurt = 1;

	// Use this for initialization
	void Awake () {
		 
		Xp = 0;
		Level = 1;
		MaxXp = 100;
		PlayerCharacteristic = GetComponent<Characteristic> ();
		//MaxHealth = PlayerCharacteristic.MaxHealth;
		Health = PlayerCharacteristic.MaxHealth; //set the full life at the beginning
	}

	public void ApplyDammage(int damage)
	{
		damage = damage - (PlayerCharacteristic.PlayerCharacteristics [3] + Level) / 3; //the formula to apply damage
		//the dammage of the ennemy minus the defense of the player + the level / 3
		if (damage > 0) { //if the dammages are not reduced entirely by the defense
			Health -= damage ;
			PlayHurt ();
		}
		if (Health <= 0) {

			Dead ();
		}

	}
	void Dead()
	{
		//Application.LoadLevel (Application.loadedLevel);
	}

	
	// Update is called once per frame
	void Update () {

		if (IsPaused) {
			PauseGame ();
		} else {
			UnPauseGame ();
		}
		texthealth.text = Health.ToString () + "/" + PlayerCharacteristic.MaxHealth.ToString ();
		textgold.text = Gold.ToString () ;



		if(Input.GetKeyDown("p"))
		   {

			IsPaused = !IsPaused;
		}

			
		}
	void CheckLevel()
	{
		if (Xp >= MaxXp) {
			Xp -= MaxXp;
			MaxXp += Level * 100; 
			PlayerCharacteristic.MaxHealth += Level * 5; //add health to the player
			PlayerCharacteristic.PlayerCharacteristics [0] = PlayerCharacteristic.MaxHealth; //set full life
			Health = PlayerCharacteristic.MaxHealth;
			Level++;
			PlayerCharacteristic.AddSkillPoint();

		}
	}


	void PlayHurt()
	{
		if(ChoseHurt == 1)
		{
			GetComponent<AudioSource>().PlayOneShot(Hurt1);
			ChoseHurt++;
		}
		else if(ChoseHurt == 2)
		{
			GetComponent<AudioSource>().PlayOneShot(Hurt2);
			ChoseHurt++;
		}
		else if(ChoseHurt == 3)
		{
			GetComponent<AudioSource>().PlayOneShot(Hurt3);
			ChoseHurt++;
		}
		else if(ChoseHurt == 4)
		{
			GetComponent<AudioSource>().PlayOneShot(Hurt4);
			ChoseHurt++;
		}
		else 
		{
			GetComponent<AudioSource>().PlayOneShot(Hurt3);
			ChoseHurt = 1;
		}
	}

	public void AddGold(int value)
	{
		Gold += value;
	}

	public bool BuyItem(Item item)
	{
		if (Gold >= item.price) {
			Gold -= item.price;
			return true;
		} else {
			return false;
		}
	}
	public void SellItem(Item item)
	{

			Gold += item.price/2;
	}
	public void AddXp(int value)
	{
		Xp += value +(value * PlayerCharacteristic.PlayerCharacteristics[5]/100.0f);
		CheckLevel ();
	}
	public void AddHealth(int ItemHealth)
	{
		if(ItemHealth < PlayerCharacteristic.MaxHealth - Health)
		{
			Health+=ItemHealth;
		}
		else {Health = PlayerCharacteristic.MaxHealth;}
	}


	
	public void PauseGame() 
	{
		Time.timeScale = 0.0f;
		Screen.lockCursor = false;
		IsPaused = true;
	}
	public void UnPauseGame() 
	{
		Time.timeScale = 1.0f;
		Screen.lockCursor = true;
		IsPaused = false;
	}

	public void UpdateManager()
	{
		//PlayerCharacteristic.MaxHealth = PlayerCharacteristic.MaxHealth;
	}
}
