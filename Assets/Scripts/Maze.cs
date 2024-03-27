using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Text;
using System.Threading.Tasks;

public class Maze : MonoBehaviour
{
    [SerializeField] private string _horizontalAxis = "Horizontal";
    [SerializeField] private string _verticalAxis = "Vertical";
    [SerializeField] private float _cursorSpeed;
    [SerializeField] private float _cursorSpeedForRigidBody = 1000;
    [SerializeField] private Transform _cursor;
    [SerializeField] private Rigidbody _cursorRigidBody;
    [SerializeField] private TextMeshProUGUI _timer;
    private MazeField _mazeField;
    private Vector3 _cursorPosition;

    public void StartMaze(MazeField mazeField)
    {
        _mazeField = mazeField;
    }

    private void Update()
    {
        var inputVector = new Vector3(Input.GetAxis(_horizontalAxis), Input.GetAxis(_verticalAxis));
        _cursorPosition += inputVector * Time.deltaTime * _cursorSpeed;

        if (_cursor != null)
        {
            _cursor.transform.position = _cursorPosition;
        }

        if (_cursorRigidBody != null)
        {
            _cursorRigidBody.velocity = inputVector * Time.deltaTime * _cursorSpeedForRigidBody;
        }
    }
}
