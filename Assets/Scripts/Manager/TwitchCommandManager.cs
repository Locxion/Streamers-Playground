using Assets;
using Assets.Scripts.Manager;
using TwitchLib.Client.Models;
using UnityEngine;

public class TwitchCommandManager : MonoBehaviour
{
    public TwitchManager _twitchManager;
    public GameManager _GameManager;

    public string CommandPrefix = "!";
    public bool ModeratorCanIgnoreCooldown = false;

    public void Start()
    {
        _twitchManager.OnChatMessageRecieved += _twitchManager_OnChatMessageRecieved;
    }

    private void _twitchManager_OnChatMessageRecieved(ChatMessage message)
    {
        if (!CheckForCommand(message.Message))
        {
        }

        switch (message.Message)
        {
            case "!jump":

                break;
        }
    }

    private bool CheckForCommand(string message)
    {
        if (message.StartsWith(CommandPrefix))
            return true;

        return false;
    }
}