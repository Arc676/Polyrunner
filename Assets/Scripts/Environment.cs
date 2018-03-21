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
using UnityEngine.UI;

public class Environment : MonoBehaviour {

	[SerializeField] private Camera cam;
	[SerializeField] private GameObject gameOverSprite;

	[SerializeField] private Player playerPrefab;
	[SerializeField] private List<Player> players;
	private int selectedPlayer = 0;

	[SerializeField] private GameObject stdObstaclePrefab;
	private List<GameObject> obstacles = new List<GameObject> ();
	private float timeSinceObstacleSpawn = 0;

	[SerializeField] private GameObject coinPrefab;
	private List<GameObject> coins = new List<GameObject> ();

	[SerializeField] private Selector selector;

	[SerializeField] private Text scoreText;
	private int score = 0;
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

		for (int i = 0; i < scale.x / 2; i++) {
			Vector3 coinPos = new Vector3 (9 + scale.x / 2 + i, pos.y + 1, 0);
			GameObject coin = (GameObject)Instantiate (coinPrefab, coinPos, Quaternion.identity);
			coins.Add (coin);
		}

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

		foreach (GameObject o in coins) {
			Destroy (o);
		}
		coins.Clear ();

		spawnPlayerAt (Vector2.zero);
		selectedPlayer = 0;

		gameOverSprite.SetActive (false);
		gameOver = false;
		paused = false;
		Time.timeScale = 1;
		score = 0;
		scoreText.text = "Score: 0";
	}

	void translateObjectsInList(List<GameObject> list, float dx, float limit) {
		for (int i = 0; i < list.Count;) {
			GameObject o = list [i];
			Vector2 pos = o.transform.position;
			pos.x += dx;
			if (pos.x < limit) {
				Destroy (o);
				list.RemoveAt (i);
			} else {
				o.transform.position = pos;
				i++;
			}
		}
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
		float dx = -Time.deltaTime * 5;
		translateObjectsInList (obstacles, dx, -15);
		translateObjectsInList (coins, dx, -15);

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

	public void changeScore(int delta, GameObject coin) {
		score += delta;
		scoreText.text = "Score: " + score;
		coins.Remove (coin);
		Destroy (coin);
	}

}