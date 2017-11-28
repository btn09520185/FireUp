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
		transform.localScale = new Vector2 (1, 1);
		_lineDownSpeed = Screen.height / 6;
		_numEnemy = 4;
//		ScaleWithNewEnemyNum (5);
	}

	public void ChangeNumEnemyAction (int num) {
		float scale = (float)_numEnemy / (float)num;
		print ("_numEnemy = " + _numEnemy);
		print ("num = " + num);
		print ("ScaleWithNewEnemyNum = " + scale);
		DOTween.Sequence ()
			.Append (transform.DOScale (scale, 2f));
	}

	public void CreateEmemyLine (int numEnemy) {
		_numEnemy = numEnemy;
		var canvasRt = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		int enemyWidth = (int)(canvasRt.sizeDelta.x / _numEnemy);
//		GetComponent<RectTransform>().sizeDelta = new Vector2 (canvasRt.sizeDelta.x, 100);

		for (var i = 0; i < _numEnemy; i++) {
			GameObject enemy = Instantiate (_enemyPrefab);
			int index = i;

			Enemy enemyComponent = enemy.GetComponent<Enemy> ();
			enemyComponent.InitInfo (Random.Range(10 , 30));
//			enemyComponent.DeadCallBack += EnemyDeadCallback;
			enemy.transform.SetParent (gameObject.transform);
			enemy.transform.localPosition = new Vector2 ((index * enemyWidth) + enemyWidth / 2 - canvasRt.sizeDelta.x / 2, 0);
			enemy.GetComponent<RectTransform> ().sizeDelta = new Vector2 (enemyWidth, enemy.GetComponent<RectTransform> ().sizeDelta.y);
		}
	}
	
	public void UpdateGO (float dt) {
		transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y - _lineDownSpeed * dt);

		var canvasRt = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		SetDead (transform.position.y < 0);
	}

	void SetDead(bool isDead) {
		_isDead = isDead;
	}

	public bool IsDead() {
		return _isDead;
	}
}
