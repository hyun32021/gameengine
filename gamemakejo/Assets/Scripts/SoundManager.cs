using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource shootAudioSource;  // �Ѿ� �߻� ���带 ����� AudioSource
    public AudioSource enemyDeadAudioSource;  // �� ��� ���带 ����� AudioSource
    public AudioClip shootSound;  // �Ѿ� �߻� ���� Ŭ��
    public AudioClip enemyDeadSound;  // �� ��� ���� Ŭ��

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

    // �Ѿ� �߻� ���带 ����ϴ� �޼���
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

    // �� ��� ���带 ����ϴ� �޼���
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
