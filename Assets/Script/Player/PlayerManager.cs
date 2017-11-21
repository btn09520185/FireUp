using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public GameObject _bulletPrefab;
	public RectTransform _canvasRt;
	PlayerStats _playerStats;
	RectTransform _playerRt;

	List<GameObject> _listBullets;

	float _timeBulletFireChangeInterval;
	float _timeStyleFireChangeLapse;
	float _timeLapse;

	int _changeBulletSide;

	// Use this for initialization
	void Start () {
		_playerStats = ScriptableObject.CreateInstance<PlayerStats>();
		print ("_playerStats = " + _playerStats);
		print ("_playerStats.ID = " + _playerStats.ID);
		print ("_playerStats.Heal = " + _playerStats.Heal);
		print ("_playerStats.BulletPower = " + _playerStats.BulletPower);
		print ("_playerStats.BulletSpeed = " + _playerStats.BulletSpeed);
		print ("_playerStats.IsInvincible = " + _playerStats.IsInvincible);

		_playerRt = GetComponent<RectTransform> ();
		_listBullets = new List<GameObject> ();

		_timeBulletFireChangeInterval = 5;
		_changeBulletSide = -1;
	}

	public IEnumerator Fire () {
		GameObject bullet = (GameObject)Instantiate(_bulletPrefab);
//		bullet.InitInfo ();
		bullet.transform.SetParent(transform.parent);
		bullet.GetComponent<RectTransform> ().position = new Vector2(_playerRt.position.x + _changeBulletSide * _playerStats.BulletStartRange,
			_playerRt.position.y + _playerRt.sizeDelta.y / 2 - 20);
		_changeBulletSide *= -1;
		_listBullets.Add (bullet);
		return null;
	}

	public IEnumerator DoubleFire () {
		GameObject bullet = (GameObject)Instantiate(_bulletPrefab);
		//		bullet.InitInfo ();
		bullet.transform.SetParent(transform.parent);
		bullet.GetComponent<RectTransform> ().position = new Vector2(_playerRt.position.x + _changeBulletSide * _playerStats.BulletStartRange,
			_playerRt.position.y + _playerRt.sizeDelta.y / 2 - 20);
		_changeBulletSide *= -1;
		_listBullets.Add (bullet);
		return null;
	}

	public IEnumerator TripleFire () {
		GameObject bullet = (GameObject)Instantiate(_bulletPrefab);
		//		bullet.InitInfo ();
		bullet.transform.SetParent(transform.parent);
		bullet.GetComponent<RectTransform> ().position = new Vector2(_playerRt.position.x + _changeBulletSide * _playerStats.BulletStartRange,
			_playerRt.position.y + _playerRt.sizeDelta.y / 2 - 20);
		_changeBulletSide *= -1;
		_listBullets.Add (bullet);
		return null;
	}

	public void Move (float deltaX) {
		var rectTranform = GetComponent<RectTransform> ();
		rectTranform.localPosition = new Vector2 (rectTranform.localPosition.x + deltaX, rectTranform.localPosition.y);
		ClampToArea ();
	}

	// Clamp to canvas area
	public void ClampToArea()
	{
		Vector3 pos = _playerRt.localPosition;

		Vector3 minPosition = _canvasRt.rect.min - _playerRt.rect.min;
		Vector3 maxPosition = _canvasRt.rect.max - _playerRt.rect.max;

		pos.x = Mathf.Clamp(_playerRt.localPosition.x, minPosition.x, maxPosition.x);
		pos.y = Mathf.Clamp(_playerRt.localPosition.y, minPosition.y, maxPosition.y);

		_playerRt.localPosition = pos;
	}

	public void UpdateGO (float dt) {
		// update Fire state
		FireUpdate(dt);

		// Cheat change style fire
		ChangeStyleBulletFire(dt);

		// update bullets and check dead
		List<GameObject> listDeadBullet = new List<GameObject>();
		foreach (var bullet in _listBullets) {
			bullet.GetComponent<Bullet> ().UpdateGO (dt);
			if (CheckDeadBullet(bullet)) {
				listDeadBullet.Add (bullet);
			}
		}
		foreach (var bullet in listDeadBullet) {
			GameObject.Destroy (bullet);
			_listBullets.Remove (bullet);
		}
	}

	void FireUpdate (float dt) {
		_timeLapse += dt;
		// fire with interval
		if (_timeLapse >= _playerStats.BulletSpeed) {
			switch (_playerStats.BulletFireType) {
			case BulletFireStyle.Single:
				Fire ();
				break;
			case BulletFireStyle.Double:
				DoubleFire ();
				break;
			case BulletFireStyle.Triple:
				TripleFire ();
				break;
			}
			_timeLapse = 0;
		}
	}

	void ChangeStyleBulletFire (float dt) {
		_timeStyleFireChangeLapse += dt;
		if (_timeStyleFireChangeLapse >= _timeBulletFireChangeInterval) {
			print ("Change Style Fire");
			switch (_playerStats.BulletFireType) {
			case BulletFireStyle.Single:
				_playerStats.BulletFireType = BulletFireStyle.Double;
				break;

			case BulletFireStyle.Double:
				_playerStats.BulletFireType = BulletFireStyle.Triple;
				break;

			case BulletFireStyle.Triple:
				_playerStats.BulletFireType = BulletFireStyle.Single;
				break;

			}

			_timeStyleFireChangeLapse = 0;
		}
	}

	bool CheckDeadBullet (GameObject bullet) {
		return bullet.transform.position.y >= _canvasRt.sizeDelta.y;
	}

	// Update is called once per frame
	void Update () {



	}
}
