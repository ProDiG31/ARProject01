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
        LoadLevelIntoContainer();
        RetreiveGoalReward();
    }

    private void LoadLevelIntoContainer()
    {
        int _levelId = PlayerPrefs.GetInt(ControllerMenu.LEVEL_INDEX_KEY, -1);
        string _gameMode = PlayerPrefs.GetString(ControllerMenu.GAME_MODE, string.Empty);

        if (_levelId >= 0 && !string.IsNullOrEmpty(_gameMode))
        {
            switch (_gameMode)
            {
                case ControllerMenu.ARCADE_GAME_MODE:
                    _currentLevel = Instantiate(levelPrefabArcade[_levelId], transform);
                    break;
                case ControllerMenu.TIME_GAME_MODE:
                    _currentLevel = Instantiate(levelPrefabTime[_levelId], transform);
                    break;
                default:
                    break;
            }
        }
    }

    private void RetreiveGoalReward()
    {
        if (_currentLevel)
        {
            int RewardSum = 0;
            foreach (DestructibleProps props in _currentLevel.GetComponentsInChildren<DestructibleProps>())
            {
                RewardSum += props.Reward;
            }

            ControllerPlane.maxRewardPoint = RewardSum;
        }
    }


}
