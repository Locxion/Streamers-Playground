using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        public SettingsManager _settingsManager;

        public void StartGame()
        {
        }

        public void QuitGame()
        {
            Debug.Log("Quit game...");
            Application.Quit();
        }

        public void LoadSettings()
        {
            _settingsManager.LoadSettings();

            var inputFieldObject = GameObject.Find("UsernameInputField");
            var inputField = inputFieldObject.GetComponent<TMP_InputField>();
            inputField.text = _settingsManager.CurrentGameSettings.Username;
        }

        public void SaveSettings()
        {
            var inputFieldObject = GameObject.Find("UsernameInputField");
            var inputField = inputFieldObject.GetComponent<TMP_InputField>();
            _settingsManager.CurrentGameSettings.Username = inputField.text;

            inputFieldObject = GameObject.Find("AuthTokenInputField");
            inputField = inputFieldObject.GetComponent<TMP_InputField>();
            _settingsManager.CurrentGameSettings.AccessToken = inputField.text;

            _settingsManager.SaveSettings();
        }
    }
}