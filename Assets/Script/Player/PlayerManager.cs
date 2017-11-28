using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
	int _originalNumEnemyOfLine;
	float _currentBulletScale;

	// Use this for initialization
	void Start () {
		print ("PlayerManager Start");
		print ("Canvas Size = " + _canvasRt.sizeDelta.x + " , " + _canvasRt.sizeDelta.y);
		print ("Screen Size = " + Screen.width + " , " + Screen.height);

	}

	void Awake () {
		this._playerStats = ScriptableObject.CreateInstance<PlayerStats>();
		this._playerRt = GetComponent<RectTransform> ();
		this._listBullets = new List<GameObject> ();
		this._poolBullets = new List<GameObject> ();

		this._timeBulletFireChangeInterval = 5;
		this._changeBulletSide = -1;
		this._originalNumEnemyOfLine = 4;
		this._currentBulletScale = 1;
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
		ret.GetComponent<RectTransform> ().localScale = new Vector2 (_currentBulletScale, _currentBulletScale);
        ret.SetActive(true);
        return ret;
	}

	public void ChangeNumEnemyAction (int newnum) {
		float scale = (float)this._originalNumEnemyOfLine / (float)newnum;
		_currentBulletScale = scale;
		DOTween.Sequence ()
			.Append (transform.DOScale (scale, 2f));
		foreach (var bullet in this._listBullets) {
			bullet.GetComponent<RectTransform> ().localScale = new Vector2 (_currentBulletScale, _currentBulletScale);
		}
	}


	public IEnumerator Fire () {
		GameObject bullet = GetFreeBullet ();
		bullet.GetComponent<Bullet>().InitInfo (0, this.GetPlayerStats().BulletSpeedMove, this.GetPlayerStats().BulletPower);
		bullet.GetComponent<RectTransform> ().localPosition = new Vector2(this._playerRt.localPosition.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.localPosition.y + this._playerRt.sizeDelta.y / 2 - 20);
		this._changeBulletSide *= -1;
        this._listBullets.Add (bullet);
        return null;
	}

	public IEnumerator DoubleFire () {
        GameObject bullet1 = GetFreeBullet();
        bullet1.GetComponent<Bullet>().InitInfo(_changeBulletSide * 1, this.GetPlayerStats().BulletSpeedMove, this.GetPlayerStats().BulletPower);
		bullet1.GetComponent<RectTransform>().localPosition = new Vector2(this._playerRt.localPosition.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.localPosition.y + this._playerRt.sizeDelta.y / 2 - 20);
        this._changeBulletSide *= -1;
        this._listBullets.Add (bullet1);
		return null;
	}

	public IEnumerator TripleFire () {
		GameObject bullet = GetFreeBullet ();
		bullet.GetComponent<Bullet>().InitInfo (0, this.GetPlayerStats().BulletSpeedMove, this.GetPlayerStats().BulletPower);
		bullet.GetComponent<RectTransform> ().localPosition = new Vector2(this._playerRt.localPosition.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.localPosition.y + this._playerRt.sizeDelta.y / 2 - 20);

        GameObject bullet1 = GetFreeBullet();
        bullet1.GetComponent<Bullet>().InitInfo(_changeBulletSide * 3, this.GetPlayerStats().BulletSpeedMove, this.GetPlayerStats().BulletPower);
		bullet1.GetComponent<RectTransform>().localPosition = new Vector2(this._playerRt.localPosition.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.localPosition.y + this._playerRt.sizeDelta.y / 2 - 20);
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
