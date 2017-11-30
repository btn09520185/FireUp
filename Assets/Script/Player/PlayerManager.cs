using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour {
	public GameObject _bulletPrefab;
	PlayerStats _playerStats;
	public RectTransform _canvasRt;
	RectTransform _playerRt;
	bool _isDead;
	BulletManager _bulletManager;

	float _timeLapse;

	int _changeBulletSide;
	int _originalNumEnemyOfLine;
	float _currentBulletScale;

	// Use this for initialization
	void Start () {

	}

	void Awake () {
		this._playerStats = ScriptableObject.CreateInstance<PlayerStats>();
		this._playerRt = GetComponent<RectTransform> ();
		this._bulletManager = new BulletManager (this._bulletPrefab, this.transform.parent.gameObject);

		this._changeBulletSide = -1;
		this._originalNumEnemyOfLine = 4;
		this._currentBulletScale = 1;
	}

	public void ActivePlayer (bool isActive) {
		this.gameObject.SetActive(isActive);
		this.SetDead (!isActive);
	}

	public void Fire () {
		var speed = new Vector2 (0, this.GetPlayerStats ().BulletSpeedMove);
		var damage = this.GetPlayerStats().BulletPower;
		var position = new Vector2(this._playerRt.localPosition.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.localPosition.y + this._playerRt.sizeDelta.y / 2 - 20);
		this._changeBulletSide *= -1;
		var scale = this._currentBulletScale;

		this._bulletManager.GetFreeBullet (speed, damage, position, scale);
	}

	public void DoubleFire () {
		var speed = new Vector2 (0, this.GetPlayerStats ().BulletSpeedMove);
		var damage = this.GetPlayerStats().BulletPower;
		var position = new Vector2(this._playerRt.localPosition.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.localPosition.y + this._playerRt.sizeDelta.y / 2 - 20);
		this._changeBulletSide *= -1;
		var scale = this._currentBulletScale;

		this._bulletManager.GetFreeBullet (speed, damage, position, scale);
	}

	public void TripleFire () {
		var speed = new Vector2 (0, this.GetPlayerStats ().BulletSpeedMove);
		var damage = this.GetPlayerStats().BulletPower;
		var position = new Vector2(this._playerRt.localPosition.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.localPosition.y + this._playerRt.sizeDelta.y / 2 - 20);
		var scale = this._currentBulletScale;

		this._bulletManager.GetFreeBullet (speed, damage, position, scale);

		speed = new Vector2 (this._changeBulletSide * 3, this.GetPlayerStats ().BulletSpeedMove);
		damage = this.GetPlayerStats().BulletPower;
		position = new Vector2(this._playerRt.localPosition.x + this._changeBulletSide * this.GetPlayerStats().BulletStartRange,
			this._playerRt.localPosition.y + this._playerRt.sizeDelta.y / 2 - 20);
		scale = this._currentBulletScale;
		this._changeBulletSide *= -1;

		this._bulletManager.GetFreeBullet (speed, damage, position, scale);
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

		// update bullets
		this._bulletManager.UpdateGO(dt);
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
			this._timeLapse = 0;
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

	public void SetDead(bool isDead) {
		this._isDead = isDead;
	}

	public bool IsDead() {
		return this._isDead;
	}

	public void BeingDestroy() {
		this._bulletManager.RemoveAllBullet ();
		this.ActivePlayer (false);
	}

    // Update is called once per frame
    void Update () {
		
	}

	public void ChangeNumEnemyAction (int newnum) {
		float scale = (float)this._originalNumEnemyOfLine / (float)newnum;
		this._currentBulletScale = scale;
		DOTween.Sequence ()
			.Append (transform.DOScale (scale, 2f));

		this._bulletManager.ChangeAllBulletSize(scale);
	}

	public PlayerStats GetPlayerStats () {
		return this._playerStats;
	}
}
