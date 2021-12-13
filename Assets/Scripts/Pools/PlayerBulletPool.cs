using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBulletPool : MonoBehaviour
{
    protected static PlayerBulletPool _instance;
    public static PlayerBulletPool Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("Bullet pool is NULL!");
            return _instance;
        }
    }

    [SerializeField] protected GameObject _bulletPrefab;
    [SerializeField] protected int _bulletAmount = 10;
    [SerializeField] Transform _bulletContainer;
    [SerializeField] protected List<GameObject> _bulletPool;

    protected void Awake()
    {
        _instance = this;
    }

    protected void Start()
    {
        _bulletPool = GenerateBullets(_bulletAmount);
    }

    protected List<GameObject> GenerateBullets(int _bulletAmount)
    {
        for (int i = 0; i < _bulletAmount; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab);
            bullet.transform.parent = _bulletContainer;
            bullet.SetActive(false);
            _bulletPool.Add(bullet);
        }
        return _bulletPool;
    }

    public GameObject RequestBullet()
    {
        foreach (var bullet in _bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        var newBullet = Instantiate(_bulletPrefab);
        newBullet.transform.parent = _bulletContainer;
        _bulletPool.Add(newBullet);
        return newBullet;
    }
}
