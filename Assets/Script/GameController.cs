using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject _player;
	public GameObject _bulletManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var dt = Time.deltaTime;
		_player.GetComponent<PlayerManager> ().UpdateGO(dt);
	}
}
