using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject gameOverPanel;  // 게임 오버 UI 패널
    private CameraCtrl cameraCtrl;    // CameraCtrl 스크립트 참조

    void Start()
    {
        gameOverPanel.SetActive(false);  // 시작 시 UI 비활성화
        cameraCtrl = FindObjectOfType<CameraCtrl>();
    }

    public void ShowGameOverUI()
    {
        Time.timeScale = 0;  // 게임 일시정지
        gameOverPanel.SetActive(true);  // 게임 오버 UI 활성화
        if (cameraCtrl != null)
        {
            cameraCtrl.UnlockCursor();  // 커서를 해제하여 버튼 클릭 가능
        }
    }

    public void OnRestartButtonClicked()
    {
        // 플레이어 상태 초기화
        PlayerCtrl player = FindObjectOfType<PlayerCtrl>();
        if (player != null)
        {
            player.ResetPlayerState();  // 플레이어 상태 초기화
        }

        // Stage1 씬 재로드
        SceneManager.LoadScene("Stage1");

        if (cameraCtrl != null)
        {
            cameraCtrl.LockCursor();  // 재시작 시 커서 잠금
        }
        Time.timeScale = 1f;
    }

    public void OnExitButtonClicked()
    {
        // Lobby 씬 로드
        SceneManager.LoadScene("Lobby");

        if (cameraCtrl != null)
        {
            cameraCtrl.LockCursor();  // 로비로 돌아가도 커서 잠금
        }
        Time.timeScale = 1f;
    }
}
