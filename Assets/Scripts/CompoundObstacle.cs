using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundObstacle : MonoBehaviour {

	private List<ComponentObstacle> components = new List<ComponentObstacle>();

	public void addComponent(ComponentObstacle c) {
		components.Add (c);
	}

	public bool componentsPassed() {
		foreach (ComponentObstacle c in components) {
			if (!c.hasBeenPassed ()) {
				return false;
			}
		}
		return true;
	}

}
