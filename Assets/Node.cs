using System;
using System.Collections.Generic;

// [Serializable]
public class Node
{
    public CellState[,] grid;
    public List<Node> children = new();
    
    public int evalValue;
    
    public Node(CellState[,] cGrid)
    {
        grid = ConnectFour.CloneGrid(cGrid);
    }

    public void InitializeChildren(int childCount, bool isMax)
    {
        for (var i = 0; i < childCount; i++)
        {
            var color = isMax ? CellState.Red : CellState.Blue;
            var newGrid = ConnectFour.PlaceChip(grid, i, color);
            
            children.Add(new Node(newGrid));
        }
    }

    public int Evaluate(bool isMax)
    {
        var gridWidth = grid.GetLength(0);
        var gridHeight = grid.GetLength(1);
        
        var score = 0;
        
        var pColor = isMax ? CellState.Red : CellState.Blue;
        var oppColor = isMax ? CellState.Blue : CellState.Red;
        
        // Horizontal scan for evaluation
        for (var x = 0; x < gridWidth; x++)
        for (var y = 0; y < gridHeight; y++)
        {
            var startColor = grid[x, y];
            if (startColor == CellState.Empty) continue; // Skip empty cells
            
            var count = 0;
            for (var s = 0; s < 4; s++)
            {
                var si = x + s;
                
                if (si >= gridWidth || grid[si, y] != startColor)
                {
                    if (startColor == pColor)
                    {
                        switch (count)
                        {
                            case >= 4:
                                score += 100;
                                break;
                            case 3:
                                score += 5;
                                break;
                            case 2:
                                score += 2;
                                break;
                        }
                    }
                    else if (startColor == oppColor && count >= 3)
                    {
                        score -= 4;
                    }
                }
                
                count++;
            }
        }
        
        // Vertical scan for evaluation
        for (var x = 0; x < gridWidth; x++)
        for (var y = 0; y < gridHeight; y++)
        {
            var startColor = grid[x, y];
            if (startColor == CellState.Empty) continue; // Skip empty cells
            
            var count = 0;
            for (var s = 0; s < 4; s++)
            {
                var si = y + s;
                
                if (si >= gridHeight || grid[x, si] != startColor)
                {
                    if (startColor == pColor)
                    {
                        switch (count)
                        {
                            case >= 4:
                                score += 100;
                                break;
                            case 3:
                                score += 5;
                                break;
                            case 2:
                                score += 2;
                                break;
                        }
                    }
                    else if (startColor == oppColor && count >= 3)
                    {
                        score -= 4;
                    }
                }
                    
                count++;
            }
        }
        
        // Diagonal bot left to top right scan for evaluation
        for (var x = 0; x < gridWidth; x++)
        for (var y = 0; y < gridHeight; y++)
        {
            var startColor = grid[x, y];
            if (startColor == CellState.Empty) continue; // Skip empty cells
            
            var count = 0;
            for (var s = 0; s < 4; s++)
            {
                var xi = x + s;
                var yi = y + s;
                
                if (xi >= gridWidth || yi >= gridHeight ||  grid[xi, yi] != startColor)
                {
                    if (startColor == pColor)
                    {
                        switch (count)
                        {
                            case >= 4:
                                score += 100;
                                break;
                            case 3:
                                score += 5;
                                break;
                            case 2:
                                score += 2;
                                break;
                        }
                    }
                    else if (startColor == oppColor && count >= 3)
                    {
                        score -= 4;
                    }
                }
                    
                count++;
            }
        }
        
        // Diagonal top left to bot right scan for evaluation
        for (var x = 0; x < gridWidth; x++)
        for (var y = 0; y < gridHeight; y++)
        {
            var startColor = grid[x, y];
            if (startColor == CellState.Empty) continue; // Skip empty cells
            
            var count = 0;
            for (var s = 0; s < 4; s++)
            {
                var xi = x + s;
                var yi = y - s;
                
                if (xi >= gridWidth || yi < 0 ||  grid[xi, yi] != startColor)
                {
                    if (startColor == pColor)
                    {
                        switch (count)
                        {
                            case >= 4:
                                score += 100;
                                break;
                            case 3:
                                score += 5;
                                break;
                            case 2:
                                score += 2;
                                break;
                        }
                    }
                    else if (startColor == oppColor && count >= 3)
                    {
                        score -= 4;
                    }
                }
                    
                count++;
            }
        }

        return score;
    }
}
