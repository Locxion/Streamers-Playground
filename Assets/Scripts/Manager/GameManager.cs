using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.AI;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public GameStateEnum _gameState;
        public SettingsManager _settingsManager;
        public GameObject __navMeshSurface;

        // Start is called before the first frame update
        private void Start()
        {
            _gameState = GameStateEnum.InMenu;
        }

        public void Update()
        {
            if (_gameState == GameStateEnum.PathfinderRunning)
            {
                var navMesh = __navMeshSurface.GetComponent<NavMeshSurface>();
                navMesh.BuildNavMesh();
            }
        }

        public void StartGame()
        {
            _gameState = GameStateEnum.PathfinderRunning;
        }

        public void StopGame()
        {
            _gameState = GameStateEnum.Stopped;
        }

        public void PauseGame()
        {
            _gameState = GameStateEnum.Paused;
        }

        public void ResetGame()
        {
        }
    }
}