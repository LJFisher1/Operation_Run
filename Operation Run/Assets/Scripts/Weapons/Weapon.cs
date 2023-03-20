using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public int damage;
    public float useTime;
    public int range;
    public float duration;
    public float bulletSpeed;
    public GameObject model;
    public GameObject bullet;
    public bool canBreakWalls;
}
