using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponManagerUI : MonoBehaviour
{
    public GameObject upgradeUI;
    public Button[] weaponButtons;
    public WeaponManager weaponManager;

    private List<WeaponData> allWeapons = new List<WeaponData>();
    private List<WeaponData> equippedWeapons = new List<WeaponData>();

    void Start()
    {
        upgradeUI.SetActive(false);

        foreach (Button button in weaponButtons)
        {
            button.onClick.AddListener(() => HandleWeaponButtonClick(button));
        }
    }

    public void ShowWeaponUI()
    {
        upgradeUI.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        allWeapons = weaponManager.GetAvailableWeapons();
        equippedWeapons = weaponManager.GetEquippedWeapons();

        List<WeaponData> displayWeapons = GetDisplayWeapons();

        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (i < displayWeapons.Count)
            {
                weaponButtons[i].gameObject.SetActive(true);
                int index = i;

                Image buttonImage = weaponButtons[i].GetComponent<Image>();
                buttonImage.sprite = displayWeapons[index].weaponImage;

                weaponButtons[i].onClick.RemoveAllListeners();
                weaponButtons[i].onClick.AddListener(() => SelectWeapon(displayWeapons[index]));
            }
            else
            {
                weaponButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void HandleWeaponButtonClick(Button button)
    {
        int index = System.Array.IndexOf(weaponButtons, button);
        if (index >= 0 && index < allWeapons.Count)
        {
            SelectWeapon(allWeapons[index]);
        }
    }

    private List<WeaponData> GetDisplayWeapons()
    {
        if (equippedWeapons.Count < weaponManager.maxWeaponSlots)
        {
            return allWeapons.OrderBy(x => Random.value).Take(3).ToList();
        }
        else
        {
            return equippedWeapons.OrderBy(x => Random.value).Take(3).ToList();
        }
    }

    void SelectWeapon(WeaponData selectedWeapon)
    {
        if (equippedWeapons.Contains(selectedWeapon))
        {
            selectedWeapon.Upgrade();
            Debug.Log($"{selectedWeapon.weaponName} °­È­µÊ!");
        }
        else if (equippedWeapons.Count < weaponManager.maxWeaponSlots)
        {
            weaponManager.EquipWeapon(selectedWeapon);
            Debug.Log($"{selectedWeapon.weaponName} ÀåÂøµÊ!");
        }
        else
        {
            Debug.Log("¹«±â ½½·ÔÀÌ ²Ë Ã¡½À´Ï´Ù.");
        }

        CloseWeaponUI();
    }

    public void CloseWeaponUI()
    {
        upgradeUI.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

