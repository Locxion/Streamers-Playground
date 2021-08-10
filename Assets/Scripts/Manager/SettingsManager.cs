using Assets.Scripts;
using Assets.Scripts.Objects;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public GameSettings CurrentGameSettings;

    // Start is called before the first frame update
    private void Start()
    {
        if (File.Exists(Constants.SettingsPath))
        {
            Debug.Log("Found Settings File ... Loading ...");
            CurrentGameSettings = LoadSettings();
        }
        else
        {
            Debug.Log("No Settings File found ... Create Blank Settings instead ...");
            Directory.CreateDirectory(Constants.ProgramfilesfolderPath);
            File.Create(Constants.SettingsPath);
            CurrentGameSettings = new GameSettings();
        }
    }

    public GameSettings LoadSettings()
    {
        return JsonConvert.DeserializeObject<GameSettings>(File.ReadAllText(Constants.SettingsPath));
    }

    public void SaveSettings()
    {
        File.WriteAllText(Constants.SettingsPath, JsonConvert.SerializeObject(CurrentGameSettings, Formatting.Indented));
    }
}