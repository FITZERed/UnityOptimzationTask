using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    
    [SerializeField] private PlayerCharacterController bobby;
    [SerializeField] private SkillButtonUI[] skillButtons;
    
    public void RefreshHPText(int newHP)
    {
        hpText.text = newHP.ToString();
    }

    private void Awake()
    {
        bobby.onTakeDamageEventAction += RefreshHPText;
    }

    private void Start()
    {
        hpText.text = bobby.Hp.ToString();
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].skillIcon.sprite =  skillButtons[i].skillIcons[i];
            skillButtons[i].skillNameText.text = "Skill " + (i + 1);
        }
    }
}
