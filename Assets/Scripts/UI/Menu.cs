using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Menu : MonoBehaviour
{
    [SerializeField] private Slider _mazeSizeSlider;
    [SerializeField] private Slider _mazeExitsSlider;
    [SerializeField] private Button _playButton;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileSprites tileSprites;
    [SerializeField] private GridScaler _gridScaler;
    private MazePatternGenerator _generator = new MazePatternGenerator();
    private TileIndexProvider _tileIndexProvider = new TileIndexProvider();
    private MazePassageGenerator _passageGenerator = new MazePassageGenerator();
    private MazeExitGenerator _exitGenerator = new MazeExitGenerator();

    private void Awake()
    {
        _playButton.onClick.AddListener(GenerateMaze);
        Application.targetFrameRate = 40;
    }

    private void GenerateMaze()
    {
        var generatedPattern = _generator.GeneratePatternLinks(_mazeSizeSlider.Value);
        _passageGenerator.GeneratePassages(generatedPattern);
        _exitGenerator.GenerateExits(generatedPattern, _mazeExitsSlider.Value);
        var mazeField = new MazeField(generatedPattern);

        _tilemap.ClearAllTiles();

        for (int x = mazeField.MinPosition.x - 1; x <= mazeField.MaxPosition.x; x++)
        {
            for (int y = mazeField.MinPosition.y; y <= mazeField.MaxPosition.y + 1; y++)
            {
                var position = new Vector2Int(x, y);
                _tilemap.SetTile(new Vector3Int(x, y, 0), tileSprites.GetTile(_tileIndexProvider.GetIndex(position, mazeField)));
            }
        }

        _tilemap.RefreshAllTiles();
        _gridScaler.ScaleGrid(mazeField.MinPosition, mazeField.MaxPosition);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(GenerateMaze);
    }
}
