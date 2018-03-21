using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentObstacle : MonoBehaviour {
	
	[SerializeField] private Sprite green;

	private bool passed = false;

	public void pass() {
		passed = true;
	}

	public bool hasBeenPassed() {
		GetComponent <SpriteRenderer> ().sprite = green;
		return passed;
	}

}
