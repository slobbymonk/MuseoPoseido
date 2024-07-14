using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Managers it knows about
    private RoomManager _roomManager;
    [SerializeField] private ArtifactManager _artifactManager;

    [SerializeField] GameObject _winPanel;
    [SerializeField] TMP_Text _score;

    [Header("Stars")]
    [SerializeField] GameObject[] _starImageArray;
    [SerializeField] private float _threeStarArtifactThreshold;
    [SerializeField] private float _twoStarArtifactThreshold;
    [SerializeField] private float _oneStarArtifactThreshold;

    [Header("Diegetic UI")]
    public GameObject[] _scoreCardStars;

    private int _highscore;

    //Stops you from automatically winning
    private bool _enabled;

    void Awake()
    {
        _roomManager = GetComponent<RoomManager>();

        _roomManager.StartedRoom += EnableEverything;
        _roomManager.EndedRoom += RoomManager_EndRoom;
    }

    private void RoomManager_EndRoom(object sender, EventArgs e)
    {
        SetWinUIWhenLevelEnds();
    }
    private void EnableEverything(object sender, EventArgs e)
    {
        _enabled = true;
    }
    private int CalculateEndScore()
    {
        float artifactCount = _artifactManager.GetArtifactCount();
        float maxScore = _artifactManager.transform.childCount;
        float score = artifactCount / maxScore;
        if (score >= _threeStarArtifactThreshold)
        {
            return 3;
        }
        else if(score >= _twoStarArtifactThreshold)
        {
            return 2;
        }
        else if(score >= _oneStarArtifactThreshold)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void SetWinUIWhenLevelEnds()
    {
        if (_enabled)
        {
            int endScore = CalculateEndScore();


            _winPanel.SetActive(true);

            _score.text = $"{_artifactManager.GetArtifactCount()}/{_artifactManager.transform.childCount}";

            //Reset score
            for (int i = 0; i < _starImageArray.Length; i++)
            {
                _starImageArray[i].SetActive(false);
            }
            //Set new score
            for (int i = 0; i < endScore; i++)
            {
                _starImageArray[i].SetActive(true);
            }
            HandleScorecard(endScore);
            Invoke("DisableWinUI", 5);
            _enabled= false;
        }
        
    }
    void HandleScorecard(int endScore)
    {
        if(endScore > _highscore)
        {
            _highscore = endScore;
            for (int i = 0; i < endScore; i++)
            {
                _scoreCardStars[i].SetActive(true);
            }
        }
    }
    private void DisableWinUI()
    {
        _winPanel.SetActive(false);
    }
}
