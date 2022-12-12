using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _speed;
    private Vector3 _direction;

    // Update is called once per frame
    void Update()
    {
        float distance = Time.deltaTime * _speed;
        transform.Translate(_direction.x * distance, _direction.y * distance, _direction.z * distance);
        CheckImpact();
    }

    public void InitializeBullet(float speed, Vector3 direction)
    {
        _speed = speed;
        _direction = direction;
    }

    private void CheckImpact()
    {
        Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y, -1.0f);
        Ray ray = new Ray(rayOrigin, Vector3.forward); //cast upwards
        RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
        foreach(var hit in hits)
        {
            if (hit.transform.TryGetComponent(out Cell cell))
            {
                CellType cellType = cell._currentCellType;
                if(cellType == CellGrid._conwayCellType || cellType == CellGrid._mutatedCellType)
                {
                    cell.SetNextCellType(CellGrid._deadCellType);
                    cell.ChangeType();
                    Destroy(this.gameObject);
                }
            }
        }
    }

}
