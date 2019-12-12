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
    public GameObject Score;
    public GameObject PausePanel;
    public GameObject StarsPanel;

    public int ActivatedStar = 0;
    private bool _isRotating = false;

    private List<GameObject> _starsArray;

    public void ActivateGameUI()
    {
        Score.SetActive(true);
        StrengthSlider.gameObject.SetActive(true);
        Catapult.SetActive(true);
        RotatePositiveButton.SetActive(true);
        RotateNegativeButton.SetActive(true);
        StarsPanel.SetActive(true);

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
        if (!_isRotating) StartCoroutine(RotateCouroutine(90f, 1f));
    }

    public void RotateNegativeLevel()
    {
        if (!_isRotating) StartCoroutine(RotateCouroutine(-90f, 1f));
    }

    private IEnumerator RotateCouroutine(float angleYRotation, float time)
    {
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
        _isRotating = false;
    }
    #endregion LevelRotateRegion

}
