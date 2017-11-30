using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum GameState {
	IDLE,
	PLAYING,
	END
};

public class GameController : MonoBehaviour {
	public GameObject _popupStartGame;
	public GameObject _popupEndGame;
	public Button _buttonChangeLevelSpeed;
    public Button _buttonChangeStyleFire;
	public Button _buttonChangeEnemyNumPerLine;
	public PlayerManager _playerManager;
	public GameObject _enemyLinePrefab;

	GameState _gameState;
	EnemyLineManager _enemyManager;
	float CreateEnemyInterval = 3f;
	float CreateEnemyTimeLapse = 0;
	int _currentNumEnemyOfLine = 4;

	// Use this for initialization
	void Start () {
		var canvasRt = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		print ("Canvas Size = " + canvasRt.sizeDelta.x + " , " + canvasRt.sizeDelta.y);
		print ("Screen Size = " + Screen.width + " , " + Screen.height);

		this._gameState = GameState.IDLE;
		this._enemyManager = new EnemyLineManager(this._enemyLinePrefab, this.gameObject);

		var playerStats = this._playerManager.GetPlayerStats();
        UpdateButtonText(this._buttonChangeStyleFire, "Style Fire: " + playerStats.FireType);
        UpdateButtonText(this._buttonChangeLevelSpeed, "Speed Level: " + playerStats.BulletSpeedLevel);
		UpdateButtonText(this._buttonChangeEnemyNumPerLine, "Num Enemy of Line: " + this._currentNumEnemyOfLine);
    }

	public void onButtonChangeLevelSpeed () {
		var playerStats = this._playerManager.GetPlayerStats();
		playerStats.BulletSpeedLevel = (playerStats.BulletSpeedLevel >= playerStats.BulletSpeed.Length - 1) ? 1 : (playerStats.BulletSpeedLevel + 1);
        UpdateButtonText(this._buttonChangeLevelSpeed, "Speed Level: " + playerStats.BulletSpeedLevel);
    }

    public void onButtonChangeStyleShot () {
		this._playerManager.ChangeStyleFire();
		var playerStats = this._playerManager.GetPlayerStats();
        UpdateButtonText(this._buttonChangeStyleFire, "Style Fire: " + playerStats.FireType);
    }

	public void onButtonChangeEnemyNumPerLine () {
		this._currentNumEnemyOfLine++;
		if (this._currentNumEnemyOfLine > 6) {
			this._currentNumEnemyOfLine = 4;
		}
		this.ChangeNumEnemyOfLineAction ();
		this.UpdateButtonText(this._buttonChangeEnemyNumPerLine, "Num Enemy of Line: " + this._currentNumEnemyOfLine);
	}

	void ChangeNumEnemyOfLineAction () {
		this._enemyManager.ChangeNumEnemyAction (this._currentNumEnemyOfLine);
		this._playerManager.ChangeNumEnemyAction (this._currentNumEnemyOfLine);
	}

    void UpdateButtonText (Button go, string str) {
        go.GetComponentInChildren<Text>().text = str;
    }

	public void StartGame () {
		this._playerManager.ActivePlayer(true);
		this._enemyManager.ResetAllEnemy ();

		this._popupEndGame.SetActive (false);
		this._popupStartGame.SetActive (false);
		this._gameState = GameState.PLAYING;
	}

	void EndGame () {
		this._playerManager.BeingDestroy ();
		this._popupEndGame.SetActive (true);
		this._gameState = GameState.END;
	}

    // Update is called once per frame
    void Update () {
		var dt = Time.deltaTime;

		switch (this._gameState) {
		case GameState.IDLE:
			break;

		case GameState.PLAYING:
			{
				if (this._playerManager.IsDead()) {
					this.EndGame ();
					break;
				}

				// spaw enemy line
				this.CreateEnemyTimeLapse += dt;
				if (this.CreateEnemyTimeLapse >= this.CreateEnemyInterval) {
					this._enemyManager.CreateEnemyLine (this._currentNumEnemyOfLine);
					this.CreateEnemyTimeLapse = 0;
				}

				// update player
				this._playerManager.UpdateGO (dt);

				// update enemy line
				this._enemyManager.UpdateGO (dt);
			}
			break;

		case GameState.END:
			break;

		}
	}
}
