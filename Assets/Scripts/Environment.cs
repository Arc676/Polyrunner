using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

	[SerializeField] private Camera cam;

	[SerializeField] private List<Player> players;
	private int selectedPlayer = 0;

	[SerializeField] private Player playerPrefab;

	[SerializeField] private Selector selector;

	private bool gameOver = false;
	private static bool paused = false;

	void Start() {
		players [0].setEnv (this);
	}

	void resetGame() {
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
			Vector3 pos = Input.mousePosition;
			pos = cam.ScreenToWorldPoint (pos);
			pos.z = 0;
			Player p = (Player)Instantiate (
				playerPrefab,
				pos,
				Quaternion.identity
			);
			p.setEnv (this);
			players.Add (p);
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
		gameOver = true;
		paused = true;
	}

	public static bool isPaused() {
		return paused;
	}

}