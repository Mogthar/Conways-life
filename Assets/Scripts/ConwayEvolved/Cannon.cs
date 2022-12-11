using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private CellGrid _cellGrid;
    private Weapon _currentWeapon;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EquipWeapon(Weapon newWeapon)
    {
        _currentWeapon = newWeapon;
    }
}
