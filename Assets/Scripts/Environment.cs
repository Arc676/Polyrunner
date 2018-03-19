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

public class Environment : MonoBehaviour {

	[SerializeField] private Camera cam;
	[SerializeField] private GameObject gameOverSprite;

	[SerializeField] private List<Player> players;
	private int selectedPlayer = 0;

	[SerializeField] private Player playerPrefab;

	[SerializeField] private Selector selector;

	private bool gameOver = false;
	private static bool paused = false;

	void Start() {
		players [0].setEnv (this);
	}

	void spawnPlayerAt(Vector3 pos) {
		pos.z = 0;
		Player player = (Player)Instantiate (playerPrefab, pos, Quaternion.identity);
		player.setEnv (this);
		players.Add (player);
	}

	void resetGame() {
		foreach (Player p in players) {
			Destroy (p.gameObject);
		}
		players.Clear ();

		spawnPlayerAt (Vector2.zero);
		selectedPlayer = 0;

		gameOverSprite.SetActive (false);
		gameOver = false;
		paused = false;
		Time.timeScale = 1;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (gameOver) {
				resetGame ();
			} else {
				paused = !paused;
				Time.timeScale = paused ? 0 : 1;
			}
		}
		if (paused) {
			return;
		}
		if (Input.GetMouseButtonDown (0)) {
			spawnPlayerAt (cam.ScreenToWorldPoint (Input.mousePosition));
		}
		if (Input.GetKeyDown (KeyCode.Tab)) {
			selectNextPlayer ();
		}
		if (Input.GetKeyDown (KeyCode.Space) && players.Count > 1) {
			players [selectedPlayer].detach ();
			players.RemoveAt (selectedPlayer);
			selectNextPlayer ();
		}
	}

	void selectNextPlayer() {
		selectedPlayer++;
		selectedPlayer %= players.Count;
	}

	void LateUpdate() {
		selector.transform.position = players [selectedPlayer].transform.position;
	}

	public void playerDied() {
		gameOverSprite.SetActive (true);
		gameOver = true;
		paused = true;
	}

	public static bool isPaused() {
		return paused;
	}

}