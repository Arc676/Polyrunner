using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	
	private bool isControlled = true;

	private const int playerLayerMask = ~(1 << 8);

	void Update () {
		if (isControlled) {
			Vector2 pos = transform.position;
			float dy = 0;
			int sgn = 0;
			if (Input.GetKey (KeyCode.W)) {
				dy = 0.1f;
				sgn = 1;
			} else if (Input.GetKey (KeyCode.S)) {
				dy = -0.1f;
				sgn = -1;
			}
			if (dy != 0) {
				dy = sgn * 0.1f;
				RaycastHit2D hit = Physics2D.Raycast (pos, sgn * Vector2.up, 0.2f, playerLayerMask);
				if (hit.collider == null) {
					pos.y += dy;
					transform.position = pos;
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (isControlled) {
		} else {
			Destroy (gameObject);
		}
	}

	public void detach() {
		isControlled = false;
	}

}
