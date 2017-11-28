using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour, IDragHandler {
	public Button _buttonChangeLevelSpeed;
    public Button _buttonChangeStyleFire;
	public Button _buttonChangeEnemyNumPerLine;
    public GameObject _player;
	public GameObject _enemyLinePrefab;
	List<GameObject> _listEnemyLine;
	float CreateEnemyInterval = 3f;
	float CreateEnemyTimeLapse = 0;
	int _currentNumEnemyOfLine = 4;

	void EnemyDeadCallback (GameObject enemy) {
//		this._listEnemy.Remove (enemy);
//		print ("EnemyDeadCallback this._listEnemy Count = " + this._listEnemy.Count);
	}

	// Use this for initialization
	void Start () {
		this._listEnemyLine = new List<GameObject>();

		var playerStats = _player.GetComponent<PlayerManager> ().GetPlayerStats();
        UpdateButtonText(this._buttonChangeStyleFire, "Style Fire: " + playerStats.FireType);
        UpdateButtonText(this._buttonChangeLevelSpeed, "Speed Level: " + playerStats.BulletSpeedLevel);
		UpdateButtonText(this._buttonChangeEnemyNumPerLine, "Num Enemy of Line: " + _currentNumEnemyOfLine);
    }

	void CreateEnemyLine(int numEnemy) {
		GameObject enemyLine = Instantiate (_enemyLinePrefab);
		enemyLine.transform.SetParent (transform);
		var canvasRt = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		enemyLine.transform.position = new Vector2 (Screen.width / 2, Screen.height);
		enemyLine.GetComponent <EnemyLine> ().CreateEmemyLine(numEnemy);

		_listEnemyLine.Add (enemyLine);
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

	public void onButtonChangeEnemyNumPerLine () {
		_currentNumEnemyOfLine++;
		if (_currentNumEnemyOfLine > 6) {
			_currentNumEnemyOfLine = 4;
		}
		ChangeNumEnemyOfLineAction ();
		UpdateButtonText(this._buttonChangeEnemyNumPerLine, "Num Enemy of Line: " + _currentNumEnemyOfLine);
	}

	void ChangeNumEnemyOfLineAction () {
		foreach(var lineEnenmy in _listEnemyLine) {
			if (!lineEnenmy) {
				continue;
			}
			lineEnenmy.GetComponent <EnemyLine> ().ChangeNumEnemyAction (_currentNumEnemyOfLine);
		}
		this._player.GetComponent<PlayerManager> ().ChangeNumEnemyAction (_currentNumEnemyOfLine);
	}

    void UpdateButtonText (Button go, string str) {
        go.GetComponentInChildren<Text>().text = str;
    }

    // Update is called once per frame
    void Update () {
		var dt = Time.deltaTime;

		// spaw enemy line
		CreateEnemyTimeLapse += dt;
		if (CreateEnemyTimeLapse >= CreateEnemyInterval) {
			CreateEnemyLine (_currentNumEnemyOfLine);
			CreateEnemyTimeLapse = 0;
		}

		// update player
		this._player.GetComponent<PlayerManager> ().UpdateGO (dt);


		// update enemy line
		List<GameObject> listDeadedEnemyLine = new List<GameObject>();
		foreach(var lineEnenmy in _listEnemyLine) {
			if (!lineEnenmy) {
				continue;
			}
			lineEnenmy.GetComponent <EnemyLine> ().UpdateGO (dt);
			if (lineEnenmy.GetComponent <EnemyLine> ().IsDead ()) {
				GameObject.Destroy (lineEnenmy);
				listDeadedEnemyLine.Add (lineEnenmy);
			}
		}
		foreach (var lineEnenmy in listDeadedEnemyLine) {
			_listEnemyLine.Remove (lineEnenmy);
		}

	}

	public void OnDrag (PointerEventData eventData) {
		this._player.GetComponent<PlayerManager> ().Move(eventData.delta.x);
	}
}
