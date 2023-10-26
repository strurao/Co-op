using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshPlayBGM : MonoBehaviour
{
    int idx = 0;
    public void playSound(AudioClip[] clip, AudioSource audioPlayer)
    {
        idx++;
        audioPlayer.Stop();
        audioPlayer.clip = clip[idx % clip.Length];
        audioPlayer.Play();
    }
}
