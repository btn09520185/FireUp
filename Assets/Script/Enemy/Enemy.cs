using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Enemy : MonoBehaviour {
	Text _hpText;
	int _hp;
	bool _isDead;

	// Use this for initialization
	void Start () {
		this.transform.localScale = new Vector2 (1, 1);
		this._hpText = transform.Find ("Text").gameObject.GetComponent<Text>();
		this.UpdateHpText ();
	}

	public void InitInfo (int hp) {
		this._hp = hp;
	}

	void OnTriggerEnter2D(Collider2D collider){
		print ("Bullet OnTriggerEnter2D");
		switch (collider.gameObject.tag) {
		case "Bullet":
			{
				var bullet = collider.gameObject.GetComponent<Bullet> ();
				if (!bullet) {
					print ("bullet null");
					break;
				}
				this.BeingShot (bullet.GetDamage());
				bullet.gameObject.SetActive(false);
			}
			break;

		case "Player":
			{
				var player = collider.gameObject.GetComponent<PlayerManager> ();
				if (!player) {
					print ("player null");
					break;
				}
				player.SetDead(true);
			}
			break;
		}
	}

	public void BeingShot (int damage) {
		this._hp = (this._hp - damage <= 0) ? 0 : (this._hp - damage);
		this.UpdateHpText ();
		if (this._hp == 0) {
			this._isDead = true;
			this.BeingExplore ();
		}
	}

	void UpdateHpText () {
		this._hpText.text = "" + this._hp;
	}

	void BeingExplore () {
		GameObject.Destroy (gameObject);
	}

	public bool IsDead() {
		return this._isDead;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
