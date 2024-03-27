using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Text;
using System.Threading.Tasks;

public class GridScaler : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Transform _tilemapTransform;
    [SerializeField] private Grid _grid;
    [SerializeField] private Transform _gridTransform;
    [SerializeField] private Camera _camera;
    [SerializeField, Range(0, 1)] private float _widthFillPercentage;
    [SerializeField, Range(0, 1)] private float _heightFillPercentage;

    private bool _inited;
    private Vector3Int _minSlot;
    private Vector3Int _maxSlot;
    private int _screenWidth;
    private int _screenHeight;

    public void ScaleGrid(Vector2Int minSlot, Vector2Int maxSlot)
    {
        _inited = true;
        _minSlot = new Vector3Int(minSlot.x, minSlot.y, 0);
        _maxSlot = new Vector3Int(maxSlot.x + 1, maxSlot.y + 1, 0);

        ScaleGrid();
        CenterGrid();
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
    }

    private void FixedUpdate()
    {
        if (!_inited || (Screen.width == _screenWidth && Screen.height == _screenHeight))
        {
            return;
        }

        ScaleGrid();
        CenterGrid();
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;
    }

    private void CenterGrid()
    {
        var origin = _grid.CellToWorld(Vector3Int.zero) + ( _tilemapTransform.position - _gridTransform.position);
        var gridCenterForCamera = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Mathf.Abs(origin.z - _camera.transform.position.z)));
        _grid.transform.position += gridCenterForCamera - (_tilemapTransform.position - _gridTransform.position) - origin;
    }

    private void ScaleGrid()
    {
        var worldMin = _grid.CellToWorld(_minSlot);
        var worldMax = _grid.CellToWorld(_maxSlot);
        worldMin.z = _camera.transform.position.z;
        worldMax.z = _camera.transform.position.z;
        var viewPortMin = _camera.WorldToViewportPoint(worldMin);
        var viewPortMax = _camera.WorldToViewportPoint(worldMax);

        var viewPortSize = viewPortMax - viewPortMin;
        var scaleFactor = 0f;

        if (viewPortSize.x > viewPortSize.y)
        {
            scaleFactor = _widthFillPercentage / viewPortSize.x;
        }
        else
        {
            scaleFactor = _heightFillPercentage / viewPortSize.y;
        }

        _gridTransform.localScale *= scaleFactor;
    }
}
