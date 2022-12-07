using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private SpriteRenderer _mySprite;
    private int[] _gridPosition = new int[2];
    public List<Cell> _neighbouringCells = new List<Cell>();

    public CellType _currentCellType;
    private CellType _nextCellType;

    // Start is called before the first frame update
    void Awake()
    {
        _mySprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetGridPosition(int i, int j)
    {
        _gridPosition[0] = i;
        _gridPosition[1] = j;
    }

    public int[] GetGridPosition()
    {
        return _gridPosition;
    }

    public void AddNeighbour(Cell cell)
    {
        _neighbouringCells.Add(cell);
    }

    public void GetNextState()
    {
        _currentCellType.GetNextState(this);
    }

    public void ChangeType()
    {
        if(_nextCellType != null)
        {
            if(_currentCellType != null)
            {
                _currentCellType.OnLeave(this);
            }
            _currentCellType = _nextCellType;
            _currentCellType.OnEnter(this);
            _nextCellType = null;
        }
    }

    public void SetNextCellType(CellType nextType)
    {
        _nextCellType = nextType;
    }

    public void ChangeColor(Color color)
    {
        _mySprite.color = color;
    }
}
