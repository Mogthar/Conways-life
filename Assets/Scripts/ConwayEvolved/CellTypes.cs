using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellType
{
    protected Color _cellColour;

    public CellType(){}

    public virtual void OnEnter(Cell cell)
    {
        cell.ChangeColor(_cellColour);
    }

    public virtual void OnLeave(Cell cell)
    {

    }

    public virtual void GetNextState(Cell cell)
    {

    }
}

public class DeadCell : CellType
{
    public DeadCell()
    {
        _cellColour = Color.black;
    }

    public override void GetNextState(Cell cell)
    {
        int conwayNeighbours = 0;
        foreach(Cell neighbour in cell._neighbouringCells)
        {
            if(neighbour._currentCellType == CellGrid._conwayCellType)
            {
                conwayNeighbours += 1;
            }
        }
        if(conwayNeighbours == 3)
        {
            cell.SetNextCellType(CellGrid._conwayCellType);
        }
    }

}

public class ConwayCell : CellType
{
    private float _mutationProbability = 0.00f;
    public ConwayCell()
    {
        _cellColour = Color.white;
    }

    public override void GetNextState(Cell cell)
    {
        int conwayNeighbours = 0;
        foreach(Cell neighbour in cell._neighbouringCells)
        {
            if(neighbour._currentCellType == CellGrid._conwayCellType)
            {
                conwayNeighbours += 1;
            }
        }
        if(conwayNeighbours != 3 && conwayNeighbours !=2)
        {
            cell.SetNextCellType(CellGrid._deadCellType);
        }
        // determine whether random mutation happens
        else
        {
            if(Random.Range(0.0f, 1.0f) < _mutationProbability)
            {
                cell.SetNextCellType(CellGrid._mutatedCellType);
            }
        }

    }
}

public class MutatedCell : CellType
{
    public MutatedCell()
    {
        _cellColour = Color.green;
    }

    public override void GetNextState(Cell cell)
    {
        List<Cell> mutantNeighbours = new List<Cell>();
        foreach(Cell neighbour in cell._neighbouringCells)
        {
            if(neighbour._currentCellType == CellGrid._mutatedCellType)
            {
                mutantNeighbours.Add(neighbour);
            }
        }
    }
}
