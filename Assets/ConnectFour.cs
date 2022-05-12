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

    private void Start()
    {
        _grid = new CellState[_gridWidth, _gridHeight];
        _material = GetComponent<Renderer>().material;
        _boardTex = new Texture2D(_gridWidth, _gridHeight)
        {
            filterMode = FilterMode.Point
        };
        _material.mainTexture = _boardTex;
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

    public void PlaceChipBlue(int columnIndex)
    {
        PlaceChip(columnIndex, CellState.Blue);
    }

    private void PlaceChip(int columnIndex, CellState color)
    {
        for (var y = _gridHeight - 1; y >= 0; y--)
        {
            if (y == 0 && _grid[columnIndex, y] == CellState.Empty)
            {
                _grid[columnIndex, y] = color;
                UpdateGridTexture();
                return;
            }
            
            if (_grid[columnIndex, y - 1] != CellState.Empty)
            {
                _grid[columnIndex, y] = color;
                UpdateGridTexture();
                return;
            }
        }
    }
}
