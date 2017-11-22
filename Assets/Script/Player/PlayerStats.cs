using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireStyle {
	SingleLine = 1,
    TripleLine = 3
}

public class PlayerStats : ScriptableObject {
	public int ID = 0;
	public int Heal = 100;
	public bool IsInvincible = false;
    public int BulletPower = 1;
	public int BulletSpeedLevel = 1;
	public float[] BulletSpeed = {0, 0.2f, 0.15f, 0.1f, 0.08f, 0.06f, 0.05f, 0.04f, 0.03f }; // speed create bullet by level
	public float BulletSpeedMove = 16; // 2s = 960 px
	public float BulletStartRange = 5;
	public FireStyle FireType = FireStyle.SingleLine;
    public bool IsDoubleBullet = false;
}


