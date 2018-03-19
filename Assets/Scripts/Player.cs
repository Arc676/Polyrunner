using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	
	private bool isControlled = true;

	void Update () {
		if (isControlled) {
			Vector2 pos = transform.position;
			float dy = Input.GetAxis ("Vertical");
			if (dy != 0) {
				int sgn = (int)Mathf.Sign (dy);
				dy = sgn * 0.1f;
				RaycastHit2D hit = Physics2D.Raycast (pos, sgn * Vector2.up, 0.2f, ~(1 << 8));
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
