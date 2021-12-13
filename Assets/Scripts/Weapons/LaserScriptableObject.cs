using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Single Shot Lasers")]
public class LaserScriptableObject : ScriptableObject
{
    public float _bulletSpeed;
    public float _bulletDamage = 1f;
    public  Quaternion _rotation;
}
