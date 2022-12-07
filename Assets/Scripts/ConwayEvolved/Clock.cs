using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{

    [SerializeField] private float _timePeriod = 1.0f;
    private float _timer = 0.0f;

    [SerializeField] private CellGrid _cellGrid;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _timePeriod)
        {
            _cellGrid.DetermineNextState();
        }
    }

    void LateUpdate()
    {
        if(_timer >= _timePeriod)
        {
            _cellGrid.EvolveCells();
            _timer -= _timePeriod;
        }
    }
}
