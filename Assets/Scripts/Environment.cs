using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

	[SerializeField] private Camera cam;

	[SerializeField] private List<Player> players;
	private int selectedPlayer = 0;

	[SerializeField] private Player playerPrefab;

	[SerializeField] private Selector selector;

	void Start() {
		players [0].setEnv (this);
	}

	void Update () {
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

	public void untrackPlayer(Player p) {
		players.Remove (p);
	}

}