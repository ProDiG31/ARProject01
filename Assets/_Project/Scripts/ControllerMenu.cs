using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenu : MonoBehaviour
{
    public const string GAME_LEVEL_NAME = "GameScene";
    public const string LEVEL_INDEX_KEY = "LevelIndex";
    public const string GAME_MODE = "GameMode";

    public const string ARCADE_GAME_MODE = "ArcadeGameMode";
    public const string TIME_GAME_MODE = "TimeGameMode";

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadGameArcadeLevel(int levelIndex)
    {
        PlayerPrefs.SetString(GAME_MODE, ARCADE_GAME_MODE);
        PlayerPrefs.SetInt(LEVEL_INDEX_KEY, levelIndex);
        LoadLevel(GAME_LEVEL_NAME);
    }

    public void LoadGameTimeLevel(int levelIndex)
    {
        PlayerPrefs.SetString(GAME_MODE, TIME_GAME_MODE);
        PlayerPrefs.SetInt(LEVEL_INDEX_KEY, levelIndex);
        LoadLevel(GAME_LEVEL_NAME);
    }


}