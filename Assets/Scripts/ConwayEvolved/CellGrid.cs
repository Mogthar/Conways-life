using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGrid : MonoBehaviour
{

    // grid geometry
    private Cell[,] _grid;
    [SerializeField] public int _gridSize;
    [SerializeField] private float _padding;

    // misc data
    [SerializeField] private Cell _cellPrefab;

    public static DeadCell _deadCellType;
    public static ConwayCell _conwayCellType;
    public static MutatedCell _mutatedCellType;

    private List<CellType> _cellTypes;
    private System.Random rnd;

    // Start is called before the first frame update
    void Awake()
    {
        Camera.main.orthographicSize = (float) _gridSize / 2;

        _deadCellType = new DeadCell();
        _conwayCellType = new ConwayCell();
        _mutatedCellType = new MutatedCell();

        _cellTypes = new List<CellType>();

        _cellTypes.Add(_deadCellType);
        _cellTypes.Add(_conwayCellType);
        _cellTypes.Add(_mutatedCellType);

        rnd = new System.Random();
    }

    void Start()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        // check if grid already exists
        if(_grid != null)
        {
            for(int i = 0; i < _gridSize; i++)
            {
                for(int j =  0; j < _gridSize; j++)
                {
                    Destroy(_grid[i,j].gameObject);
                }
            }
        }

        // create new grid
        _grid = new Cell[_gridSize, _gridSize];

        // first create a grid of cells and assign random types
        for(int i = 0; i < _gridSize; i++)
        {
            for(int j = 0; j < _gridSize; j++)
            {
                float xPosition = (1.0f - _gridSize) * (1.0f + _padding) / 2.0f + i * (1.0f + _padding);
                float yPosition = (1.0f - _gridSize) * (1.0f + _padding) / 2.0f + j * (1.0f + _padding);
                Vector3 cellPosition = new Vector3(xPosition, yPosition, 0);

                Cell cell = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, this.transform);
                _grid[i,j] = cell;
                cell.SetGridPosition(i, j);

                cell.SetNextCellType(GetRandomCellType());
                cell.ChangeType();
            }
        }

        // then populate neighbour lists for every cell
        for(int i = 0; i < _gridSize; i++)
        {
            for(int j = 0; j < _gridSize; j++)
            {
                Cell targetCell = _grid[i,j];
                int[] indexOffset = new int[] {-1, 0, 1};
                foreach(int a in indexOffset)
                {
                    foreach(int b in indexOffset)
                    {
                        // cant count itself
                        if(a != 0 || b!=0)
                        {
                            int xInd = i + a;
                            int yInd = j + b;
                            if((xInd >= 0 && xInd < _gridSize) && (yInd >= 0 && yInd < _gridSize))
                            {
                                targetCell.AddNeighbour(_grid[xInd,yInd]);
                            }
                        }
                    }
                }
            }
        }
    }

    public void DetermineNextState()
    {
        List<Cell> mutatedCells = new List<Cell>();
        // first go through normal cells
        for(int i = 0; i < _gridSize; i++)
        {
            for(int j = 0; j < _gridSize; j++)
            {
                Cell cell = _grid[i,j];
                if(cell._currentCellType == _mutatedCellType)
                {
                    mutatedCells.Add(cell);
                }
                else
                {
                    cell.GetNextState();
                }
            }
        }
        // then go through mutated Cells
        foreach(Cell mutant in mutatedCells)
        {
            mutant.GetNextState();
        }
    }

    public void EvolveCells()
    {
        for(int i = 0; i < _gridSize; i++)
        {
            for(int j = 0; j < _gridSize; j++)
            {
                Cell cell = _grid[i,j];
                cell.ChangeType();
            }
        }
    }

    public CellType GetRandomCellType()
    {
        int randIndex = rnd.Next(_cellTypes.Count-1);
        return _cellTypes[randIndex];
    }
}
