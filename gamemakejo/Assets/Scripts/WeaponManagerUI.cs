using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponManagerUI : MonoBehaviour
{
    public GameObject upgradeUI; // ���� ���� UI
    public Button[] weaponButtons; // ���� ���� ��ư��
    public WeaponManager weaponManager; // ���� �Ŵ���

    private List<WeaponData> allWeapons = new List<WeaponData>(); // ��� �����
    private List<WeaponData> equippedWeapons = new List<WeaponData>(); // ������ �����

    void Start()
    {
        upgradeUI.SetActive(false); // UI�� �⺻������ ��Ȱ��ȭ

        // �� ���� ��ư�� Ŭ�� �̺�Ʈ �߰�
        foreach (Button button in weaponButtons)
        {
            button.onClick.AddListener(() => HandleWeaponButtonClick(button));
        }
    }

    // ���� ���� UI�� �����ִ� �޼���
    public void ShowWeaponUI()
    {
        upgradeUI.SetActive(true); // UI Ȱ��ȭ
        Time.timeScale = 0f; // ���� �Ͻ� ����

        // ���콺 Ȱ��ȭ �� Ŀ�� ǥ��
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // ������ ����� ��� ���� ����Ʈ �޾ƿ���
        allWeapons = weaponManager.GetAvailableWeapons();
        equippedWeapons = weaponManager.GetEquippedWeapons();

        List<WeaponData> displayWeapons = GetDisplayWeapons();

        // ��ư�� ���� ������ �Ҵ�
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (i < displayWeapons.Count)
            {
                weaponButtons[i].gameObject.SetActive(true); // ��ư ���̱�
                int index = i; // ��ư�� �´� ���� ����

                // ��ư�� ���� �̹��� ����
                Image buttonImage = weaponButtons[i].GetComponent<Image>();
                buttonImage.sprite = displayWeapons[index].weaponImage; // �ش� ���� �̹��� �Ҵ�

                weaponButtons[i].onClick.RemoveAllListeners(); // ���� ������ ����
                weaponButtons[i].onClick.AddListener(() => SelectWeapon(displayWeapons[index])); // �� ������ �߰�
            }
            else
            {
                weaponButtons[i].gameObject.SetActive(false); // ��ư �����
            }
        }
    }

    // ���� ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void HandleWeaponButtonClick(Button button)
    {
        // ��ư �ε����� �´� ���⸦ �����Ͽ� ó��
        int index = System.Array.IndexOf(weaponButtons, button);
        if (index >= 0 && index < allWeapons.Count)
        {
            SelectWeapon(allWeapons[index]);
        }
    }

    // ������ ���⸦ �����ϴ� �޼���
    private List<WeaponData> GetDisplayWeapons()
    {
        // ������ ���Ⱑ maxWeaponSlots�� �̸��̸� �������� 3�� ���� ����
        if (equippedWeapons.Count < weaponManager.maxWeaponSlots)
        {
            return allWeapons.OrderBy(x => Random.value).Take(3).ToList();
        }
        else
        {
            // ������ ���Ⱑ maxWeaponSlots����� ������ ����� �� �������� 3�� ����
            return equippedWeapons.OrderBy(x => Random.value).Take(3).ToList();
        }
    }

    // ���� ����/���׷��̵� �޼���
    void SelectWeapon(WeaponData selectedWeapon)
    {
        if (equippedWeapons.Contains(selectedWeapon))
        {
            // �̹� ������ ������ ��ȭ
            selectedWeapon.Upgrade();  // ���� ���׷��̵�
            Debug.Log($"{selectedWeapon.weaponName} ��ȭ��!");
        }
        else if (equippedWeapons.Count < weaponManager.maxWeaponSlots)
        {
            // �������� ���� ������ ����
            weaponManager.EquipWeapon(selectedWeapon); // ���� ����
            Debug.Log($"{selectedWeapon.weaponName} ������!");
        }
        else
        {
            Debug.Log("���� ������ �� á���ϴ�.");
        }

        // UI�� �ݰ� ���� ���� �� ó���� �������Ƿ� UI ��Ȱ��ȭ
        CloseWeaponUI();
    }

    // UI �ݱ�
    public void CloseWeaponUI()
    {
        upgradeUI.SetActive(false); // UI ��Ȱ��ȭ
        Time.timeScale = 1f; // ���� �簳

        Cursor.lockState = CursorLockMode.Locked; // ���콺 ��Ȱ��ȭ
        Cursor.visible = false;
    }
}
