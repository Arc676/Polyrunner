using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private List<Player> clones = new List<Player>();

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		Vector2 pos = transform.position;
		float dy = Input.GetAxis ("Vertical");
		if (dy > 0) {
			pos.y += 0.1f;
		} else if (dy < 0) {
			pos.y -= 0.1f;
		}
		transform.position = pos;
	}
}
