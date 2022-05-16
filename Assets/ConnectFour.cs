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
    
    // private bool _playerStarts;
    private bool _isPlayersTurn = true;
    
    private void Start()
    {
        _grid = new CellState[_gridWidth, _gridHeight];
        _material = GetComponent<Renderer>().material;
        _boardTex = new Texture2D(_gridWidth, _gridHeight)
        {
            filterMode = FilterMode.Point
        };
        _material.mainTexture = _boardTex;
        // PlaceChip(0, CellState.Red);
        // PlaceChip(0, CellState.Red);
        // PlaceChip(0, CellState.Red);
        // PlaceChip(1, CellState.Red);
        // PlaceChip(1, CellState.Red);
        // PlaceChip(2, CellState.Red);
        //
        // PlaceChip(4, CellState.Red);
        // PlaceChip(5, CellState.Red);
        // PlaceChip(5, CellState.Red);
        // PlaceChip(6, CellState.Red);
        // PlaceChip(6, CellState.Red);
        // PlaceChip(6, CellState.Red);
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

        node.InitializeChildren(_gridWidth, _gridWidth, _gridHeight);

        // calculate best move and get col index for that move
        var tarEval = AI.MiniMax(node, 3, int.MinValue, int.MaxValue, true);
        var tarCol = node.children.FindIndex(n => n.evalValue == tarEval);
        
        PlaceChip(tarCol, CellState.Red);
        
        print($"AI took {Time.time - sTime} seconds to decide their turn");
        
        _isPlayersTurn = true;
    }

    public void PlaceChipBlue(int columnIndex)
    {
        // if (!_isPlayersTurn) return;
        
        PlaceChip(columnIndex, CellState.Blue);
        var o = IsGameOver(_grid);
        print(o);
        // _isPlayersTurn = false;

        // AITurn();
    }

    /// <summary>
    /// Method to place a chip of a specified color into a column of the grid
    /// </summary>
    /// <param name="columnIndex"></param>
    /// <param name="color"></param>
    /// <returns>A boolean representing if placement is a success</returns>
    private bool PlaceChip(int columnIndex, CellState color)
    {
        for (var y = _gridHeight - 1; y >= 0; y--)
        {
            if (y == 0 && _grid[columnIndex, y] == CellState.Empty)
            {
                _grid[columnIndex, y] = color;
                UpdateGridTexture();
                return true;
            }
            
            if (_grid[columnIndex, y - 1] != CellState.Empty)
            {
                _grid[columnIndex, y] = color;
                UpdateGridTexture();
                return true;
            }
        }

        return false;
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
                if (si >= gridWidth || grid[x, si] != startColor) break;
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
                if (siX >= gridWidth || siY < 0 ||  siY >= gridHeight || grid[siX, siY] != startColor) break;
                count++;
                    
                if (count >= 4) return true;
            }
        }
        
        return false;
    }
}
