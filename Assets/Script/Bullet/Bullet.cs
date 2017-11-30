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
		this.transform.localScale = new Vector2 (1, 1);
	}

	public void InitInfo (Vector2 speed, int damage) {
		this._speedX = speed.x * 60;
		this._speedY = speed.y * 60;
		this._damage = damage;
	}

	public void UpdateGO (float dt) {
		this.transform.localPosition = new Vector2 (this.transform.localPosition.x + this._speedX * dt, this.transform.localPosition.y + this._speedY * dt);
		this.SetDead (this.transform.position.y >= Screen.height);
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
