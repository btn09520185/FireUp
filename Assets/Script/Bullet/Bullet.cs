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
		transform.localScale = new Vector2 (1, 1);
	}

	public void InitInfo (float speedX, float speedY, int damage) {
		this._speedX = speedX;
		this._speedY = speedY;
		this._damage = damage;
	}

	void OnCollisionEnter2D(Collision2D collision){
		print ("Bullet OnCollisionEnter2D");
		if (collision.gameObject.tag == "Enemy") {
			var enemy = collision.gameObject.GetComponent<Enemy> ();
			if (!enemy) {
				print ("Enemy null");
				return;
			}
			enemy.BeingShot (this.GetDamage());
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D(Collider2D collider){
		print ("Bullet OnTriggerEnter2D");
		var enemy = collider.gameObject.GetComponent<Enemy> ();
		if (!enemy) {
			print ("Bullet enemy null");
			return;
		}
		enemy.BeingShot (this.GetDamage());
		SetDead (true);
		gameObject.SetActive(false);
	}

	public void UpdateGO (float dt) {
		transform.localPosition = new Vector2 (transform.localPosition.x + this._speedX, transform.localPosition.y + this._speedY);
	
		var canvasRt = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
		SetDead (transform.position.y >= Screen.height);
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
