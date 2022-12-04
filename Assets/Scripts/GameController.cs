using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameGrid _gameGrid;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
            foreach(var hit in hits)
            {
                if (hit.transform.TryGetComponent(out Tile tile))
                {
                    if(tile.GetAliveStatus())
                    {
                        _gameGrid.DisableTile(tile);
                    }
                    else
                    {
                        _gameGrid.ActivateTile(tile);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);
            foreach(var hit in hits)
            {
                if (hit.transform.TryGetComponent(out Tile tile))
                {
                    int aliveNeighbours = _gameGrid.GetNumberOfAliveNeighbours(tile);
                    Debug.Log(aliveNeighbours);
                }
            }
        }
    }
}
