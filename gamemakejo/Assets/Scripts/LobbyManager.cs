using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LobbyManager : MonoBehaviour
{
    // 게임 시작 버튼이 눌렸을 때 호출되는 함수
    public void StartGame()
    {
        Time.timeScale = 1f;
        // GameManager에서 씬 전환 메서드를 호출
        GameManager.Instance.LoadNextScene();  // Stage1 씬으로 이동하는 방식
    }
}
