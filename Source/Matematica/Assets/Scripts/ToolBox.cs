using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBox : MonoBehaviour
{
    public Button delete_button,draw_button,move_button,rotate_button,glue_button,cut_button;

    public Toggle grid_visibility_toggle;

    public GameObject cut_object;

    private string selectedTool;
    private TutorialManager tutorialManager;
    // Start is called before the first frame update
    void Start()
    {
        delete_button.onClick.AddListener(SelectDeleteTool);
        draw_button.onClick.AddListener(SelectDrawTool);
        move_button.onClick.AddListener(SelectMoveTool);
        rotate_button.onClick.AddListener(SelectRotateTool);
        glue_button.onClick.AddListener(SelectGlueTool);
        cut_button.onClick.AddListener(SelectCutTool);

        tutorialManager = GameObject.FindObjectOfType<TutorialManager>();

        ResetToolsColors();

        grid_visibility_toggle.onValueChanged.AddListener(SwitchGridVisibility);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void DestroyCutObject()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("cut"))
        {
            Destroy(obj);
        }
    }

    public bool IsDrawSelected()
    {
        return selectedTool == "draw";
    }

    public bool IsDeleteSelected()
    {
        return selectedTool == "delete";
    }

    public bool IsMoveSelected()
    {
        return selectedTool == "move";
    }

    public bool IsRotateSelected()
    {
        return selectedTool == "rotate";
    }

    public bool IsGlueSelected()
    {
        return selectedTool == "glue";
    }

    public bool IsCutSelected()
    {
        return selectedTool == "cut";
    }

    public void SelectNoTool()
    {
        selectedTool = "";
        DestroyCutObject();
        ResetToolsColors();
    }

    void SelectDrawTool()
    {
        if (tutorialManager != null)
        {
            if (!tutorialManager.isDrawEnabled())
            {
                return;
            }
        }       

        selectedTool = "draw";
        DestroyCutObject();
        ResetToolsColors();
        draw_button.GetComponent<Image>().color = new Color(1, 1, 1, 1);        
    }

    void SelectDeleteTool()
    {
        if (tutorialManager != null)
        {
            if (!tutorialManager.isDeleteEnabled())
            {
                return;
            }
        }
        
        selectedTool = "delete";
        DestroyCutObject();
        ResetToolsColors();
        delete_button.GetComponent<Image>().color = new Color(1, 1, 1, 1);        
    }

    void SelectMoveTool()
    {
        if (tutorialManager != null)
        {
            if (!tutorialManager.isMoveEnabled())
            {
                return;
            }
        }
        
        selectedTool = "move";
        DestroyCutObject();
        ResetToolsColors();
        move_button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    void SelectRotateTool()
    {
        if (tutorialManager != null)
        {
            if (!tutorialManager.isRotateEnabled())
            {
                return;
            }
        }

        selectedTool = "rotate";
        DestroyCutObject();
        ResetToolsColors();
        rotate_button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    void SelectGlueTool()
    {
        if (tutorialManager != null)
        {
            if (!tutorialManager.isGlueEnabled())
            {
                return;
            }
        }

        selectedTool = "glue";
        DestroyCutObject();
        ResetToolsColors();
        glue_button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }

    void SelectCutTool()
    {
        if (tutorialManager != null)
        {
            if (!tutorialManager.isCutEnabled())
            {
                return;
            }
        }

        selectedTool = "cut";
        ResetToolsColors();
        cut_button.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        if (GameObject.FindGameObjectWithTag("cut") == null)
        {
            Instantiate(cut_object);
        }
    }

    void ResetToolsColors()
    {
        delete_button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        draw_button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        move_button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        rotate_button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        glue_button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
        cut_button.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
    }

    void SwitchGridVisibility(bool value)
    {
        GameObject.FindObjectOfType<GridScript>().gridVisibility = value;
    }
}
