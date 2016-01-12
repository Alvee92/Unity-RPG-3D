using UnityEngine;
using System.Collections;

public class ButtonTurn : MonoBehaviour {

	private float h = 0.0f;

	void Update () {
		if ((h>=.1)||(h<=-.1))
		{
			
			transform.Rotate(0,(h*2),0);
		}
	}
}
