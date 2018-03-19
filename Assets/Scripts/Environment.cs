using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

	[SerializeField] private Camera cam;

	[SerializeField] private List<Player> players;
	[SerializeField] private Player playerPrefab;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector3 pos = Input.mousePosition;
			pos.z = 0;
			pos = cam.ScreenToWorldPoint (pos);
			Instantiate (
				playerPrefab,
				pos,
				Quaternion.identity
			);
		}
	}

}