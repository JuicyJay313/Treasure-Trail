using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManagement : MonoBehaviour
{
    private int highlighted = 0;
    [SerializeField] TextMeshProUGUI[] buttonTexts;
    [SerializeField] string[] sceneNames;
    Color32 menuTextColor;
    void Start()
    {
        menuTextColor = buttonTexts[0].color;
    }

    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            highlighted = highlighted == 0 ? buttonTexts.Length-1 : highlighted - 1;       
        }

        if (Input.GetKeyDown("down"))
        {
            highlighted = highlighted == buttonTexts.Length-1 ? 0 : highlighted + 1;
        }

        switch (highlighted)
        {
            case 0:
                for (int i = 0; i < buttonTexts.Length; i++)
                {
                    buttonTexts[i].color = menuTextColor;
                }
                buttonTexts[highlighted].color = Color.white;

                if (Input.GetButtonDown("Submit"))
                {
                    SceneManager.LoadScene(sceneNames[0]);
                }
                break;

            case 1:
                for (int i = 0; i < buttonTexts.Length; i++)
                {
                    buttonTexts[i].color = menuTextColor;
                }
                buttonTexts[highlighted].color = Color.white;

                if (Input.GetButtonDown("Submit"))
                {
                    SceneManager.LoadScene(sceneNames[1]);
                }
                break;

            case 2:
                for (int i = 0; i < buttonTexts.Length; i++)
                {
                    buttonTexts[i].color = menuTextColor;
                }
                buttonTexts[highlighted].color = Color.white;

                if (Input.GetButtonDown("Submit"))
                {
                    SceneManager.LoadScene(sceneNames[2]);
                }
                break;
        }
        /*
        if (highlighted == 0)
        {
            for(int i = 0; i < buttonTexts.Length; i++)
            {
                buttonTexts[i].color = menuTextColor;
            }
            buttonTexts[highlighted].color = Color.white;
            
            if (Input.GetButtonDown("Submit"))
            {
                SceneManager.LoadScene("Level 1");
            }
        }
        else if (highlighted == 1)
        {
            for (int i = 0; i < buttonTexts.Length; i++)
            {
                buttonTexts[i].color = menuTextColor;
            }
            buttonTexts[highlighted].color = Color.white;

            if (Input.GetButtonDown("Submit"))
            {
                // Go to Credits screen
            }
        }
        else if (highlighted == 2)
        {
            for (int i = 0; i < buttonTexts.Length; i++)
            {
                buttonTexts[i].color = menuTextColor;
            }
            buttonTexts[highlighted].color = Color.white;

            if (Input.GetButtonDown("Submit"))
            {
                // Go to Options screen
            }
        }*/
        
    }

}
