using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.AI;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public GameStateEnum _gameState;
        public GameModeEnum _gameMode;
        public SettingsManager _settingsManager;
        public GameObject __navMeshSurface;

        // Start is called before the first frame update
        private void Start()
        {
            _gameState = GameStateEnum.InMenu;
        }

        public void Update()
        {
            if (_gameState == GameStateEnum.Running && _gameMode == GameModeEnum.PathFinding)
            {
                var navMesh = __navMeshSurface.GetComponent<NavMeshSurface>();
                navMesh.BuildNavMesh();
            }
        }

        public void StartGame(string mode)
        {
            switch (mode)
            {
                case "pf":
                    StartGame(GameModeEnum.PathFinding);
                    break;

                case "td":
                    StartGame(GameModeEnum.TowerDefence);
                    break;
            }
        }

        private void StartGame(GameModeEnum mode)
        {
            switch (mode)
            {
                case GameModeEnum.PathFinding:
                    _gameMode = GameModeEnum.PathFinding;
                    _gameState = GameStateEnum.Running;
                    var player = GameObject.Find("PathFindingPlayer");
                    player.SetActive(true);
                    var camera = player.GetComponent<Camera>();
                    camera.enabled = true;
                    var mouseLook = player.GetComponent<MouseLook>();
                    mouseLook.LockCursor();

                    var enemy = GameObject.Find("PathFindingEnemy");
                    enemy.SetActive(true);
                    break;

                case GameModeEnum.TowerDefence:
                    _gameMode = GameModeEnum.TowerDefence;
                    _gameState = GameStateEnum.Running;
                    break;
            }
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