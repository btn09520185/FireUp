using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	bool _isDead = false;
	float _speedX = 0;
	float _speedY = 0;
	int _damage = 0;

	// Use this for initialization
	void Start () {
		
	}

	public void InitInfo (float speedX, float speedY, int damage) {
		this._speedX = speedX;
		this._speedY = speedY;
		this._damage = damage;
	}

	public void UpdateGO (float dt) {
		var rt = GetComponent<RectTransform> ();
		rt.position = new Vector2 (rt.position.x + this._speedX, rt.position.y + this._speedY);
	
		var canvasRt = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		SetDead (transform.position.y >= canvasRt.sizeDelta.y);
	}

	public void SetDead(bool isDead) {
		this._isDead = isDead;
	}

	public bool IsDead () {
		return this._isDead;
	}

	public int GetDamage () {
		return this._damage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
