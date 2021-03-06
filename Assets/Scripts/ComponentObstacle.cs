﻿//Written by Alessandro Vinciguerra <alesvinciguerra@gmail.com>
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
