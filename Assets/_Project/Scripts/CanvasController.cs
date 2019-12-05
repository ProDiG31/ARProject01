using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{

    private bool _isRotating = false; 

    public void LoadStartMenuScene()
    {
        SceneManager.LoadScene("StartMenu");
    }

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

}
