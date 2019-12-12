using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject[] levelPrefabArcade;
    public GameObject[] levelPrefabTime;

    private GameObject _currentLevel; 
    
    // Start is called before the first frame update
    void Start()
    {
        int _levelId = PlayerPrefs.GetInt(ControllerMenu.LEVEL_INDEX_KEY, -1);
        string _gameMode = PlayerPrefs.GetString(ControllerMenu.GAME_MODE, string.Empty);

        if (_levelId >= 0 && !string.IsNullOrEmpty(_gameMode))
        {
            if (_gameMode == ControllerMenu.ARCADE_GAME_MODE)
            {
                _currentLevel = Instantiate(levelPrefabArcade[_levelId], transform);
            }
            else if (_gameMode == ControllerMenu.TIME_GAME_MODE)
            {
                _currentLevel = Instantiate(levelPrefabTime[_levelId], transform);
            }
        }
    }

}
