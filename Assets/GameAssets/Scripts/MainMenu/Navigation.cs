using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject optionsButton;

    public Animator cameraMove;

    public void ChangeScene(string sceneName)       //Used for the navigation between scenes
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

    public void Quit()      //Used to quit the application
    {
        Application.Quit();
    }

    //In game options
    public void OptionsOpen()
    {
        optionsButton.SetActive(false);
        optionsPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void OptionsClose()
    {
        optionsButton.SetActive(true);
        optionsPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    //IEnumerator used to allow for smooth animation between menus
    public void CameraMoveToOptions()
    {
        StartCoroutine(CanmeraMovement());
    }
    public void CameraMoveToMenu()
    {
        StartCoroutine(CanmeraMovement2());
    }

    IEnumerator CanmeraMovement()
    {
        mainMenuPanel.SetActive(false);
        cameraMove.Play("optionsCameraMove");
        yield return new WaitForSeconds(2);
        optionsPanel.SetActive(true);
    }
    IEnumerator CanmeraMovement2()
    {
        optionsPanel.SetActive(false);
        cameraMove.Play("optionsCameraMoveBack");
        yield return new WaitForSeconds(2);
        mainMenuPanel.SetActive(true);
    }
}

