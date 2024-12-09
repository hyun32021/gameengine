using UnityEngine;

public class SceneBGMSetter : MonoBehaviour
{
    public AudioClip bgmClip; // æ¿ø° πË¡§«“ BGM

    private void Start()
    {
        if (BGMManager.Instance != null && bgmClip != null)
        {
            BGMManager.Instance.ChangeBGM(bgmClip);
        }
    }
}
