using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
	Text _hpText;
	int _hp;
	bool _isDead;

	// Use this for initialization
	void Start () {
		print ("Enemy Start");
		this._hpText = transform.Find ("Text").gameObject.GetComponent<Text>();
		this.UpdateHpText ();
	}

	public void InitInfo (int hp) {
		this._hp = hp;

	}

	public void BeingShot (int damage) {
		this._hp = (this._hp - damage <= 0) ? 0 : (this._hp - damage);
		if (this._hp == 0) {
			this._isDead = true;
		}
		this.UpdateHpText ();
	}

	public bool IsDead() {
		return this._isDead;
	}

	void UpdateHpText () {
		this._hpText.text = "" + _hp;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
