using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.MazeGeneration
{
    public class MazeGenerationTester : MonoBehaviour
    {
        [Range(3,15), SerializeField] private int _size = 3;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileSprites tileSprites;
        private MazePatternGenerator _generator = new MazePatternGenerator();
        private TileIndexProvider _tileIndexProvider = new TileIndexProvider();

        [ContextMenu("GeneratePattern")]
        private void GeneratePattern()
        {
            var generatedPattern = _generator.GeneratePatternLinks(_size);
            var mazeField = new MazeField(generatedPattern);

            foreach (var item in generatedPattern)
            {
                Debug.Log(item);
            }

            _tilemap.ClearAllTiles();

            for (int x = -_size; x < _size; x++)
            {
                for (int y = -_size; y < _size; y++)
                {
                    var position = new Vector2Int(x, y);
                    _tilemap.SetTile(new Vector3Int(x, y, 0), tileSprites.GetTile(_tileIndexProvider.GetIndex(position, mazeField)));
                }
            }

            _tilemap.RefreshAllTiles();
        }
    }
}
