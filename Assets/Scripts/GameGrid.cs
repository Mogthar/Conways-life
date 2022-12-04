using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    // grid geometry
    private Tile[,] _grid;
    [SerializeField] private int _gridSize;
    [SerializeField] private float _padding;
    [SerializeField] private float _refreshRate;

    // misc data
    [SerializeField] private Tile _tilePrefab;
    public static Color aliveColour = Color.red;
    public static Color deadColour = Color.white;
    [SerializeField] private LevelDataSO _levelData;


    private List<Tile> _aliveTiles;
    private float _timer;
    private bool _isGameActive = false;
    // Start is called before the first frame update
    void Awake()
    {
        Camera.main.orthographicSize = (float) _gridSize / 2;
    }

    void Start()
    {
        InitializeGrid();
        LoadLevelData();
    }

    void Update()
    {
        if(_isGameActive)
        {
            _timer += Time.deltaTime;
            if(_timer > _refreshRate)
            {
                _timer -= _refreshRate;
                EvolveGame();
            }
        }
    }

    private void EvolveGame()
    {
        // go through all the alive cells and check the close deadNeighbours
        // add dead neighbours that can potentially turn to alive ones into a list and calculat their
        // number of alive neighbours
        List<Tile> allDeadNeighbours = new List<Tile>();
        foreach(Tile tile in _aliveTiles)
        {
            // get unaccounted dead neighbouring tiles
            List<Tile> newDeadNeighbours = GetUnaccountedDeadNeighbours(tile);

            // get surrounding alive cells
            // FLAG!!!  this part is slightly redundant and should be dealt with in one step (i.e. counting dead and alive in one go)
            tile.aliveNeighbours = GetNumberOfAliveNeighbours(tile);

            // calculate alive neighbours of all the newly added active dead cells
            foreach(Tile deadNeighbour in newDeadNeighbours)
            {
                deadNeighbour.aliveNeighbours = GetNumberOfAliveNeighbours(deadNeighbour);
                allDeadNeighbours.Add(deadNeighbour);
            }
        }

        // now evaluate all alive tiles - one needs to be careful with looping and modifying a list at the same time
        // we have to loop from the back and use a for loop rather than for each loop
        for (int i = _aliveTiles.Count - 1; i >= 0; i--)
        {
            Tile tile = _aliveTiles[i];
            if(tile.aliveNeighbours != 2 && tile.aliveNeighbours != 3)
            {
                tile.Kill();
                _aliveTiles.RemoveAt(i);
            }
        }

        // this needs to be done after the alive tiles otherwise the _aliveTiles list will be
        // extended with the new alive cells before the old ones are evaulated
        foreach(Tile deadTile in allDeadNeighbours)
        {
            deadTile.wasAccountedFor = false;
            if(deadTile.aliveNeighbours == 3)
            {
                ActivateTile(deadTile);
            }
        }
    }

    public List<Tile> GetUnaccountedDeadNeighbours(Tile targetTile)
    {
        int[] positionIndices = targetTile.GetGridPosition();
        int i = positionIndices[0];
        int j = positionIndices[1];
        int[] indexOffset = new int[] {-1, 0, 1};

        List<Tile> unaccountedDeadNeighbours = new List<Tile>();

        foreach(int a in indexOffset)
        {
            foreach(int b in indexOffset)
            {
                // cant count itself
                if(a!=0 || b!=0)
                {
                    // periodic boundary conditions where -x directly connects to +x etc
                    int neighbourXIndex = (i + a + _gridSize) % _gridSize;
                    int neighbourYIndex = (j + b + _gridSize) % _gridSize;
                    Tile neighbour = _grid[neighbourXIndex, neighbourYIndex];
                    if(!neighbour.GetAliveStatus() && !neighbour.wasAccountedFor)
                    {
                        neighbour.wasAccountedFor = true;
                        unaccountedDeadNeighbours.Add(neighbour);
                    }
                }
            }
        }

        return unaccountedDeadNeighbours;
    }

    public int GetNumberOfAliveNeighbours(Tile targetTile)
    {
        int[] positionIndices = targetTile.GetGridPosition();
        int i = positionIndices[0];
        int j = positionIndices[1];
        int[] indexOffset = new int[] {-1, 0, 1};

        int numAliveNeighbours = 0;

        foreach(int a in indexOffset)
        {
            foreach(int b in indexOffset)
            {
                // cant count itself
                if(a!=0 || b!=0)
                {
                    // periodic boundary conditions where -x directly connects to +x etc
                    int neighbourXIndex = (i + a + _gridSize) % _gridSize;
                    int neighbourYIndex = (j + b + _gridSize) % _gridSize;

                    if(_grid[neighbourXIndex, neighbourYIndex].GetAliveStatus())
                    {
                        numAliveNeighbours += 1;
                    }
                }
            }
        }

        return numAliveNeighbours;

    }

    private void InitializeGrid()
    {
        _grid = new Tile[_gridSize, _gridSize];
        for(int i = 0; i < _gridSize; i++)
        {
            for(int j = 0; j < _gridSize; j++)
            {
                float xPosition = (1 - _gridSize) * (1 + _padding) / 2 + i * (1 + _padding);
                float yPosition = (1 - _gridSize) * (1 + _padding) / 2 + j * (1 + _padding);
                Vector3 tilePosition = new Vector3(xPosition, yPosition, 0);
                Tile tile = Instantiate(_tilePrefab, tilePosition, Quaternion.identity, this.transform);
                _grid[i,j] = tile;
                tile.SetGridPosition(i, j);
            }
        }

        _aliveTiles = new List<Tile>();
    }

    private void LoadLevelData()
    {
        if(_gridSize == _levelData.gridSize)
        {
            foreach(Vector2 aliveCellPosition in _levelData.initialAlivePositions)
            {
                int x_index = (int) aliveCellPosition.x;
                int y_index = (int) aliveCellPosition.y;
                Tile tile = _grid[x_index, y_index];
                tile.SetAlive();
                _aliveTiles.Add(tile);
            }
        }
        else
        {
            Debug.Log("wrong input file");
        }
    }

    public void ActivateTile(Tile tile)
    {
        tile.SetAlive();
        _aliveTiles.Add(tile);
    }

    public void DisableTile(Tile tile)
    {
        tile.Kill();
        _aliveTiles.Remove(tile);
    }

    public void PlayPause()
    {
        if(_isGameActive)
        {
            _isGameActive = false;
        }
        else
        {
            _isGameActive = true;
        }
    }
}
