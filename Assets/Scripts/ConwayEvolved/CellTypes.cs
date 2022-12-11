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
    private float _mutationProbability = 0.01f;
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
        int[] cellPosition = cell.GetGridPosition();
        List<Vector2> mutantNeighbourPosition = new List<Vector2>();
        List<Cell> normalNeighbour = new List<Cell>();

        foreach(Cell neighbour in cell._neighbouringCells)
        {
            if(neighbour._currentCellType == CellGrid._mutatedCellType)
            {
                int[] neighbourPosition = neighbour.GetGridPosition();
                mutantNeighbourPosition.Add(new Vector2(neighbourPosition[0] - cellPosition[0], neighbourPosition[1] - cellPosition[1]));
            }
            else
            {
                normalNeighbour.Add(neighbour);
            }
        }

        // the total probability of mutation depends on the number of mutants around
        // capped at 100% when all neighbours are mutants
        float mutationProbability = (2.0f + mutantNeighbourPosition.Count) / 11.0f;

        // if there is a free space around the mutant then grow
        if(mutationProbability < 1.0f)
        {
            float[] neighbourMutProbab = new float[normalNeighbour.Count];
            for(int i = 0; i < normalNeighbour.Count; i++)
            {
                int[] neighbourPosition = normalNeighbour[i].GetGridPosition();
                Vector2 neighbourVector = new Vector2(neighbourPosition[0] - cellPosition[0], neighbourPosition[1] - cellPosition[1]);
                foreach(Vector2 mutantVector in mutantNeighbourPosition)
                {
                    // potentially put the dot product to a power to favour heavily opposite directions
                    neighbourMutProbab[i] += (1.0f - Vector2.Dot(neighbourVector, mutantVector)) / 2.0f;
                }
            }
            //normalize the probabilites such that they all add up to mutationProbability determined by num of mutant neighbours
            int norm = 0;
            foreach(int probability in neighbourMutProbab)
            {
                norm += probability;
            }
            for(int i = 0; i < neighbourMutProbab.Length; i++)
            {
                neighbourMutProbab[i] *= mutationProbability / norm;
            }

            // randomly choose direction to mutate
            float random = Random.Range(0.0f, 1.0f);
            if(random < mutationProbability)
            {
                float cummulativeProbab = 0.0f;
                for(int i = 0; i < normalNeighbour.Count; i++)
                {
                    cummulativeProbab += neighbourMutProbab[i];
                    if(random <= cummulativeProbab)
                    {
                        normalNeighbour[i].SetNextCellType(CellGrid._mutatedCellType);
                        normalNeighbour[i].ChangeType();
                        break;
                    }
                }
            }
        }
    }
}
