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
        Vector3 position = new Vector3(transform.position.x, - _cellGrid._gridSize/2, 0);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0.0f);
            direction = Vector3.Normalize(direction);
            _currentWeapon.Shoot(direction);
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        _currentWeapon = newWeapon;
    }

    public void Reload()
    {
        StartCoroutine(ReloadWeapon(_currentWeapon));
    }

    private IEnumerator ReloadWeapon(Weapon weapon)
    {
        float timer = 0.0f;
        while(timer < weapon.reloadSpeed)
        {
            UIManager.Instance.reloadSlider.value = timer / weapon.reloadSpeed;
            timer += Time.deltaTime;
            yield return null;
        }
        weapon.isLoaded = true;
    }
}
