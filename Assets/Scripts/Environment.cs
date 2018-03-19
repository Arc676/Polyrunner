using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

	[SerializeField] private Camera cam;

	[SerializeField] private List<Player> players;
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
	}

	public void playerDespawned(Player p) {
		players.Remove (p);
		Destroy (p.gameObject);
	}

}