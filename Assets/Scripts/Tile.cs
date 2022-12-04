using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isAlive = false;
    private SpriteRenderer _mySprite;
    private int[] _gridPosition = new int[2];
    public int aliveNeighbours = 0;
    public bool wasAccountedFor = false;
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

    public void Kill()
    {
        isAlive = false;
        _mySprite.color = GameGrid.deadColour;
    }

    public void SetAlive()
    {
        isAlive = true;
        _mySprite.color = GameGrid.aliveColour;
    }

    public bool GetAliveStatus()
    {
        return isAlive;
    }
}
