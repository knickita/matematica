using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TetrisTutorial
{
    static List<TutorialManager.TutorialStep> tutorialSteps;

    static List<Vector4> quads_grid;



    static void AddStep(string toolsEnabled, string text, float targetPositionX, float targetPositionY, float bubblePositionX, float bubblePositionY, System.Func<bool> endCondition, string tag="")
    {
        TutorialManager.TutorialStep step = new TutorialManager.TutorialStep();

        foreach (string s in toolsEnabled.Split(','))
        {
            switch (s)
            {
                case "delete": step.enableDeleteTool = true; break;
                case "draw": step.enableDrawTool = true; break;
                case "move": step.enableMoveTool = true; break;
                case "cut": step.enableCutTool = true; break;
                case "glue": step.enableGlueTool = true; break;
                case "rotate": step.enableRotateTool = true; break;
                case "settings": step.enableSettingsButton = true; break;
                case "none": step.unselectAllTools = true; break;
            }
        }

        step.endCondition = endCondition;

        step.text = text;
        step.targetPosition = new Vector2(targetPositionX, targetPositionY);
        step.bubblePosition = new Vector2(bubblePositionX, bubblePositionY);
        step.changemaxAvailableQuads = -1;
        step.tag = tag;

        tutorialSteps.Add(step);
    }

    static void ChangeMaxAvailableQuads(int maxQuads)
    {
        TutorialManager.TutorialStep step = new TutorialManager.TutorialStep();
        step.endCondition = delegate { return true; } ;
        step.text = "";
        step.targetPosition = new Vector2(0, 0);
        step.bubblePosition = new Vector2(0, 0);

        step.changemaxAvailableQuads = maxQuads;

        tutorialSteps.Add(step);
    }
    static void GoToStep(string stepTag)
    {
        TutorialManager.TutorialStep step = new TutorialManager.TutorialStep();
        step.endCondition = delegate { return true; };
        step.text = "";
        step.targetPosition = new Vector2(0, 0);
        step.bubblePosition = new Vector2(0, 0);
        step.changemaxAvailableQuads = -1;
        step.goToStep = stepTag;
        
        tutorialSteps.Add(step);
    }

    static void EnableCameraMove(bool enable)
    {
        TutorialManager.TutorialStep step = new TutorialManager.TutorialStep();
        step.endCondition = delegate { return true; };
        step.text = "";
        step.targetPosition = new Vector2(0, 0);
        step.bubblePosition = new Vector2(0, 0);

        if (enable)
        {
            step.changeEnableCameraMove = "true";
        }
        else
        {
            step.changeEnableCameraMove = "false";
        }

        tutorialSteps.Add(step);
    }



    public static List<TutorialManager.TutorialStep> Load()
    {
        tutorialSteps = new List<TutorialManager.TutorialStep>();

        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorTopRight);
        EnableCameraMove(true);
        ChangeMaxAvailableQuads(18);
        AddStep("", "",0,0,0,0,GenerateBaseRectangles);
        AddStep("delete,draw,move,glue,cut,rotate,settings", "Benvenuto nella modalità Tetris.\n\nSul campo di gioco sono indicati dei numeri:\nrappresentano l'area dei rettangoli.\n\nRiesci a disegnare i rettangoli in modo che\nl'angolo in alto a destra di essi\ncada in corrispondenza dei numeri?", 351,-90,-142,-70, ControlloLivello);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorBottomRight);
        AddStep("settings", "Esatto!\n\nPer tornare al menù fai click sul bottone IMPOSTAZIONI", -100, 100, -300, 300, ApriImpostazioni);

        return tutorialSteps;
    }

    static void GridGenerator(int number_of_quads, int grid_width, int grid_height)
    {
        List<Vector4> grid = new List<Vector4>();

        grid.Add(new Vector4(0, 0, grid_width, grid_height));

        for (int iteration = 0; iteration < number_of_quads - 1; iteration++)
        {
            int index;
            Vector4 old_rect;

            do
            {
                index = Random.Range(0, grid.Count);
                old_rect = grid[index];
            } while (old_rect.z == 1 && old_rect.w == 1);

            //select an axis for cut
            bool horizontal_cut = false;
            if (old_rect.z > 1)
            {
                if (old_rect.w > 1)
                {
                    if (Random.Range(0.0f, 1.0f) >= 0.5f)
                    {
                        horizontal_cut = true;
                    }
                }                
            }
            else
            {
                horizontal_cut = true;
            }

            //cut the rectangle
            Vector4 new_rect;
            if (horizontal_cut)
            {
                int point_of_cut = Random.Range(1, (int)old_rect.w);
                new_rect = new Vector4(old_rect.x, old_rect.y + point_of_cut, old_rect.z, old_rect.w - point_of_cut);
                grid[index] = new Vector4(old_rect.x, old_rect.y, old_rect.z, point_of_cut);
            }
            else
            {
                int point_of_cut = Random.Range(1, (int)old_rect.z);
                new_rect = new Vector4(old_rect.x + point_of_cut, old_rect.y, old_rect.z - point_of_cut, old_rect.w);
                grid[index] = new Vector4(old_rect.x, old_rect.y, point_of_cut, old_rect.w);
            }

            grid.Add(new_rect);
        }

        quads_grid = grid;
    }

    static bool QuadExistInGrid(GameObject real_quad)
    {
        foreach (Vector4 grid_quad in quads_grid)
        {
            if ((real_quad.transform.position.x == grid_quad.x) &&
               (real_quad.transform.position.y == grid_quad.y) &&
               (real_quad.GetComponent<Quad>().getSize().x == grid_quad.z) &&
               (real_quad.GetComponent<Quad>().getSize().y == grid_quad.w)) {
                return true;
            }
        }
        return false;
    }

    static bool GenerateBaseRectangles()
    {
        Camera.main.transform.position = new Vector3(6, 5, -10);
        GridScript gs = GameObject.FindObjectOfType<GridScript>();

        GridGenerator(6, 10, 10);


        GameObject overlay_canvas = GameObject.Find("TetrisCanvasOverlay");
        GameObject overlay_text = overlay_canvas.transform.Find("TetrisTextOverlay").gameObject;

        foreach (Vector4 quad_data in quads_grid)
        {
            GameObject new_text = MonoBehaviour.Instantiate(overlay_text,overlay_canvas.transform);
            new_text.transform.position=new Vector3(quad_data.x+quad_data.z,quad_data.y+quad_data.w,overlay_text.transform.position.z);
            new_text.GetComponent<UnityEngine.UI.Text>().text=(quad_data.z*quad_data.w).ToString();
        }

        MonoBehaviour.Destroy(overlay_text);

        return true;
    }

    public static bool SelezionaDraw()
    {
        return GameObject.FindObjectOfType<ToolBox>().IsDrawSelected();
    }

    public static bool ChangeUIAnchorTopLeft()
    {
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
        return true;
    }
    public static bool ChangeUIAnchorTopRight()
    {
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        return true;
    }
    public static bool ChangeUIAnchorBottomLeft()
    {
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        return true;
    }
    public static bool ChangeUIAnchorBottomRight()
    {
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
        return true;
    }


    public static bool ControlloLivello()
    {
        GameObject[] real_quads = GameObject.FindGameObjectsWithTag("quad");
        if (real_quads.Length != quads_grid.Count)
        {
            return false;
        }
        foreach (GameObject quad in real_quads)
        {
            if (!QuadExistInGrid(quad))
            {
                return false;
            }
        }
        return true;
    }    

    public static bool ControlloFinale()
    {
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            //impedisce di costruire altri Rettangoli
            if (quad.GetComponent<Quad>().getState() < 2)
            {
                MonoBehaviour.Destroy(quad);
                return false;
            }

            if (quad.GetComponent<QuadUI>().buttonX.image.color == new Color(0, 255, 0))
            {
                if (quad.GetComponent<QuadUI>().buttonY.image.color == new Color(0, 255, 0))
                {
                    if (quad.GetComponent<QuadUI>().buttonA.image.color == new Color(0, 255, 0))
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }

    public static bool ApriImpostazioni()
    {
        return GameObject.FindObjectOfType<Menu_Interaction>().isMenuOpen();
    }
}
