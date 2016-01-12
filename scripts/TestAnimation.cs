using UnityEngine;
using System.Collections;

public class TestAnimation : MonoBehaviour {
	public GameObject test;
	public GameObject thegame;
	// Use this for initialization
	void Start () {
		thegame =(GameObject)Instantiate (test, transform.position, transform.rotation);
		EnnemyScript script1 = thegame.GetComponent<EnnemyScript> ();

		//EnnemyScript script2 = GetComponent<EnnemyScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("k"))
		   Destroy(thegame );
	

	if (thegame == null) {
			thegame =(GameObject)Instantiate (test, transform.position, transform.rotation);

		}

}
		   }