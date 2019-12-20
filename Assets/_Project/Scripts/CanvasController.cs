using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public Slider StrengthSlider;
    public Image SliderInfill;

    public GameObject Catapult;
    public GameObject HandAnimationPreview;
    public GameObject RotatePositiveButton;
    public GameObject RotateNegativeButton;
    public GameObject MovePlusButton;
    public GameObject MoveMinusButton;
    public GameObject Score;
    public GameObject PausePanel;
    public GameObject StarsPanel;

    public Vector3 initialLevelPosition; 

    public int ActivatedStar = 0;
    private bool _isRotating = false;
    private bool _isMoving = false;
    private float _MovingProgression = 0;

    private List<GameObject> _starsArray;

    public void ActivateGameUI()
    {
        Score.SetActive(true);
        StrengthSlider.gameObject.SetActive(true);
        Catapult.SetActive(true);
        RotatePositiveButton.SetActive(true);
        RotateNegativeButton.SetActive(true);
        StarsPanel.SetActive(true);
        MovePlusButton.SetActive(true);
        MoveMinusButton.SetActive(true);

        HandAnimationPreview.SetActive(false);
        PausePanel.SetActive(false);
    }

    public void DisactiveGameUI()
    {
        Score.SetActive(false);
        StrengthSlider.gameObject.SetActive(false);
        Catapult.SetActive(false);
        RotatePositiveButton.SetActive(false);
        RotateNegativeButton.SetActive(false);
        PausePanel.SetActive(false);
        StarsPanel.SetActive(false);
        MovePlusButton.SetActive(false);
        MoveMinusButton.SetActive(false);
    }

    public void DisableAllStar()
    {
        _starsArray = new List<GameObject>();
        foreach(Transform star in StarsPanel.GetComponentsInChildren<Transform>())
        {
            _starsArray.Add(star.gameObject);
            star.gameObject.SetActive(false);
        }
    }

    public void ActivateOneStar()
    {
        if(ActivatedStar < 3)
        {
            foreach (GameObject star in _starsArray)
            {
                if (!star.activeSelf)
                {
                    star.SetActive(true);
                    ActivatedStar++;
                    return;
                }
            }
        }
        
    }

    public void UpdateStrengthSlider(float str)
    {
        StrengthSlider.value = str;
        SliderInfill.color = Color.Lerp(Color.grey, Color.red, str);
    }

    public void LoadStartMenuScene()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    #region LevelRotateRegion
    public void RotatePositiveLevel()
    {
        if (!_isRotating) StartCoroutine(RotateCoroutine(90f, 1f));
    }

    public void RotateNegativeLevel()
    {
        if (!_isRotating) StartCoroutine(RotateCoroutine(-90f, 1f));
    }

    private IEnumerator RotateCoroutine(float angleYRotation, float time)
    {
        RotatePositiveButton.GetComponent<Button>().interactable = false;
        RotateNegativeButton.GetComponent<Button>().interactable = false;

        _isRotating = true;
        Quaternion fromRotation = ControllerPlane.levelCreated.transform.rotation;
        Quaternion toRotation = Quaternion.Euler(fromRotation.eulerAngles + new Vector3(0, angleYRotation, 0));
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            ControllerPlane.levelCreated.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        RotatePositiveButton.GetComponent<Button>().interactable = true;
        RotateNegativeButton.GetComponent<Button>().interactable = true;

        _isRotating = false;
    }
    #endregion LevelRotateRegion

    public void MoveInLevel()
    {
        if (!_isMoving) StartCoroutine(MoveCoroutine(.1f, 1f));
    }

    public void MoveOutLevel()
    {
        if (!_isMoving) StartCoroutine(MoveCoroutine(-.1f, 1f));
    }

    private IEnumerator MoveCoroutine(float step, float time)
    {
        _isMoving = true;

        //initialLevelPosition = ControllerPlane.levelCreated.transform.position; 
        MovePlusButton.GetComponent<Button>().interactable = false;
        MoveMinusButton.GetComponent<Button>().interactable = false;
        Vector3 targetedPosition = Camera.main.transform.position;

        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            // lerp 0 => 5 / -5 
            float progress = Mathf.Lerp(_MovingProgression, _MovingProgression + step, (elapsedTime / time));
            ControllerPlane.levelCreated.transform.position = Vector3.Lerp(initialLevelPosition, targetedPosition, progress);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        MovePlusButton.GetComponent<Button>().interactable = true;
        MoveMinusButton.GetComponent<Button>().interactable = true;
        _MovingProgression += step;

        _isMoving = false;
    }

}
