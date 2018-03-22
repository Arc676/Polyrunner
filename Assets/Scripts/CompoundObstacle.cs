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

public class CompoundObstacle {

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

	public int getComponentCount() {
		return components.Count;
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
		}
		return false;
	}

	public void despawn() {
		foreach (ComponentObstacle c in components) {
			c.despawn ();
		}
	}

}
