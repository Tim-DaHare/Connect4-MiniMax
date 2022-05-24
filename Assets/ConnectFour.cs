using UnityEngine;

public enum CellState
{
    Empty,
    Red,
    Blue,
}

public class ConnectFour : MonoBehaviour
{
    private Texture2D _boardTex;
    private Material _material;
    
    private int _gridWidth = 7;
    private int _gridHeight = 6;
    
    private CellState[,] _grid;

    public Node testNode;

    private void Start()
    {
        _grid = new CellState[_gridWidth, _gridHeight];
        _material = GetComponent<Renderer>().material;
        _boardTex = new Texture2D(_gridWidth, _gridHeight)
        {
            filterMode = FilterMode.Point
        };
        _material.mainTexture = _boardTex;

        // var newG = PlaceChip(_grid, 0, CellState.Blue);
        // _grid = newG;
        // newG = PlaceChip(_grid, 0, CellState.Blue);
        // _grid = newG;
        // newG = PlaceChip(_grid, 0, CellState.Blue);
        // _grid = newG;
        // newG = PlaceChip(_grid, 0, CellState.Blue);
        // _grid = newG;
        // newG = PlaceChip(_grid, 0, CellState.Blue);
        // _grid = newG;
        // newG = PlaceChip(_grid, 0, CellState.Blue);
        // _grid = newG;
        //
        // print($"first col is full: {ConnectFour.IsColumnFull(_grid, 0)}");
        
        UpdateGridTexture();
    }

    private void UpdateGridTexture()
    {
        for (var x = 0; x < _gridWidth; x++)
            for (var y = 0; y < _gridHeight; y++)
            {
                var color = _grid[x, y] switch
                {
                    CellState.Blue => Color.blue,
                    CellState.Red => Color.red,
                    _ => Color.black
                };
                _boardTex.SetPixel(x, y, color);
            }
        
        _boardTex.Apply();
    }

    private void AITurn()
    {
        var node = new Node(_grid);
        
        var sTime = Time.time;
        
        // calculate best move and get col index for that move
        var tarEval = AI.MiniMax(node, 7, int.MinValue, int.MaxValue, true);
        var tarCol = node.children.FindIndex(n => n.evalValue == tarEval);

        testNode = node;

        print($"AI took {Time.time - sTime} seconds to decide their turn, decide for column: {tarCol}, evaluation: {tarEval}. Column filled before placement: {IsColumnFull(_grid, tarCol)}");

        var newGrid = PlaceChip(_grid, tarCol, CellState.Red);
        _grid = newGrid;
        UpdateGridTexture();

        var o = IsGameOver(_grid);
        if (o) 
            print("AI Won!");
    }

    public void PlaceChipBlue(int columnIndex)
    {
        var newGrid = PlaceChip(_grid, columnIndex, CellState.Blue);
        _grid = newGrid;
        UpdateGridTexture();
        
        var o = IsGameOver(_grid);
        if (o)
        {
            print("Player Won!");
            return;
        }

        AITurn();
    }

    /// <summary>
    /// Method to place a chip of a specified color into a column of the grid
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="columnIndex"></param>
    /// <param name="color"></param>
    /// <returns>A boolean representing if placement is a success</returns>
    public static CellState[,] PlaceChip(CellState[,] grid, int columnIndex, CellState color)
    {
        var gridCopy = CloneGrid(grid);
        if (IsColumnFull(grid, columnIndex)) return gridCopy;
        
        var gridHeight = grid.GetLength(1);
        
        for (var y = gridHeight - 1; y >= 0; y--)
        {
            if (y == 0 && grid[columnIndex, y] == CellState.Empty)
            {
                gridCopy[columnIndex, y] = color;
                return gridCopy;
            }

            var botI = y - 1;
            if (botI < 0 || botI >= gridHeight) continue;
            
            if (grid[columnIndex, botI] != CellState.Empty)
            {
                gridCopy[columnIndex, y] = color;
                return gridCopy;
            }
        }

        return gridCopy;
    }

    public static CellState[,] CloneGrid(CellState[,] sourceGrid)
    {
        var width = sourceGrid.GetLength(0);
        var height = sourceGrid.GetLength(1);

        var newGrid = new CellState[width, height];

        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            newGrid[x, y] = sourceGrid[x, y];
        }
        
        return newGrid;
    }

    public static bool IsColumnFull(CellState[,] grid, int columnIndex)
    {
        var gridHeight = grid.GetLength(1);
        
        return grid[columnIndex, gridHeight - 1] != CellState.Empty;
    }

    public static bool IsGameOver(CellState[,] grid)
    {
        var gridWidth = grid.GetLength(0);
        var gridHeight = grid.GetLength(1);
        
        // Horizontal scan for 4 chips across the grid
        for (var x = 0; x < gridWidth; x++)
        for (var y = 0; y < gridHeight; y++)
        {
            var startColor = grid[x, y];
            if (startColor == CellState.Empty) continue; // Skip empty cells

            var count = 0;
            for (var s = 0; s < 4; s++)
            {
                var si = x + s;
                if (si >= gridWidth || grid[si, y] != startColor) break;
                count++;
                
                if (count >= 4) return true;
            }
        }
        
        // Vertical scan for 4 chips across the grid
        for (var x = 0; x < gridWidth; x++)
        for (var y = 0; y < gridHeight; y++)
        {
            var startColor = grid[x, y];
            if (startColor == CellState.Empty) continue; // Skip empty cells
            
            var count = 0;
            for (var s = 0; s < 4; s++)
            {
                var si = y + s;
                if (si >= gridHeight || grid[x, si] != startColor) break;
                count++;
                
                if (count >= 4) return true;
            }
        }
        
        // bottom left to top right diagonal scan for 4 chips across the grid
        for (var x = 0; x < gridWidth; x++)
        for (var y = 0; y < gridHeight; y++)
        {
            var startColor = grid[x, y];
            if (startColor == CellState.Empty) continue; // Skip empty cells
            
            var count = 0;
            for (var s = 0; s < 4; s++)
            {
                var siX = x + s;
                var siY = y + s;
                if (siX >= gridWidth || siY >= gridHeight || grid[siX, siY] != startColor) break;
                count++;
                
                if (count >= 4) return true;
            }
        }
        
        // top left to bottom right diagonal scan for 4 chips across the grid
        for (var x = 0; x < gridWidth; x++)
        for (var y = 0; y < gridHeight; y++)
        {
            var startColor = grid[x, y];
            if (startColor == CellState.Empty) continue; // Skip empty cells

            var count = 0;
            for (var s = 0; s < 4; s++)
            {
                var siX = x + s;
                var siY = y - s;
                if (siX >= gridWidth || siY < 0 || grid[siX, siY] != startColor) break;
                count++;
                    
                if (count >= 4) return true;
            }
        }
        
        return false;
    }
}
