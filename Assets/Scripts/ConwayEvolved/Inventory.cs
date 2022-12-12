using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Cannon _cannon;
    [SerializeField] private Bullet _basicBullet;

    private BasicGun _basicGun;

    void Start()
    {
        _basicGun = new BasicGun(_cannon, _basicBullet);
        EquipWeapon(_basicGun);
    }

    public void EquipWeapon(Weapon weapon)
    {
        _cannon.EquipWeapon(weapon);
    }

}
