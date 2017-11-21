using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public bool _isDead = false;

	// Use this for initialization
	void Start () {
		
	}

	public void InitInfo (float speed) {

	}

	public void UpdateGO (float dt) {
		var rt = GetComponent<RectTransform> ();
		rt.position = new Vector2 (rt.position.x, rt.position.y + 20);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
