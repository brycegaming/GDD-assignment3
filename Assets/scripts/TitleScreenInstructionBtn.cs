﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenInstructionBtn : MonoBehaviour
{

    private Button __play_button;

    void Awake()
    {
        __play_button = GetComponent<Button>();
    }

    // Use this for initialization
    void Start()
    {
        __play_button.onClick.AddListener(_go_to_play);
    }

    private void _go_to_play()
    {
        SceneManager.LoadScene("InstructionPage");
    }

    public void GoToConceptArt()
    {

    }
}
