using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System.Text;
using System.Threading.Tasks;
using System;

public class Maze : MonoBehaviour
{
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Vertical";
    [SerializeField] private float _cursorSpeed = 1;
    [SerializeField, Range(0.1f,0.9f)] private float _cursorMaxDistanceFromCellCenterPercentage = 0.3f;
    [SerializeField] private Transform _cursor;
    [SerializeField] private GameObject _winFx;
    [SerializeField] private TextMeshProUGUI _timer;
    [SerializeField] private TextMeshProUGUI _gridPosition;
    [SerializeField] private TextMeshProUGUI _localPositionInCell;
    [SerializeField] private TextMeshProUGUI _minMaxClampValues;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileSprites tileSprites;
    [SerializeField] private GridScaler _gridScaler;

    private MazePatternGenerator _generator = new MazePatternGenerator();
    private TileIndexProvider _tileIndexProvider = new TileIndexProvider();
    private MazePassageGenerator _passageGenerator = new MazePassageGenerator();
    private MazeExitGenerator _exitGenerator = new MazeExitGenerator();

    private MazeField _mazeField;
    private Vector3 _cursorPosition;
    private Vector3 _cellSize;

    private float _time;
    private bool _exitReached;

    public void StartMaze(int mazeSize, int exitCount)
    {
        _time = 0;
        _exitReached = false;
        _winFx.SetActive(false);
        var generatedPattern = _generator.GeneratePatternLinks(mazeSize);
        _passageGenerator.GeneratePassages(generatedPattern);
        var exits = _exitGenerator.GenerateExits(generatedPattern, exitCount);
        _mazeField = new MazeField(generatedPattern, exits);

        _tilemap.ClearAllTiles();

        for (int x = _mazeField.MinPosition.x - 1; x <= _mazeField.MaxPosition.x; x++)
        {
            for (int y = _mazeField.MinPosition.y; y <= _mazeField.MaxPosition.y + 1; y++)
            {
                var position = new Vector2Int(x, y);
                _tilemap.SetTile(new Vector3Int(x, y, 0), tileSprites.GetTile(_tileIndexProvider.GetIndex(position, _mazeField)));
            }
        }

        _tilemap.RefreshAllTiles();
        _gridScaler.ScaleGrid(_mazeField.MinPosition, _mazeField.MaxPosition);
        _cellSize = _tilemap.layoutGrid.CellToWorld(new Vector3Int(1, 1)) - _tilemap.layoutGrid.CellToWorld(Vector3Int.zero);
        _cursorPosition = _tilemap.layoutGrid.CellToWorld(Vector3Int.up) + (_tilemap.transform.position - _tilemap.layoutGrid.transform.position);
        _cursor.position = _cursorPosition;
    }

    private void Update()
    {
        if (!_exitReached)
        {
            _time += Time.deltaTime;
            var timeSpan = TimeSpan.FromSeconds(_time);
            _timer.text = $"{(int)timeSpan.TotalMinutes}:{timeSpan.TotalSeconds:0.0}";
        }

        var inputVector = new Vector3(Input.GetAxis(_horizontalAxis), Input.GetAxis(_verticalAxis));

        _cursorPosition = ClampCursorPositionToMaze(_cursorPosition, inputVector * Time.deltaTime * _cursorSpeed);
        var gridPos = GetGridPosition(_cursorPosition);
        var cellWorldPos = GetWorldPositionOfCell(gridPos);
        _cursor.position = _cursorPosition;
        _localPositionInCell.text = (_cursorPosition - cellWorldPos).ToString();
        _gridPosition.text = gridPos.ToString();

        if (_exitReached)
        {
            return;
        }

        foreach (var item in _mazeField.ExitSlotLinks)
        {
            if (item.End == new Vector2Int(gridPos.x, gridPos.y))
            {
                _exitReached = true;
                _winFx.SetActive(true);
            }
        }
    }

    private Vector3Int GetGridPosition(Vector3 position)
    {
        return _tilemap.layoutGrid.WorldToCell(position - (_tilemap.transform.position - _tilemap.layoutGrid.transform.position) + _cellSize / 2) - Vector3Int.up;
    }

    private Vector3 GetWorldPositionOfCell(Vector3Int cellPos)
    {
        return _tilemap.layoutGrid.CellToWorld(cellPos + Vector3Int.up) + (_tilemap.transform.position - _tilemap.layoutGrid.transform.position);
    }

    private Vector3 GetWorldPositionOfCellNoOffset(Vector3Int cellPos)
    {
        return _tilemap.layoutGrid.CellToWorld(cellPos) + (_tilemap.transform.position - _tilemap.layoutGrid.transform.position);
    }

    private Vector3 GetWorldPositionOfCell(Vector2Int cellPos)
    {
        return _tilemap.layoutGrid.CellToWorld(new Vector3Int(cellPos.x, cellPos.y) + Vector3Int.up) + (_tilemap.transform.position - _tilemap.layoutGrid.transform.position);
    }

