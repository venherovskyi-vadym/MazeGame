using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.MazeGeneration
{
    public class MazeGenerationTester : MonoBehaviour
    {
        [Range(3,15), SerializeField] private int _size = 3;
        [Range(1, 5), SerializeField] private int _exits = 3;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileSprites tileSprites;
        private MazePatternGenerator _generator = new MazePatternGenerator();
        private TileIndexProvider _tileIndexProvider = new TileIndexProvider();
        private MazePassageGenerator _passageGenerator = new MazePassageGenerator();
        private MazeExitGenerator _exitGenerator = new MazeExitGenerator();

        [ContextMenu("GeneratePattern")]
        private void GeneratePattern()
        {
            var generatedPattern = _generator.GeneratePatternLinks(_size);
            _passageGenerator.GeneratePassages(generatedPattern);
            _exitGenerator.GenerateExits(generatedPattern, _exits);
            var mazeField = new MazeField(generatedPattern);

            foreach (var item in generatedPattern)
            {
                Debug.Log(item);
            }

            _tilemap.ClearAllTiles();

            for (int x = mazeField.MinPosition.x - 1; x <= mazeField.MaxPosition.x + 1; x++)
            {
                for (int y = mazeField.MinPosition.y - 1; y <= mazeField.MaxPosition.y + 1; y++)
                {
                    var position = new Vector2Int(x, y);
                    _tilemap.SetTile(new Vector3Int(x, y, 0), tileSprites.GetTile(_tileIndexProvider.GetIndex(position, mazeField)));
                }
            }

            _tilemap.RefreshAllTiles();
        }
    }
}
