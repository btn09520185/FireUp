using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletFireStyle {
	Single = 1,
	Double = 2, 
	Triple = 3
}

public class PlayerStats : ScriptableObject {
	public int ID = 0;
	public int Heal = 100;
	public bool IsInvincible = false;
	public int BulletPower = 1;
	public float BulletSpeed = 0.2f;
	public float BulletStartRange = 5;
	public BulletFireStyle BulletFireType = BulletFireStyle.Single;
}