    private Vector3 ClampCursorPositionToMaze(Vector3 position, Vector3 delta)
    {
        var newPosition = position + delta;
        var gridPos = GetGridPosition(position);

        Vector3Int verticalAreaMin = gridPos;
        Vector3Int verticalAreaMax = gridPos;
        Vector3Int horizontalAreaMin = gridPos;
        Vector3Int horizontalAreaMax = gridPos;

        var mazePos = new Vector2Int(gridPos.x, gridPos.y);

        if(_mazeField.SlotLinksByStart.ContainsKey(mazePos))
        foreach (var item in _mazeField.SlotLinksByStart[mazePos])
        {
            if (item.Direction.x == 0)
            {
                verticalAreaMin.y = Mathf.Min(verticalAreaMin.y, item.End.y);
                verticalAreaMax.y = Mathf.Max(verticalAreaMax.y, item.End.y);
            }
            if (item.Direction.y == 0)
            {
                horizontalAreaMin.x = Mathf.Min(horizontalAreaMin.x, item.End.x);
                horizontalAreaMax.x = Mathf.Max(horizontalAreaMax.x, item.End.x);
            }
        }

        if(_mazeField.SlotLinksByEnd.ContainsKey(mazePos))
        foreach (var item in _mazeField.SlotLinksByEnd[mazePos])
        {
            if (item.Direction.x == 0)
            {
                verticalAreaMin.y = Mathf.Min(verticalAreaMin.y, item.Start.y);
                verticalAreaMax.y = Mathf.Max(verticalAreaMax.y, item.Start.y);
            }
            if (item.Direction.y == 0)
            {
                horizontalAreaMin.x = Mathf.Min(horizontalAreaMin.x, item.Start.x);
                horizontalAreaMax.x = Mathf.Max(horizontalAreaMax.x, item.Start.x);
            }
        }

        Vector3 verticalClampAreaMin = GetWorldPositionOfCell(verticalAreaMin) - _cellSize * _cursorMaxDistanceFromCellCenterPercentage / 2;
        Vector3 verticalClampAreaMax = GetWorldPositionOfCell(verticalAreaMax) + _cellSize * _cursorMaxDistanceFromCellCenterPercentage / 2;
        Vector3 horizontalClampAreaMin = GetWorldPositionOfCell(horizontalAreaMin) - _cellSize * _cursorMaxDistanceFromCellCenterPercentage / 2;
        Vector3 horizontalClampAreaMax = GetWorldPositionOfCell(horizontalAreaMax) + _cellSize * _cursorMaxDistanceFromCellCenterPercentage / 2;

        _minMaxClampValues.text = $"vMin:{verticalAreaMin}{Environment.NewLine}vMax:{verticalAreaMax}{Environment.NewLine}hMin:{horizontalAreaMin}{Environment.NewLine}hMax:{horizontalAreaMax}";

        var cellWorldPos = GetWorldPositionOfCell(gridPos);
        var localPositionInCell = _cursorPosition - cellWorldPos;
        var result = newPosition;

        if (Mathf.Abs(Mathf.Abs(localPositionInCell.x) - Mathf.Abs(localPositionInCell.y)) > 0.001f)
        {
            result = ClampDependFromAxis(newPosition, verticalClampAreaMin, verticalClampAreaMax, horizontalClampAreaMin, horizontalClampAreaMax, localPositionInCell);
        }
        else
        {
            result = ClampDependFromAxis(newPosition, verticalClampAreaMin, verticalClampAreaMax, horizontalClampAreaMin, horizontalClampAreaMax, delta);
        }

        return result;
    }

    private Vector3 ClampDependFromAxis(
        Vector3 original, 
        Vector3 verticalClampAreaMin, 
        Vector3 verticalClampAreaMax, 
        Vector3 horizontalClampAreaMin,
        Vector3 horizontalClampAreaMax, 
        Vector3 dependVector)
    {
        var result = original;

        if (Mathf.Abs(dependVector.x) > Mathf.Abs(dependVector.y))
        {
            result = Clamp(original, verticalClampAreaMin, verticalClampAreaMax);
            result = Clamp(original, horizontalClampAreaMin, horizontalClampAreaMax);
        }
        else
        {
            result = Clamp(original, horizontalClampAreaMin, horizontalClampAreaMax);
            result = Clamp(original, verticalClampAreaMin, verticalClampAreaMax);
        }

        return result;
    }

    private Vector3 Clamp(Vector3 original, Vector3 clampMin, Vector3 clampMax)
    {
        original.y = Mathf.Clamp(original.y, clampMin.y, clampMax.y);
        original.x = Mathf.Clamp(original.x, clampMin.x, clampMax.x);

        return original;
    }
}
