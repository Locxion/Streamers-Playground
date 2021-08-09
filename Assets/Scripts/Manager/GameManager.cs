using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public GameStateEnum _gameState;
        public SettingsManager _settingsManager;

        // Start is called before the first frame update
        private void Start()
        {
            _gameState = GameStateEnum.InMenu;
        }

        public void Update()
        {
            if (_gameState == GameStateEnum.Running)
            {
            }
        }

        public void StartGame()
        {
            _gameState = GameStateEnum.Running;
        }

        public void StopGame()
        {
            _gameState = GameStateEnum.Stopped;
        }

        public void ResetGame()
        {
        }
    }
}