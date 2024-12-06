using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource shootAudioSource;  // 총알 발사 사운드를 재생할 AudioSource
    public AudioSource enemyDeadAudioSource;  // 적 사망 사운드를 재생할 AudioSource
    public AudioClip shootSound;  // 총알 발사 사운드 클립
    public AudioClip enemyDeadSound;  // 적 사망 사운드 클립

    // Start is called before the first frame update
    void Start()
    {
        if (shootAudioSource == null || shootSound == null)
        {
            Debug.LogWarning("ShootAudioSource or ShootSound is not set up!");
        }

        if (enemyDeadAudioSource == null || enemyDeadSound == null)
        {
            Debug.LogWarning("EnemyDeadAudioSource or EnemyDeadSound is not set up!");
        }
    }

    // 총알 발사 사운드를 재생하는 메서드
    public void PlayShootSound()
    {
        if (shootAudioSource != null && shootSound != null)
        {
            shootAudioSource.PlayOneShot(shootSound);
        }
        else
        {
            Debug.LogWarning("ShootAudioSource or ShootSound is not set up!");
        }
    }

    // 적 사망 사운드를 재생하는 메서드
    public void PlayEnemyDeadSound()
    {
        if (enemyDeadAudioSource != null && enemyDeadSound != null)
        {
            enemyDeadAudioSource.PlayOneShot(enemyDeadSound);
        }
        else
        {
            Debug.LogWarning("EnemyDeadAudioSource or EnemyDeadSound is not set up!");
        }
    }
}
