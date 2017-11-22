using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDragHandler/*, IPointerDownHandler, IPointerUpHandler*/ {
	PlayerManager _playerManager;

	// Use this for initialization
	void Start () {
		this._playerManager = GetComponent<PlayerManager> ();
	}

	public void OnDrag (PointerEventData eventData) {
		this._playerManager.Move(eventData.delta.x);
	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
