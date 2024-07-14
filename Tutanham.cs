using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutanham : MonoBehaviour
{
    public string _voiceLine;

    private bool _skipFirst = false;

    private void Awake()
    {
        FindObjectOfType<VoiceOverManager>().SpeakLine(_voiceLine);
    }
}
