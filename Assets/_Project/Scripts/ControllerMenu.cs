using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMenu : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        //SceneManager.LoadScene("GameScene");
        SceneManager.LoadScene(levelName);
    }
}
