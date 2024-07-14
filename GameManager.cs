using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject _optionsMenu;

    //Only enable the character and stuff when this is 0
    private int _optionsLayer;

    public GameObject _player;

    [SerializeField] private TMP_Text _fps;

    private int _frames;
    private int _framesBeforeMeasuring = 6;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenOptions();
            Time.timeScale = 0;
        }
        if(_optionsLayer > 0)
        {
            StopPlaying();
        }
        else
        {
            StartPlayingAgain();
        }
        if (_frames >= _framesBeforeMeasuring)
            _fps.text = (1.0f / Time.deltaTime).ToString("F0");
        else _frames++;
    }
    void StopPlaying()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _player.SetActive(false);
    }
    void StartPlayingAgain()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _player.SetActive(true);
    }
    public void OpenOptions()
    {
        if (_optionsMenu.activeSelf)
        {
            _optionsLayer--;
            _optionsMenu.SetActive(false);
        }
        else
        {
            _optionsLayer++;
            _optionsMenu.SetActive(true);
        }
    }
    public void GoToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
