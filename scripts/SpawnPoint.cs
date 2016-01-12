using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	public GameObject Ennemy; //the ennemy to spawn
	private GameObject SpawnedEnnemy; //the current ennemy in the scene
	private EnnemyScript SpawnedEnnemyScript;

	public int EnnemyLevel;
	public int EnnemyStrength;
	public int EnnemyDefense;
	public int EnnemyXp;
	public int RespawnTime = 60;

	public GameObject[] DropContent;


	// Use this for initialization
	void Start () {
		Spawn ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	void Spawn()
	{
		SpawnedEnnemy = (GameObject)Instantiate (Ennemy, transform.position, transform.rotation);
		SpawnedEnnemyScript = SpawnedEnnemy.GetComponent<EnnemyScript> ();
		SpawnedEnnemyScript.Level = EnnemyLevel + Random.Range (1, 2);
		SpawnedEnnemyScript.Strength = EnnemyStrength + Random.Range (1, 50);
		SpawnedEnnemyScript.Defense = EnnemyDefense + Random.Range (1, 10);
		SpawnedEnnemyScript.GiveXP = EnnemyXp + Random.Range (1, 10);
		SpawnedEnnemyScript.SpawnPoint = gameObject;
		//SpawnedEnnemy.transform.FindChild ("Contents") = GetComponent<EnnemyDrop> ().Contents;
		if (DropContent.Length > 0) {
			foreach (GameObject item in DropContent) {
				GameObject InstanciatedItem = Instantiate (item);
				InstanciatedItem.transform.parent = SpawnedEnnemy.transform.FindChild ("Contents");
			}
			SpawnedEnnemy.GetComponent<Chest> ().UpdateContent ();
		}
	}
	IEnumerator WaitRespawn()
	{
		yield return new WaitForSeconds (RespawnTime);
		Spawn ();
	}
}
