//Written by Alessandro Vinciguerra <alesvinciguerra@gmail.com>
//Copyright (C) 2018  Arc676/Alessandro Vinciguerra

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation (version 3).

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
//See README.md and LICENSE for more details

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] private Rigidbody2D rb;
	private Collision2D currentCollision = null;
	
	private bool isControlled = true;
	private Environment env;

	private const int playerLayerMask = ~(1 << 8);

	void Update () {
		if (Environment.isPaused ()) {
			return;
		}
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
		currentCollision = collision;
		if (isControlled) {
			if (!collision.gameObject.CompareTag ("MapBounds")) {
				env.playerDied ();
			}
		} else {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.CompareTag ("Coin")) {
			env.changeScore (isControlled ? 5 : 10, collider.gameObject);
			return;
		} else if (collider.gameObject.CompareTag ("Component")) {
			ComponentObstacle c = collider.gameObject.GetComponent<ComponentObstacle> ();
			c.pass ();
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		currentCollision = null;
	}

	public void detach() {
		isControlled = false;
		rb.gravityScale = 1f;
		if (currentCollision != null) {
			OnCollisionEnter2D (currentCollision);
		}
	}

	public bool isUnderControl() {
		return isControlled;
	}

	public void setEnv(Environment env) {
		this.env = env;
	}

}
