using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public bool _isDead = false;
	float _speed;

	// Use this for initialization
	void Start () {
		
	}

	public void InitInfo (float speed) {
		_speed = speed;
	}

	public void UpdateGO (float dt) {
		var rt = GetComponent<RectTransform> ();
		rt.position = new Vector2 (rt.position.x, rt.position.y + _speed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
