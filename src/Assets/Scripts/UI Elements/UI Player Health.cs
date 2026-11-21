using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] Player m_player1;
    [SerializeField] TextMeshProUGUI m_healthText;

    private string playerHealth;
    private int previusHealth;

    private void Start()
    {
        SetHealthText();
    }

    private void Update()
    {
        if(m_player1.health != previusHealth)
        {
            SetHealthText();
        }
    }

    private void SetHealthText()
    {
        previusHealth = m_player1.health;
        playerHealth = ": " + m_player1.health;
        m_healthText.SetText(playerHealth);
    }
}
