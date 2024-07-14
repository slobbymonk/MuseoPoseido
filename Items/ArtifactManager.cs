using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactManager : MonoBehaviour
{
    [SerializeField] private List<Artifacts> _allArtifacts;

    //UI
    public Transform _artifactIcon;
    public Sprite _destroyedArtifactIcon;

    public int GetArtifactCount()
    {
        return _allArtifacts.Count;
    }

    private void Start()
    {
        LevelManager.instance.StartedRoom += FillList;
        LevelManager.instance.EndedRoom += ClearList;
    }
    private void FillList(object sender, EventArgs e)
    {
        SetUpArtifacts();
    }
    private void ClearList(object sender, EventArgs e)
    {
        RemoveAllCurrentUI();
    }
    public void SetUpArtifacts()
    {
        //Debug.Log("Setting Up Artifacts");
        FillListOfArtifacts();
        VisualizeListOfArtifacts();
    }
    private void RemoveAllCurrentUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
    private void FillListOfArtifacts()
    {
        _allArtifacts = new List<Artifacts>();
        var arrayOfArtifacts = GameObject.FindObjectsOfType<Artifacts>();
        foreach (var artifact in arrayOfArtifacts)
        {
            if (artifact._roomID == LevelManager.instance.GeActivetRoom())
            {
                _allArtifacts.Add(artifact);
                artifact.GetComponent<Collider>().isTrigger = false;
                artifact.GetComponent<Rigidbody>().isKinematic = false;
                artifact.WhenDestroyed += RemoveArtifactFromList;
            }
        }
    }
    private void VisualizeListOfArtifacts()
    {
        if(transform.childCount != 0)
            RemoveAllCurrentUI();
        for (int i = 0; i < _allArtifacts.Count; i++)
        {
            Instantiate(_artifactIcon, transform);
        }
    }
    private void RemoveArtifactFromList(Artifacts artifact)
    {
        if(transform.childCount > 0)
        {
            transform.GetChild(transform.childCount - (_allArtifacts.Count)).GetComponent<Image>().sprite = _destroyedArtifactIcon;
            _allArtifacts.Remove(artifact);
        }
    }
}
