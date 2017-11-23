using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public Button _buttonChangeLevelSpeed;
    public Button _buttonChangeStyleFire;
    public GameObject _player;
	public GameObject _enemyPrefab;
	List<GameObject> _listEnemy;
	float CreateEnemyInterval = 0.4f;
	float CreateEnemyTimeLapse = 0;

	void EnemyDeadCallback (GameObject enemy) {
		this._listEnemy.Remove (enemy);
		print ("EnemyDeadCallback this._listEnemy Count = " + this._listEnemy.Count);
	}

	// Use this for initialization
	void Start () {

		// cheat test 5 enemy
//		this._listEnemy = new List<GameObject>();
//		for (var i = 0; i < 5; i++) {
//			GameObject enemy = Instantiate (_enemyPrefab);
//
//			enemy.transform.position = new Vector2 (Random.Range(0 , 640), Random.Range(600 , 960));
//			Enemy enemyComponent = enemy.GetComponent<Enemy> ();
//			enemyComponent.InitInfo (Random.Range(10 , 30));
//			enemyComponent.DeadCallBack += EnemyDeadCallback;
//			enemy.transform.SetParent (gameObject.transform);
//
//			this._listEnemy.Add (enemy);
//		}

		var playerStats = _player.GetComponent<PlayerManager> ().GetPlayerStats();
        UpdateButtonText(this._buttonChangeStyleFire, "Style Fire: " + playerStats.FireType);
        UpdateButtonText(this._buttonChangeLevelSpeed, "Speed Level: " + playerStats.BulletSpeedLevel);
    }

	public void onButtonChangeLevelSpeed () {
		var playerStats = _player.GetComponent<PlayerManager> ().GetPlayerStats();
		playerStats.BulletSpeedLevel = (playerStats.BulletSpeedLevel >= playerStats.BulletSpeed.Length - 1) ? 1 : (playerStats.BulletSpeedLevel + 1);
        UpdateButtonText(this._buttonChangeLevelSpeed, "Speed Level: " + playerStats.BulletSpeedLevel);
    }

    public void onButtonChangeStyleShot () {
        _player.GetComponent<PlayerManager>().ChangeStyleFire();
        var playerStats = _player.GetComponent<PlayerManager>().GetPlayerStats();
        UpdateButtonText(this._buttonChangeStyleFire, "Style Fire: " + playerStats.FireType);
    }

    void UpdateButtonText (Button go, string str) {
        go.GetComponentInChildren<Text>().text = str;
    }

    // Update is called once per frame
    void Update () {
		var dt = Time.deltaTime;
		this._player.GetComponent<PlayerManager> ().UpdateGO(dt);

		CreateEnemyTimeLapse += dt;
		if (CreateEnemyTimeLapse >= CreateEnemyInterval) {
			GameObject enemy = Instantiate (_enemyPrefab);
			enemy.transform.position = new Vector2 (Random.Range(0 , 640), Random.Range(600 , 960));
			Enemy enemyComponent = enemy.GetComponent<Enemy> ();
			enemyComponent.InitInfo (Random.Range(5 , 10));
			//		enemyComponent.DeadCallBack += EnemyDeadCallback;
			enemy.transform.SetParent (gameObject.transform);

			CreateEnemyTimeLapse = 0;
		}

	}

//	void FixedUpdate () {
//		var dt = Time.deltaTime;
//		this._player.GetComponent<PlayerManager> ().UpdateGO(dt);
//	}
}
