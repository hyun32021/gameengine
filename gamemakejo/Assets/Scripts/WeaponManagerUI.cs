using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponManagerUI : MonoBehaviour
{
    public GameObject upgradeUI; // 무기 선택 UI
    public Button[] weaponButtons; // 무기 선택 버튼들
    public WeaponManager weaponManager; // 무기 매니저

    private List<WeaponData> allWeapons = new List<WeaponData>(); // 모든 무기들
    private List<WeaponData> equippedWeapons = new List<WeaponData>(); // 장착된 무기들

    void Start()
    {
        upgradeUI.SetActive(false); // UI는 기본적으로 비활성화

        // 각 무기 버튼에 클릭 이벤트 추가
        foreach (Button button in weaponButtons)
        {
            button.onClick.AddListener(() => HandleWeaponButtonClick(button));
        }
    }

    // 무기 선택 UI를 보여주는 메서드
    public void ShowWeaponUI()
    {
        upgradeUI.SetActive(true); // UI 활성화
        Time.timeScale = 0f; // 게임 일시 정지

        // 마우스 활성화 및 커서 표시
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // 장착된 무기와 모든 무기 리스트 받아오기
        allWeapons = weaponManager.GetAvailableWeapons();
        equippedWeapons = weaponManager.GetEquippedWeapons();

        List<WeaponData> displayWeapons = GetDisplayWeapons();

        // 버튼에 무기 데이터 할당
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (i < displayWeapons.Count)
            {
                weaponButtons[i].gameObject.SetActive(true); // 버튼 보이기
                int index = i; // 버튼에 맞는 무기 설정

                // 버튼에 무기 이미지 설정
                Image buttonImage = weaponButtons[i].GetComponent<Image>();
                buttonImage.sprite = displayWeapons[index].weaponImage; // 해당 무기 이미지 할당

                weaponButtons[i].onClick.RemoveAllListeners(); // 기존 리스너 제거
                weaponButtons[i].onClick.AddListener(() => SelectWeapon(displayWeapons[index])); // 새 리스너 추가
            }
            else
            {
                weaponButtons[i].gameObject.SetActive(false); // 버튼 숨기기
            }
        }
    }

    // 무기 선택 버튼 클릭 시 호출되는 메서드
    private void HandleWeaponButtonClick(Button button)
    {
        // 버튼 인덱스에 맞는 무기를 선택하여 처리
        int index = System.Array.IndexOf(weaponButtons, button);
        if (index >= 0 && index < allWeapons.Count)
        {
            SelectWeapon(allWeapons[index]);
        }
    }

    // 선택할 무기를 결정하는 메서드
    private List<WeaponData> GetDisplayWeapons()
    {
        // 장착된 무기가 maxWeaponSlots개 미만이면 랜덤으로 3개 무기 선택
        if (equippedWeapons.Count < weaponManager.maxWeaponSlots)
        {
            return allWeapons.OrderBy(x => Random.value).Take(3).ToList();
        }
        else
        {
            // 장착된 무기가 maxWeaponSlots개라면 장착된 무기들 중 랜덤으로 3개 선택
            return equippedWeapons.OrderBy(x => Random.value).Take(3).ToList();
        }
    }

    // 무기 장착/업그레이드 메서드
    void SelectWeapon(WeaponData selectedWeapon)
    {
        if (equippedWeapons.Contains(selectedWeapon))
        {
            // 이미 장착된 무기라면 강화
            selectedWeapon.Upgrade();  // 무기 업그레이드
            Debug.Log($"{selectedWeapon.weaponName} 강화됨!");
        }
        else if (equippedWeapons.Count < weaponManager.maxWeaponSlots)
        {
            // 장착되지 않은 무기라면 장착
            weaponManager.EquipWeapon(selectedWeapon); // 무기 장착
            Debug.Log($"{selectedWeapon.weaponName} 장착됨!");
        }
        else
        {
            Debug.Log("무기 슬롯이 꽉 찼습니다.");
        }

        // UI를 닫고 무기 선택 후 처리가 끝났으므로 UI 비활성화
        CloseWeaponUI();
    }

    // UI 닫기
    public void CloseWeaponUI()
    {
        upgradeUI.SetActive(false); // UI 비활성화
        Time.timeScale = 1f; // 게임 재개

        Cursor.lockState = CursorLockMode.Locked; // 마우스 비활성화
        Cursor.visible = false;
    }
}
