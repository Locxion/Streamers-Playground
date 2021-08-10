using Assets;
using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public GameManager _gameManager;
    public NavMeshAgent _navMeshAgent;
    public GameObject _player;
    public LineRenderer _lineRender;

    // Update is called once per frame
    private void Update()
    {
        if (_gameManager._gameState == GameStateEnum.Running && _gameManager._gameMode == GameModeEnum.PathFinding)
        {
            _navMeshAgent.SetDestination(_player.transform.position);

            DrawPath(_navMeshAgent.path);
        }

        void DrawPath(NavMeshPath path)
        {
            // If the path has 1 or no corners, there is no need...
            if (path.corners.Length < 2)
            {
                return;
            }

            // Set the array of positions to the amount of corners...
            _lineRender.positionCount = path.corners.Length;
            for (int i = 1; i < path.corners.Length; i++)
            {
                // Go through each corner and set that to the line renderer's position...
                _lineRender.SetPosition(i, path.corners[i]);
            }
        }
    }
}