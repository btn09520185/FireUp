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
	public Action<GameObject> DeadCallBack;

	// Use this for initialization
	void Start () {
		print ("Enemy Start");
		transform.localScale = new Vector2 (1, 1);
		this._hpText = transform.Find ("Text").gameObject.GetComponent<Text>();
		this.UpdateHpText ();

//		DOTween.Sequence ()
//			.Append (transform.DOScale (1.5f, 0.3f))
//			.Append (transform.DOScale (1, 0.3f))
//			.SetLoops (-1, LoopType.Yoyo);
	}

	public void InitInfo (int hp) {
		this._hp = hp;
	}

	public void BeingShot (int damage) {
		this._hp = (this._hp - damage <= 0) ? 0 : (this._hp - damage);
		this.UpdateHpText ();
		if (this._hp == 0) {
			this._isDead = true;
			BeingExplore ();
		}
	}

	void UpdateHpText () {
		this._hpText.text = "" + _hp;
	}

	void BeingExplore () {
//		DeadCallBack (gameObject);
		GameObject.Destroy (gameObject);
	}

	public bool IsDead() {
		return this._isDead;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
