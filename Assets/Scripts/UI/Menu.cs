using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Menu : MonoBehaviour
{
    [SerializeField] private Slider _mazeSizeSlider;
    [SerializeField] private Slider _mazeExitsSlider;
    [SerializeField] private Button _playButton;
    [SerializeField] private Maze _maze;

    private void Awake()
    {
        _playButton.onClick.AddListener(GenerateMaze);
        Application.targetFrameRate = 40;
    }

    private void GenerateMaze()
    {
        _maze.StartMaze(_mazeSizeSlider.Value, _mazeExitsSlider.Value);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(GenerateMaze);
    }
}
