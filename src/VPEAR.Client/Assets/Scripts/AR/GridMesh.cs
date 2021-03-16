using System;
using UnityEngine;

public class GridMesh
{
    private readonly int _width;
    private readonly int _height;
    private readonly float _cellSize;
    private readonly Vector3 _origin;
    private readonly int _minValue;
    private readonly int _maxValue;
    private ColorScale _colorScale;
    private int[,] _gridValues;
    private TextMesh[,] _textMeshes;

    public GridMesh(int width, int height, int minValue, int maxValue, float cellSize, Vector3 origin)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _origin = origin;
        _minValue = minValue;
        _maxValue = maxValue;
        _colorScale = ColorScale.RedToGreen;
        _gridValues = new int[width, height];
        _textMeshes = new TextMesh[width, height];

        for (var x = 0; x < _width; x++)
        {
            for (var y = 0; y < _height; y++)
            {
                _textMeshes[x, y] = CreateWorldText(_gridValues[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(_cellSize, _cellSize) * 0.5f, 10, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, 100f);
    }

    public float CellSize
    {
        get => _cellSize;
    }

    public ColorScale Scale
    {
        get => _colorScale;
    }

    public int Width
    {
        get => _width;
    }

    public int Height
    {
        get => _height;
    }

    public int this[int x, int y]
    {
        get
        {
            return _gridValues[x, y];
        }

        set
        {
            if (value >= _minValue && value <= _maxValue)
            {
                _gridValues[x, y] = value;
                _textMeshes[x, y].text = value.ToString();
            }
        }
    }

    public int this[Vector3 position]
    {
        get
        {
            GetXY(position, out var x, out var y);

            return this[x, y];
        }

        set
        {
            GetXY(position, out var x, out var y);

            this[x, y] = value;
        }
    }

    private static TextMesh CreateWorldText(
        string text,
        Transform parent = default,
        Vector3 localPoition = default,
        int fontsize = 40,
        Color color = default,
        TextAnchor textAnchor = default,
        int sortingOrder = default)
    {
        var gameObject = new GameObject("WorldText", typeof(TextMesh));
        gameObject.transform.SetParent(parent, false);
        gameObject.transform.localPosition = localPoition;

        var textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = fontsize;
        textMesh.anchor = textAnchor;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMesh;
    }

    private void GetXY(Vector3 position, out int x, out int y)
    {
        x = Mathf.FloorToInt((position - _origin).x / _cellSize);
        y = Mathf.FloorToInt((position - _origin).y / _cellSize);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _origin;
    }
}
