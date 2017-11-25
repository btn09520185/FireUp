using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	public GameObject _bulletPrefab;
	PlayerStats _playerStats;
	public RectTransform _canvasRt;
	RectTransform _playerRt;

	List<GameObject> _listBullets;
	List<GameObject> _poolBullets;

	float _timeBulletFireChangeInterval;
	float _timeStyleFireChangeLapse;
	float _timeLapse;

	int _changeBulletSide;

	// Use this for initialization
	void Start () {
		print ("PlayerManager Start");
		print ("Canvas Size = " + _canvasRt.sizeDelta.x + " , " + _canvasRt.sizeDelta.y);
	}

	void Awake () {
		this._playerStats = ScriptableObject.CreateInstance<PlayerStats>();
		this._playerRt = GetComponent<RectTransform> ();
		this._listBullets = new List<GameObject> ();
		this._poolBullets = new List<GameObject> ();

		this._timeBulletFireChangeInterval = 5;
		this._changeBulletSide = -1;
	}

	GameObject GetFreeBullet () {
		GameObject ret = null;
		for (var i = 0; i < this._poolBullets.Count; i++) {
			if (!this._poolBullets[i].activeInHierarchy) {
				ret = this._poolBullets [i];
				break;
			}
		}
		if (!ret) {
			ret = (GameObject)Instantiate(this._bulletPrefab);
			this._poolBullets.Add (ret);
			ret.transform.SetParent(transform.parent);
		}
        ret.SetActive(true);
        return ret;
	}


	public IEnumerator Fire () {
		GameObject bullet = GetFreeBullet ();
		bullet.GetComponent<Bullet>().InitInfo (0, this.GetPlayerStats().BulletSpeedMove, this.GetPlayerStats().BulletPower);
		bullet.GetComponent<RectTransform> ().position = new Vector2(this._playerRt.position.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.position.y + this._playerRt.sizeDelta.y / 2 - 20);
		this._changeBulletSide *= -1;
        this._listBullets.Add (bullet);
        return null;
	}

	public IEnumerator DoubleFire () {
        GameObject bullet1 = GetFreeBullet();
        bullet1.GetComponent<Bullet>().InitInfo(_changeBulletSide * 1, this.GetPlayerStats().BulletSpeedMove, this.GetPlayerStats().BulletPower);
        bullet1.GetComponent<RectTransform>().position = new Vector2(this._playerRt.position.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
            this._playerRt.position.y + this._playerRt.sizeDelta.y / 2 - 20);
        this._changeBulletSide *= -1;
        this._listBullets.Add (bullet1);
		return null;
	}

	public IEnumerator TripleFire () {
		GameObject bullet = GetFreeBullet ();
		bullet.GetComponent<Bullet>().InitInfo (0, this.GetPlayerStats().BulletSpeedMove, this.GetPlayerStats().BulletPower);
		bullet.GetComponent<RectTransform> ().position = new Vector2(this._playerRt.position.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.position.y + this._playerRt.sizeDelta.y / 2 - 20);

        GameObject bullet1 = GetFreeBullet();
        bullet1.GetComponent<Bullet>().InitInfo(_changeBulletSide * 3, this.GetPlayerStats().BulletSpeedMove, this.GetPlayerStats().BulletPower);
        bullet1.GetComponent<RectTransform>().position = new Vector2(this._playerRt.position.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
            this._playerRt.position.y + this._playerRt.sizeDelta.y / 2 - 20);
        this._changeBulletSide *= -1;

        this._listBullets.Add(bullet);
        this._listBullets.Add(bullet1);
        return null;
	}

	public void Move (float deltaX) {
		this._playerRt.localPosition = new Vector2 (this._playerRt.localPosition.x + deltaX * 2, this._playerRt.localPosition.y);
		this.ClampToArea ();
	}

	// Clamp to canvas area
	void ClampToArea () {
		Vector3 pos = this._playerRt.localPosition;
		Vector3 minPosition = this._canvasRt.rect.min - this._playerRt.rect.min;
		Vector3 maxPosition = this._canvasRt.rect.max - this._playerRt.rect.max;

		pos.x = Mathf.Clamp(this._playerRt.localPosition.x, minPosition.x, maxPosition.x);
		pos.y = Mathf.Clamp(this._playerRt.localPosition.y, minPosition.y, maxPosition.y);

		this._playerRt.localPosition = pos;
	}

	public bool CheckCollisionWithEnemy (GameObject enemy) {
		// check bullets vs enemy
		List<GameObject> listDeadBullet = new List<GameObject>();
		foreach (var bullet in this._listBullets) {
//			if (IsCollision(bullet, enemy)) {
//				var bulletComponent = bullet.GetComponent<Bullet> ();
//				var nenemyComponent = enemy.GetComponent<Enemy> ();
//				nenemyComponent.BeingShot (bulletComponent.GetDamage ());
//				listDeadBullet.Add (bullet);
//				if (nenemyComponent.IsDead()) {
//					break;
//				}
//			}
		}
		foreach (var bullet in listDeadBullet) {
			bullet.SetActive(false);
			this._listBullets.Remove (bullet);
		}

		return enemy.GetComponent<Enemy> ().IsDead ();
	}

	bool IsCollision (GameObject object1, GameObject object2) {
		var rt1 = object1.GetComponent<RectTransform> ();
		var rt2 = object2.GetComponent<RectTransform> ();
		Rect rect1 = new Rect(rt1.localPosition.x, rt1.localPosition.y, rt1.rect.width, rt1.rect.height);
		Rect rect2 = new Rect(rt2.localPosition.x, rt2.localPosition.y, rt2.rect.width, rt2.rect.height);
		return rect1.Overlaps(rect2);
	}

	public void UpdateGO (float dt) {
		// update Fire state
		FireUpdate(dt);

		// Cheat change fire type
		//ChangeStyleFire(dt);

		// update bullets and check dead
		List<GameObject> listDeadBullet = new List<GameObject>();
		foreach (var bullet in this._listBullets) {
			if (!bullet.activeInHierarchy) {
				listDeadBullet.Add (bullet);
				continue;
			}
			var bulletComponent = bullet.GetComponent<Bullet> ();
			bulletComponent.UpdateGO (dt);
			if (bulletComponent.IsDead()) {
				listDeadBullet.Add (bullet);
			}
		}
		foreach (var bullet in listDeadBullet) {
			bullet.SetActive(false);
			this._listBullets.Remove (bullet);
		}
	}

	void FireUpdate (float dt) {
		this._timeLapse += dt;
		// fire with interval
		if (this._timeLapse >= this.GetPlayerStats().BulletSpeed[(int)this.GetPlayerStats().BulletSpeedLevel]) {
			switch (this.GetPlayerStats().FireType) {
			case FireStyle.SingleLine:
				Fire ();
				break;
			//case FireStyle.DoubleLine:
			//	DoubleFire ();
			//	break;
			case FireStyle.TripleLine:
				TripleFire ();
				break;
			}
			_timeLapse = 0;
		}
	}

	void ChangeStyleFire (float dt) {
		_timeStyleFireChangeLapse += dt;
		if (_timeStyleFireChangeLapse >= _timeBulletFireChangeInterval) {
            ChangeStyleFire ();
            _timeStyleFireChangeLapse = 0;
		}
	}

    public void ChangeStyleFire () {
        print("Change Style Fire");
        switch (this.GetPlayerStats().FireType)
        {
            case FireStyle.SingleLine:
                this.GetPlayerStats().FireType = FireStyle.TripleLine;
                break;

            //case FireStyle.DoubleLine:
            //    this.GetPlayerStats().FireType = FireStyle.TripleLine;
            //    break;

            case FireStyle.TripleLine:
                this.GetPlayerStats().FireType = FireStyle.SingleLine;
                break;

        }
    }

    // Update is called once per frame
    void Update () {
		
	}

	public PlayerStats GetPlayerStats () {
		return this._playerStats;
	}
}
