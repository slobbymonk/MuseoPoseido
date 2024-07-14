using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverTrigger : MonoBehaviour
{
    [SerializeField] private string _voiceOverLine;

    private VoiceOverManager _manager;

    [SerializeField] private bool _onlyTriggerOnce = true;
    private bool _hasBeenTriggered;

    private void Awake()
    {
        _manager = FindObjectOfType<VoiceOverManager>();

        if (_manager == null)
        {
            Debug.LogError("VoiceOverManager component not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_onlyTriggerOnce)
            {
                _manager.SpeakLine(_voiceOverLine);
            }
            else if (!_hasBeenTriggered)
            {
                _manager.SpeakLine(_voiceOverLine);
                _hasBeenTriggered = true;
            }

        }
    }
}
