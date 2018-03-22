using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundObstacle : MonoBehaviour {

	private List<ComponentObstacle> components = new List<ComponentObstacle> ();

	public void addComponent (ComponentObstacle c) {
		components.Add (c);
	}

	public bool componentsPassed () {
		foreach (ComponentObstacle c in components) {
			if (!c.hasBeenPassed ()) {
				return false;
			}
		}
		return true;
	}

	public List<ComponentObstacle> getComponents () {
		return components;
	}

	public bool translate (float dx, float limit) {
		for (int i = 0; i < components.Count; i++) {
			GameObject o = components [i].gameObject;
			Vector2 pos = o.transform.position;
			pos.x += dx;
			if (pos.x < limit) {
				return true;
			}
			o.transform.position = pos;
			i++;
		}
		return false;
	}

	public void despawn() {
		foreach (ComponentObstacle c in components) {
			Destroy (c.gameObject);
		}
	}

}
