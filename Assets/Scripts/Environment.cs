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

	[SerializeField] private Player playerPrefab;
	[SerializeField] private List<Player> players;
	private int selectedPlayer = 0;

	[SerializeField] private GameObject stdObstaclePrefab;
	private List<GameObject> obstacles = new List<GameObject> ();
	private float timeSinceObstacleSpawn = 0;

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

	void spawnObstacle() {
		Vector3 scale = Vector3.one;
		scale.x = Random.Range (1, 10);

		Vector3 pos = new Vector3 (9 + scale.x / 2, Random.Range (-4, 4), 0);
		GameObject o = (GameObject)Instantiate (stdObstaclePrefab, pos, Quaternion.identity);

		o.transform.localScale = scale;

		obstacles.Add (o);
	}

	void resetGame() {
		foreach (Player p in players) {
			Destroy (p.gameObject);
		}
		players.Clear ();

		foreach (GameObject o in obstacles) {
			Destroy (o);
		}
		obstacles.Clear ();

		spawnPlayerAt (Vector2.zero);
		selectedPlayer = 0;

		gameOverSprite.SetActive (false);
		gameOver = false;
		paused = false;
		Time.timeScale = 1;
	}

	void Update () {
		// pause control
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

		// obstacle spawning and moving
		if (timeSinceObstacleSpawn < 2) {
			timeSinceObstacleSpawn += Time.deltaTime;
		} else {
			timeSinceObstacleSpawn = 0;
			spawnObstacle ();
		}
		for (int i = 0; i < obstacles.Count; ) {
			GameObject o = obstacles [i];
			Vector2 pos = o.transform.position;
			pos.x -= Time.deltaTime * 5;
			if (pos.x < -15) {
				Destroy (o);
				obstacles.RemoveAt (i);
			} else {
				o.transform.position = pos;
				i++;
			}
		}

		// player spawning, selecting, and detaching
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