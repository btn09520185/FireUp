using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineManager {
	GameObject _gameContainer;
	List<GameObject> _listEnemyLine;
	GameObject _enemyLinePrefab;

	public EnemyLineManager(GameObject enemyLinePrefab, GameObject gameContainer) {
		this._enemyLinePrefab = enemyLinePrefab;
		this._gameContainer = gameContainer;

		// init 
		this._listEnemyLine = new List<GameObject> ();
	}

	public void CreateEnemyLine(int numEnemy) {
		GameObject enemyLine = GameObject.Instantiate (this._enemyLinePrefab);
		enemyLine.transform.SetParent (this._gameContainer.transform);
		enemyLine.transform.position = new Vector2 (Screen.width / 2, Screen.height);
		enemyLine.GetComponent <EnemyLine> ().CreateEmemyLine(numEnemy);

		this._listEnemyLine.Add (enemyLine);
	}

	public void ChangeNumEnemyAction (int currentNumEnemyOfLine) {
		foreach(var lineEnenmy in this._listEnemyLine) {
			if (!lineEnenmy) {
				continue;
			}
			lineEnenmy.GetComponent <EnemyLine> ().ChangeNumEnemyAction (currentNumEnemyOfLine);
		}
	}

	public void ResetAllEnemy () {
		foreach (var lineEnenmy in this._listEnemyLine) {
			GameObject.Destroy (lineEnenmy);
		}
		this._listEnemyLine.Clear();
	}

	public void UpdateGO (float dt) {
		List<GameObject> listDeadedEnemyLine = new List<GameObject>();
		foreach(var lineEnenmy in this._listEnemyLine) {
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
			this._listEnemyLine.Remove (lineEnenmy);
		}
	}

}
