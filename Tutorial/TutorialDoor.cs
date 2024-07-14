using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    public AudioSource _audioSource;
    public Animator _door;

    private void Update()
    {
        if(!_audioSource.isPlaying)
        {
            _door.SetBool("OpenDoor", true);
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            _audioSource.Stop();
            _door.SetBool("OpenDoor", true);
        }
    }
}
