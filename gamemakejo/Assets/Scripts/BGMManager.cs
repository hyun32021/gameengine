using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;  // Singleton 인스턴스
    private AudioSource audioSource;    // AudioSource 컴포넌트
    private bool isMuted = false;       // BGM 활성/비활성 상태

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 삭제되지 않음
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // 중복된 BGMManager가 생성되지 않도록 파괴
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // 키보드 M 입력 감지
        {
            ToggleBGM();
        }
    }

    // BGM을 변경하는 메서드
    public void ChangeBGM(AudioClip bgmClip)
    {
        if (audioSource.clip == bgmClip) return; // 이미 재생 중인 BGM이면 무시

        audioSource.clip = bgmClip;
        audioSource.Play();
    }

    // BGM 켜고 끄는 토글 메서드
    public void ToggleBGM()
    {
        isMuted = !isMuted; // 상태 반전
        audioSource.mute = isMuted; // AudioSource의 음소거 상태 변경
        Debug.Log("BGM " + (isMuted ? "Muted" : "Unmuted"));
    }
}
