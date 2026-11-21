using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIWeapons : MonoBehaviour
{
    [SerializeField] Weapons m_currentWeapon;
    [SerializeField] TextMeshProUGUI m_weaponText;

    private string weapon;

    private void Start()
    {
        weapon = ": " + m_currentWeapon.name;
        m_weaponText.SetText(weapon);
    }
}
