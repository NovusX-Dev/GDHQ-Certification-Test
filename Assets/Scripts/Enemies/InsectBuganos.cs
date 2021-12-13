using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectBuganos : Enemy
{
    [SerializeField] private Vector3 _direction = Vector3.left;

    protected override void Move()
    {
        transform.Translate(_direction * _moveSpeed * Time.deltaTime, Space.World);
    }
}
