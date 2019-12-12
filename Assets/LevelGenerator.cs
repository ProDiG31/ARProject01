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
        int levelId = PlayerPrefs.GetInt(ControllerMenu.LEVEL_INDEX_KEY, 0);

        if(levelId != 0)
        {
            _currentLevel = Instantiate(levelPrefabArcade[levelId], transform);
        }
    }

}
