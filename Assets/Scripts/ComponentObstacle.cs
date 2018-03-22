using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentObstacle : MonoBehaviour {
	
	[SerializeField] private Sprite green;

	private bool passed = false;

	public void pass() {
		GetComponent <SpriteRenderer> ().sprite = green;
		passed = true;
	}

	public bool hasBeenPassed() {
		return passed;
	}

	public void despawn() {
		Destroy (gameObject);
	}

}
