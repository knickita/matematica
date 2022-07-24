using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Interaction : MonoBehaviour
{
    public GameObject menu;
    public Button settings_button;
    private bool is_menu_open;

    private TutorialManager tutorialManager;
    
    // Start is called before the first frame update
    void Start()
    {
        is_menu_open = false;
        menu.SetActive(false);
        settings_button.onClick.AddListener(openCloseMenu);
        tutorialManager = GameObject.FindObjectOfType<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void openCloseMenu()
    {
        if (tutorialManager != null)
        {
            if (!tutorialManager.isSettingsEnabled())
            {
                return;
            }
        }

        if (is_menu_open)
        {
            menu.SetActive(false);
            is_menu_open = false;
        }
        else
        {
            menu.SetActive(true);
            is_menu_open = true;
        }        
    }

    public bool isMenuOpen()
    {
        return is_menu_open;
    }
}

