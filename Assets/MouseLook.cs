using Assets;
using Assets.Scripts.Enum;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameManager _gameManager;

    public float mouseSensitivity = 100f;

    public Transform player;

    private float xRotation = 0f;

    // Start is called before the first frame update
    private void Start()
    {
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_gameManager._gameState == GameStateEnum.PathfinderRunning)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            player.Rotate(Vector3.up * mouseX);
        }
    }
}