using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour, IDragHandler/*, IPointerDownHandler, IPointerUpHandler*/ {
	PlayerManager _player;

	// Use this for initialization
	void Start () {
		_player = GetComponent<PlayerManager> ();
	}

	public void OnDrag (PointerEventData eventData) {
		_player.Move(eventData.delta.x);
	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
