using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public SoundManager soundManager; // SoundManager 참조

    private float v = 0;     //수직 입력을 저장할 변수 (앞뒤 이동)
    private float h = 0;     //수평 입력을 저장할 변수 (좌우 이동)

    private float v1 = 0;    //보정된 수직 이동 값을 저장하는 변수
    private float h1 = 0;    //보정된 수평 이동 값을 저장하는 변수

    private Vector3 moveDir = new Vector3(0, 0, 0); //이동 방향을 나타내는 벡터

    [SerializeField] private float speed = 10f;   //캐릭터의 이동 속도
    [SerializeField] private float runSpeed = 20f;  //달리기 속도
    [SerializeField] private float rotSpeed = 500f;  //캐릭터의 회전 속도

    private Animator _animator;

    public GameObject bullet;        // 총알 Prefab
    public Transform fireTr;         // 총알 발사 위치 (플레이어 앞)

    public int playerHp = 5;
    [SerializeField] private int playerExp = 0;
    [SerializeField] private int maxExp = 10;
    private int playerLv = 1;

    public WeaponManagerUI weaponManagerUI;
    public GameObject gameOverUI;    // 게임 오버 UI 참조


    // Start is called before the first frame update
    void Start()
    {
        if (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>(); // 자동으로 찾기
        }
        _animator = GetComponent<Animator>();  // Animator 컴포넌트 초기화

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);  // 게임 시작 시 Game Over UI 비활성화
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHp <= 0)
        {
            GameOver();
            return; // 사망 처리 이후 Update 진행 중단
        }

        if (playerExp >= maxExp)
        {
            LevelUP();
        }

        _animator.SetBool("p_Attack", false);
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        v1 = v * Mathf.Sqrt(1 - (Mathf.Pow(h, 2) / 2));
        h1 = h * Mathf.Sqrt(1 - (Mathf.Pow(v, 2) / 2));

        float currentSpeed = speed;

        if (v > 0 && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            currentSpeed = runSpeed;
        }

        moveDir = (Vector3.forward * v1 + Vector3.right * h1);

        // 캐릭터 이동
        transform.Translate(moveDir * currentSpeed * Time.deltaTime, Space.Self);
        // 회전
        transform.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));

        _animator.SetFloat("p_V", v1);
        _animator.SetFloat("p_H", h1);

        // 총알 발사 (마우스 왼쪽 버튼 클릭)
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
            soundManager.PlayShootSound();  // 총알 발사 사운드 호출
            _animator.SetBool("p_Attack", true);
        }
    }

    // 총알 발사 메서드
    private void FireBullet()
    {
        if (bullet != null && fireTr != null)
        {
            // 총알 인스턴스를 발사 위치에서 생성
            Instantiate(bullet, fireTr.position, fireTr.rotation);
        }
        else
        {
            Debug.LogWarning("Bullet or Fire Transform is not set up!");
        }
    }

    // 경험치 증가 메서드
    public void AddExperience(int amount)
    {
        playerExp += amount;
        Debug.Log("Player Experience: " + playerExp);

        if (playerExp >= maxExp)
        {
            LevelUP();
        }
    }

    void LevelUP()
    {
        playerExp -= maxExp;
        maxExp += 5;
        playerLv += 1;
        Debug.Log("Level UP");

        // 레벨업 시 WeaponManagerUI를 열기
        if (weaponManagerUI != null)
        {
            weaponManagerUI.ShowWeaponUI(); // 레벨업 시 WeaponManagerUI를 활성화
        }
    }

    // 게임 오버 처리
    private void GameOver()
    {
        Time.timeScale = 0;  // 게임 일시정지
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);  // Game Over UI 활성화
        }
        Debug.Log("Game Over");
    }
}
