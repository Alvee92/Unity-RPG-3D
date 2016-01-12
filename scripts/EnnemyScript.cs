using UnityEngine;
using System.Collections;

public class EnnemyScript : MonoBehaviour {


	float Distance;
	Animation anim ;
	bool isDead = false;
	public int Level = 1;
	public int Health = 100;
	public int Strength;
	public int Defense;
	Transform Target;
	public float LookAtDistance = 25.5f; //Minimum distance to detect and look at the player
	public float ChaseRange = 15.0f;    //Minimum distance to chase the player
	public float AttackRange = 1.5f;	//Minimum distance to attack the player
	public float MoveSpeed = 5.0f;		//speed
	public float Damping = 6.0f;		
	public float AttackRepeatTime = 2;	//Delay between two attacks
	public int Dammage = 10;		
	public int GiveXP;	//The xp given to the player when die
	public AudioClip Die;	//sound of death
	public AudioClip Hit;
	PlayerManager playermanager;	//reference to the player
	PlayerAttack playerattack;		//reference to the script of the weapon
	QuestManager questmanager;     //reference to the quest manager of the player
	

	private float attackTime; //the time when attacking
	public CharacterController Controller;	//the player

	public float gravity = 20.0f;
	private Vector3 MoveDirection = Vector3.zero;

	public GameObject SpawnPoint;


	// Use this for initialization
	void Start () {
		Strength =Strength+ 20*(Level + Random.Range(1,10));
		Defense = Defense + Level * 2;
		Dammage = Dammage + Level + Random.Range (0, 3);
		Health = 100 + Level * (5 * Level / 2); //Set the health in function of the level
		attackTime = Time.time;
		anim = GetComponent<Animation> ();
		Target = GameObject.Find ("Player").transform;
		playermanager = Target.GetComponent <PlayerManager>();
		questmanager = Target.transform.FindChild("Quest Manager").GetComponent<QuestManager> ();


		

	}


	// Update is called once per frame
	void Update () {
		Distance = Vector3.Distance(Target.position, transform.position); //the distance between the player and the ennemy
		if (!isDead) {   //if the ennemy isn't dead
			if (Distance < LookAtDistance) { //and at a suffisent distance to look
				LookAt ();
			}

		
			if (Distance < AttackRange) { //if it's close enough to attack
				Attack ();
			} else if (Distance < ChaseRange) { //else chase
				Chase ();
			}
		}

	}

	void LookAt ()
	{
		Quaternion rotation = Quaternion.LookRotation(Target.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);	//rotate in direction of the player
	}

	void Chase ()
	{

		anim.Play("walk");
		
		MoveDirection = transform.forward; //move forward
		MoveDirection *= MoveSpeed;
		
		MoveDirection.y -= gravity * Time.deltaTime;
		Controller.Move(MoveDirection * Time.deltaTime);
	}


	void Attack ()
	{
		if (Time.time > attackTime)
		{
			anim.Play("attack");
			GetComponent<AudioSource>().PlayOneShot(Hit);
			//int SendDammage;
			//SendDammage = (Dammage+Level) + (Dammage*Strength * 2 /100);
			playermanager.ApplyDammage(Dammage);
			attackTime = Time.time + AttackRepeatTime;
		}
	}


	void ApplyDammage (int Dammage)
	{

		if (!isDead) {
			Health -= Dammage - (Defense + Level) / 3;
			//GetComponent<Animation> ().Play ("gethit");

			if (Health <= 0) {
				//GameObject.Find(EnemyName).GetComponent(EnnemyScript).
				isDead = true;
				GetComponent<Animation> ().Play ("die");
				GetComponent<AudioSource> ().PlayOneShot (Die);
				playermanager.AddXp(GiveXP);


				Dead ();
				StartCoroutine (Wait30Seconds ());




			}
		}
	}
	void Dead()
	{

		questmanager.CheckQuest (tag);
		GetComponent<Chest> ().enabled = true;
		SpawnPoint.SendMessage ("WaitRespawn", SendMessageOptions.DontRequireReceiver);


	}



	IEnumerator Wait30Seconds(){
		yield return new WaitForSeconds (30);
		Destroy (gameObject);
	}



	

}
