using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private Bullet _bulletPrefab;
    private Cannon _cannon;

    protected float _bulletSpeed;
    public float reloadSpeed;

    public bool isLoaded = true;

    public Weapon(Cannon cannon, Bullet bullet)
    {
        _cannon = cannon;
        _bulletPrefab = bullet;
    }

    public virtual void Shoot(Vector3 direction)
    {
        if(isLoaded)
        {
            Bullet bullet = Object.Instantiate(_bulletPrefab, _cannon.transform.position, Quaternion.identity, _cannon.transform);
            bullet.InitializeBullet(_bulletSpeed, direction);
            isLoaded = false;
            _cannon.Reload();
        }
    }

}

public class BasicGun : Weapon
{
    public BasicGun(Cannon cannon, Bullet bullet) : base(cannon, bullet)
    {
        _bulletSpeed = 5.0f;
        reloadSpeed = 1.0f;
    }
}
