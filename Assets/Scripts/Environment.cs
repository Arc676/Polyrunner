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

	// sound
	private AudioSource music;
	public static bool soundEnabled = true;

	[SerializeField] private Player playerPrefab;
	[SerializeField] private List<Player> players;
	private int selectedPlayer = 0;

	[SerializeField] private GameObject stdObstaclePrefab;
	private List<GameObject> obstacles = new List<GameObject> ();
	private float timeSinceObstacleSpawn = 0;

	[SerializeField] private GameObject coinPrefab;
	private List<GameObject> coins = new List<GameObject> ();

	[SerializeField] private ComponentObstacle componentPrefab;
	private CompoundObstacle currentCompound = null;
	private float timeSinceCompoundSpawn = 0;

	private ContactFilter2D contactFilter;
	private Collider2D[] res = {null, null};

	[SerializeField] private Text componentCountLabel;

	[SerializeField] private Selector selector;

	[SerializeField] private Text scoreText;
	[SerializeField] private Text hiscoreLabel;
	private int hiScore = 0;
	private int score = 0;
	private bool gameOver = false;
	private static bool paused = false;

	void Start () {
		if (PlayerPrefs.HasKey("HiScore")) {
			updateHiScore(PlayerPrefs.GetInt("HiScore"), false);
		}
		players [0].setEnv (this);

		contactFilter = new ContactFilter2D ();
		contactFilter.useTriggers = true;

		music = GetComponent <AudioSource> ();
	}

	void updateHiScore(int score, bool save) {
		hiScore = score;
		hiscoreLabel.text = "High Score: " + hiScore;
		if (save) {
			PlayerPrefs.SetInt("HiScore", hiScore);
			PlayerPrefs.Save();
		}
	}

	void spawnPlayerAt (Vector3 pos) {
		pos.z = -1;
		Player player = (Player)Instantiate (playerPrefab, pos, Quaternion.identity);
		player.setEnv (this);
		players.Add (player);
	}

	void spawnObstacle () {
		Vector3 scale = Vector3.one;
		scale.x = Random.Range (1, 10);

		float xpos = 9 + scale.x / 2;
		Vector3 pos = new Vector3 (xpos, Random.Range (-4, 4), 0);
		GameObject o = (GameObject)Instantiate (stdObstaclePrefab, pos, Quaternion.identity);
		o.transform.localScale = scale;

		if (currentCompound != null) {
			if (o.GetComponent <Collider2D> ().OverlapCollider (contactFilter, res) > 0) {
				Destroy (o);
				return;
			}
		}

		bool dropMode = pos.y > 0 && scale.x > 2 && Random.Range (0, 100) % 2 == 0;
		int max = (int)(dropMode ? pos.y + 3 : scale.x / 2);
		for (int i = 0; i < max; i++) {
			Vector3 coinPos;
			if (dropMode) {
				coinPos = new Vector3 (xpos + i, pos.y - 1 - i, 0);
			} else {
				coinPos = new Vector3 (xpos + i, pos.y + 1, 0);
			}
			GameObject coin = (GameObject)Instantiate (coinPrefab, coinPos, Quaternion.identity);
			coins.Add (coin);
		}

		obstacles.Add (o);
	}

	void spawnCompound() {
		currentCompound = new CompoundObstacle ();
		int count = Random.Range (2, 5);
		for (int i = 0; i < count; i++) {
			Vector3 pos = new Vector3 (
				9 + Random.Range (0, 6),
				Random.Range (-4, 4),
				-2
			);
			bool posOK = true;
			ComponentObstacle c = (ComponentObstacle)Instantiate (componentPrefab, pos, Quaternion.identity);
			if (c.GetComponent <Collider2D> ().OverlapCollider (contactFilter, res) > 0) {
				Destroy (c.gameObject);
				posOK = false;
			}
			if (posOK) {
				currentCompound.addComponent (c);
			}
		}
		if (currentCompound.getComponentCount () == 0) {
			currentCompound = null;
		} else {
			componentCountLabel.text = currentCompound.getComponentCount () + " components";
		}
	}

	void resetGame () {
		foreach (Player p in players) {
			Destroy (p.gameObject);
		}
		players.Clear ();

		foreach (GameObject o in obstacles) {
			Destroy (o);
		}
		obstacles.Clear ();

		destroyCompound ();

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

	void destroyCompound() {
		if (currentCompound != null) {
			currentCompound.despawn ();
			currentCompound = null;
			componentCountLabel.text = "No components";
		}
	}

	void translateObjectsInList (List<GameObject> list, float dx, float limit) {
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
		// toggle music
		if (Input.GetKeyDown (KeyCode.M)) {
			if (music.isPlaying) {
				music.Pause ();
			} else {
				music.UnPause ();
			}
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			soundEnabled = !soundEnabled;
		}
		if (paused) {
			return;
		}

		// obstacle spawning
		if (timeSinceObstacleSpawn < 2) {
			timeSinceObstacleSpawn += Time.deltaTime;
		} else {
			timeSinceObstacleSpawn = 0;
			spawnObstacle ();
		}

		// spawn compounds
		if (currentCompound == null) {
			if (timeSinceCompoundSpawn < 5) {
				timeSinceCompoundSpawn += Time.deltaTime;
			} else {
				timeSinceCompoundSpawn = 0;
				spawnCompound ();
			}
		}

		// moving game components
		float dx = -Time.deltaTime * 4;
		translateObjectsInList (obstacles, dx, -15);
		translateObjectsInList (coins, dx, -10);
		if (currentCompound != null) {
			if (currentCompound.translate (dx, -10)) {
				if (currentCompound.componentsPassed ()) {
					changeScore (200);
					destroyCompound ();
				} else {
					playerDied ();
				}
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

	void selectNextPlayer () {
		selectedPlayer++;
		selectedPlayer %= players.Count;
	}

	void LateUpdate () {
		selector.transform.position = players [selectedPlayer].transform.position;
	}

	public void playerDied () {
		if (score > hiScore) {
			updateHiScore(score, true);
		}
		gameOverSprite.SetActive (true);
		gameOver = true;
		paused = true;
	}

	public static bool isPaused () {
		return paused;
	}

	public void changeScore (int delta) {
		score += delta;
		scoreText.text = "Score: " + score;
	}

	public void destroyCoin(GameObject coin) {
		coins.Remove (coin);
		Destroy (coin);
	}

}