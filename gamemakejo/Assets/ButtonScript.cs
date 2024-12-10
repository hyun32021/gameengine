using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public AudioSource audioSource;  // 클릭 소리를 재생할 AudioSource
    public AudioClip clickSound;     // 클릭 사운드 클립

    public void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound); // 클릭 소리 재생
        }
        else
        {
            Debug.LogWarning("AudioSource or ClickSound is missing!");
        }
    }
}

