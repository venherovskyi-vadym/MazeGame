using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.MazeGeneration
{
    public class MazeGenerationTester : MonoBehaviour
    {
        [Range(3,15), SerializeField] private int _size = 3;
        [SerializeField] private Tilemap _tilemap;

        private MazePatternGenerator _generator = new MazePatternGenerator();

        [ContextMenu("GeneratePattern")]
        private void GeneratePattern()
        {
            var generatedPattern = _generator.GeneratePatternLinks(_size);
            foreach (var item in generatedPattern)
            {
                Debug.Log(item);
            }

            //_tilemap.SetTile
        }
    }
}
