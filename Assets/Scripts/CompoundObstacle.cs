using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundObstacle : MonoBehaviour {

	private List<GameObject> components = new List<GameObject>();

	public void addComponent(ComponentObstacle c) {
		components.Add (c.gameObject);
	}

	public bool componentsPassed() {
		foreach (GameObject o in components) {
			ComponentObstacle c = o.GetComponent <ComponentObstacle> ();
			if (!c.hasBeenPassed ()) {
				return false;
			}
		}
		return true;
	}

	public List<GameObject> getComponents() {
		return components;
	}

}
