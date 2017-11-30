using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager {
	List<GameObject> _listBullets;
	List<GameObject> _poolBullets;
	GameObject _bulletPrefab;
	GameObject _gameContainer;

	public BulletManager (GameObject bulletPrefab, GameObject gameContainer) {
		this._bulletPrefab = bulletPrefab;
		this._gameContainer = gameContainer;

		// init
		this._listBullets = new List<GameObject> ();
		this._poolBullets = new List<GameObject> ();
	}

	public GameObject GetFreeBullet (Vector2 speed, int damage, Vector2 position, float scale) {
		GameObject ret = null;
		for (var i = 0; i < this._poolBullets.Count; i++) {
			if (!this._poolBullets[i].activeInHierarchy) {
				ret = this._poolBullets [i];
				break;
			}
		}
		if (!ret) {
			ret = GameObject.Instantiate(this._bulletPrefab);
			this._poolBullets.Add (ret);
			ret.transform.SetParent(this._gameContainer.transform);
		}
		ret.SetActive(true);
		ret.GetComponent<Bullet>().InitInfo (speed, damage);
		ret.GetComponent<RectTransform> ().localPosition = position;
		ret.GetComponent<RectTransform> ().localScale = new Vector2(scale, scale);

		this._listBullets.Add (ret);
		return ret;
	}

	public void ChangeAllBulletSize(float scale) {
		foreach (var bullet in this._listBullets) {
			bullet.GetComponent<RectTransform> ().localScale = new Vector2 (scale, scale);
		}
	}

	public void RemoveAllBullet() {
		foreach (var bullet in this._listBullets) {
			if (!bullet) {
				continue;
			}
			GameObject.Destroy (bullet);
		}
		this._listBullets.Clear ();
		foreach (var bullet in this._poolBullets) {
			if (!bullet) {
				continue;
			}
			GameObject.Destroy (bullet);
		}
		this._poolBullets.Clear ();
	}

	public void UpdateGO(float dt) {
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
}
