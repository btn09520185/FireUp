using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public Button _buttonChangeLevelSpeed;
	public GameObject _player;
	public GameObject _bulletManager;

	// Use this for initialization
	void Start () {
//		var playerStats = _player.GetComponent<PlayerManager> ()._playerStats;
//		_buttonChangeLevelSpeed.GetComponentInChildren<Text>().text = "Speed Level " + playerStats.BulletSpeedLevel;
	}

	public void onButtonChangeLevelSpeed () {
		var playerStats = _player.GetComponent<PlayerManager> ()._playerStats;
		playerStats.BulletSpeedLevel = (playerStats.BulletSpeedLevel >= playerStats.BulletSpeed.Length - 1) ? 1 : (playerStats.BulletSpeedLevel + 1);
		_buttonChangeLevelSpeed.GetComponentInChildren<Text>().text = "Speed Level " + playerStats.BulletSpeedLevel;
	}
	
	// Update is called once per frame
	void Update () {
		var dt = Time.deltaTime;
		_player.GetComponent<PlayerManager> ().UpdateGO(dt);
	}
}
