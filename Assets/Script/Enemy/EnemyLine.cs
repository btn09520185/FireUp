using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyLine : MonoBehaviour {
	public GameObject _enemyPrefab;
	float _lineDownSpeed;
	int _numEnemy;
	bool _isDead = false;

	// Use this for initialization
	void Start () {
		this.transform.localScale = new Vector2 (1, 1);
		this._lineDownSpeed = Screen.height / 6;
		this._numEnemy = 4;
	}

	public void ChangeNumEnemyAction (int num) {
		float scale = (float)this._numEnemy / (float)num;
		DOTween.Sequence ()
			.Append (this.transform.DOScale (scale, 2f));
	}

	public void CreateEmemyLine (int numEnemy) {
		this._numEnemy = numEnemy;
		var canvasRt = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		int enemyWidth = (int)(canvasRt.sizeDelta.x / this._numEnemy);

		for (var i = 0; i < _numEnemy; i++) {
			GameObject enemy = Instantiate (this._enemyPrefab);
			int index = i;

			Enemy enemyComponent = enemy.GetComponent<Enemy> ();
			enemyComponent.InitInfo (Random.Range(10 , 30));
			enemy.transform.SetParent (gameObject.transform);
			enemy.transform.localPosition = new Vector2 ((index * enemyWidth) + enemyWidth / 2 - canvasRt.sizeDelta.x / 2, 0);

			RectTransform enemyRt = enemy.GetComponent<RectTransform> ();
			BoxCollider2D enemyCollider = enemy.GetComponent<BoxCollider2D> ();
			enemyCollider.size = new Vector2 (enemyWidth, enemy.GetComponent<RectTransform> ().sizeDelta.y);
			enemyRt.sizeDelta = new Vector2 (enemyWidth, enemy.GetComponent<RectTransform> ().sizeDelta.y);
		}
	}
	
	public void UpdateGO (float dt) {
		this.transform.localPosition = new Vector2 (this.transform.localPosition.x, this.transform.localPosition.y - this._lineDownSpeed * dt);
		this.SetDead (this.transform.position.y < 0);
	}

	void SetDead(bool isDead) {
		this._isDead = isDead;
	}

	public bool IsDead() {
		return this._isDead;
	}
}
