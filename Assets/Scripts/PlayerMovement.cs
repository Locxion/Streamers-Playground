using Assets;
using Assets.Scripts.Enum;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController _controller;
    public GameManager _gameManager;
    public Transform _groundCheck;
    public float _groundDistance = 0.4f;
    public LayerMask _groundMask;

    public float _speed = 12f;
    public float _gravitiy = -9.81f;
    public float _jumpHeight = 3f;

    private Vector3 velocity;
    private bool isGrounded;

    // Update is called once per frame
    private void Update()
    {
        if (_gameManager._gameState == GameStateEnum.Running)
        {
            isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            _controller.Move(move * _speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravitiy);
            }

            velocity.y += _gravitiy * Time.deltaTime;

            _controller.Move(velocity * Time.deltaTime);
        }
    }
}