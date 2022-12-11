using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private Bullet _bulletPrefab;
    private float _bulletSpeed;
    private Cannon _cannon;

    public Weapon(Cannon cannon)
    {
        _cannon = cannon;
    }

    public virtual void Shoot(Vector3 direction)
    {
        Bullet bullet = Object.Instantiate(_bulletPrefab, _cannon.transform.position, Quaternion.identity, _cannon.transform);
        bullet.InitializeBullet(_bulletSpeed, direction);
    }
}

public class BasicGun : Weapon
{

      public BasicGun(Cannon cannon) : base(cannon)
      {

      }

}
