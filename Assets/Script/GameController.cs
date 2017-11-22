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

	// Use this for initialization
	void Start () {
		this._listEnemy = new List<GameObject>();
		for (var i = 0; i < 5; i++) {
			GameObject enemy = Instantiate (_enemyPrefab);

			enemy.transform.position = new Vector2 (Random.Range(0 , 640), Random.Range(600 , 960));
			Enemy enemyComponent = enemy.GetComponent<Enemy> ();
			enemyComponent.InitInfo (Random.Range(100 , 1000));
			enemy.transform.SetParent (gameObject.transform);

			this._listEnemy.Add (enemy);
		}

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

		List<GameObject> listEnemyDead = new List<GameObject>();
		foreach (var enemy in this._listEnemy) {
			if (this._player.GetComponent<PlayerManager> ().CheckCollisionWithEnemy (enemy)) {
				listEnemyDead.Add (enemy);
			}
		}
	}
}
