using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string _levelName;

    public GameObject _optionsPanel;

    public GameObject _fadeToBlack;

    public void StartGame()
    {
        _fadeToBlack.SetActive(true);
        SceneManager.LoadScene(_levelName);
    }
    public void OpenOptions()
    {
        _optionsPanel.SetActive(true);
    }
    public void CloseOptions()
    {
        _optionsPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
