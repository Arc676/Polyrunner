using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentObstacle : MonoBehaviour {

	private bool passed = false;

	public void pass() {
		passed = true;
	}

	public bool hasBeenPassed() {
		return passed;
	}

}
