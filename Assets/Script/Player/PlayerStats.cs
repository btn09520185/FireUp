using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireStyle {
	Single = 1,
	Double = 2, 
	Triple = 3
}

public class PlayerStats : ScriptableObject {
	public int ID = 0;
	public int Heal = 100;
	public bool IsInvincible = false;
	public int BulletPower = 1;
	public int BulletSpeedLevel = 1;
	public float[] BulletSpeed = {0, 0.2f, 0.1f, 0.05f, 0.02f}; // speed create bullet by level
	public float BulletSpeedMove = 16; // 2s = 960 px
	public float BulletStartRange = 3;
	public FireStyle FireType = FireStyle.Single;
}


