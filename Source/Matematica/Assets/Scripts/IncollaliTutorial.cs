using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class IncollaliTutorial
{
    static List<TutorialManager.TutorialStep> tutorialSteps;

    //combinations of possible problems given the area of a Quad
    static Dictionary<int, List<int>> combinations = new Dictionary<int, List<int>>()
    {
        {2, new List<int>(){ 2,3,4,5,6,7,8,10,12,14,16,18,25,30,40,48,54,70} },
        {3, new List<int>(){ 3,4,5,6,7,9,12,15,18,21,24,27,32,42,45,60} },
        {4, new List<int>(){ 4,5,6,8,10,12,14,16,20,21,32,36,45,50,56,60,63,64,10} },
        {5, new List<int>(){ 5,7,9,10,15,16,20,25,27,30,35,40,45,49,100} },
        {6, new List<int>(){ 6,8,9,10,12,14,15,18,21,24,30,36,42,48,50,54,64} },
        {7, new List<int>(){ 7,8,9,14,18,20,25,35,42,49,56,63} },
        {8, new List<int>(){ 8,10,12,16,24,27,32,40,42,48,56,64,72,100} },
        {9, new List<int>(){ 9,12,15,16,18,21,27,36,40,45,54,63,72,81} },
        {10, new List<int>(){ 10,14,15,20,25,30,32,35,40,50,54,60,70,80,90,100} },
        {12, new List<int>(){ 12,15,18,20,24,30,36,42,48,54,60,100} },
        {14, new List<int>(){ 16,18,21,35,36,40,42,49,50,56,90,100} },
        {15, new List<int>(){ 15,20,21,25,27,30,35,45,48,49,54,90} },
        {16, new List<int>(){ 16,20,24,32,40,48,54,56,64} },
        {18, new List<int>(){ 18,24,27,30,32,36,42,45,54,63,72,90} },
        {20, new List<int>(){ 20,25,30,36,40,50,60,70,80,81,90,100} },
        {21, new List<int>(){ 21,24,27,35,42,49,60,80,100} },
        {24, new List<int>(){ 24,25,30,32,36,40,48,56,80,81,90} },
        {25, new List<int>(){ 25,35,45,56,80} },
        {27, new List<int>(){ 27,36,45,54,63,81,90} },
        {30, new List<int>(){ 30,40,42,50,60,70,80,90,100} },
        {32, new List<int>(){ 32,40,48,49,72,80,100} },
        {35, new List<int>(){ 35,45,70,100} },
        {36, new List<int>(){ 36,45,54,64,72,81,90,100} },
        {40, new List<int>(){ 40,50,60,64,70,72,80,81,90,100} },
        {42, new List<int>(){ 48,63,70,72,90} },
        {45, new List<int>(){ 45,56,60,63,64,72,81,90} },
        {48, new List<int>(){ 56,60,64,72,80} },
        {49, new List<int>(){ 56,60,63,70,72,81} },
        {50, new List<int>(){ 50,54,60,64,70,80,90,100} },
        {54, new List<int>(){ 54,56,60,63,72,81,90,100} },
        {56, new List<int>(){ 56,63,64,70,72,80,100} },
        {60, new List<int>(){ 60,70,72,80,90,100} },
        {63, new List<int>(){ 63,70,72,80,81,90} },
        {64, new List<int>(){ 64,72,80,90,100} },
        {70, new List<int>(){ 70,80,90,100} },
        {72, new List<int>(){ 72,80,81,90} },
        {80, new List<int>(){ 80,90,100} },
        {81, new List<int>(){ 81,90} },
        {90, new List<int>(){ 90,100} },
        {100, new List<int>(){100 } }
    };



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

        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorTopLeft);
        EnableCameraMove(true);
        ChangeMaxAvailableQuads(18);
        AddStep("", "",0,0,0,0,GenerateBaseRectangles);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorTopLeft);
        AddStep("move,glue,cut,rotate,settings", "Benvenuto nella modalità Incollali.\n\nL'obbiettivo del gioco è ottenere un solo Rettangolo!\n\nUsa tutti gli strumenti che hai a disposizione per riuscirci", 290,90,290,-45, ControlloLivello);
        AddStep("draw", "Ottimo lavoro!\n\nQuale sono le misure totali del Rettangolo?.\n\nSeleziona la Matita e scrivi i valori", 107, -146, 290, -45, ControlloFinale);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorBottomRight);
        AddStep("settings", "Esatto!\n\nPer tornare al menù fai click sul bottone IMPOSTAZIONI", -100, 100, -300, 300, ApriImpostazioni);

        return tutorialSteps;
    }

    static bool GenerateBaseRectangles()
    {
        Camera.main.transform.position = new Vector3(6, 5, -10);

        Vector2 rectSize = new Vector2(Random.Range(2, 11), Random.Range(2, 11));

        GridScript gs = GameObject.FindObjectOfType<GridScript>();
        GameObject newQuad = MonoBehaviour.Instantiate(gs.quad);

        Color color = gs.pickRandomColor();
        newQuad.GetComponent<Quad>().setColor(color);
        newQuad.transform.position = new Vector3(2, 0, 0);
        newQuad.GetComponent<Quad>().setSize(rectSize);
        newQuad.GetComponent<Quad>().changeState();

        newQuad.GetComponent<QuadUI>().operation_reference.transform.Find("Number1").GetComponent<UnityEngine.UI.InputField>().text = rectSize.x.ToString();
        newQuad.GetComponent<QuadUI>().operation_reference.transform.Find("Number2").GetComponent<UnityEngine.UI.InputField>().text = rectSize.y.ToString();

        List<int> solutions;
        combinations.TryGetValue((int)(rectSize.x * rectSize.y),out solutions);

        int secondArea = solutions[Random.Range(0, solutions.Count)];
        List<int> areaDivisors = new List<int>();
        for (int i = 1; i < Mathf.Sqrt(secondArea); i++)
        {
            if (secondArea % i == 0)
            {
                if (secondArea / i <= 20)
                {
                    areaDivisors.Add(i);
                }
            }
        }
        int secondWidth = areaDivisors[Random.Range(0, areaDivisors.Count)];

        Vector2 rect2Size = new Vector2(secondWidth, secondArea/secondWidth);
        //50% of the time swaps width with height
        if (Random.Range(0.0f, 1.0f) >= 0.5f)
        {
            rect2Size = new Vector2(rect2Size.y, rect2Size.x);
        }

        GameObject newQuad2 = MonoBehaviour.Instantiate(gs.quad);

        Color color2 = gs.pickRandomColor();
        newQuad2.GetComponent<Quad>().setColor(color2);
        newQuad2.transform.position = new Vector3(2+rectSize.x+1, 0, 0);
        newQuad2.GetComponent<Quad>().setSize(rect2Size);
        newQuad2.GetComponent<Quad>().changeState();

        newQuad2.GetComponent<QuadUI>().operation_reference.transform.Find("Number1").GetComponent<UnityEngine.UI.InputField>().text = rect2Size.x.ToString();
        newQuad2.GetComponent<QuadUI>().operation_reference.transform.Find("Number2").GetComponent<UnityEngine.UI.InputField>().text = rect2Size.y.ToString();

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
        //deve esserci solo un rettangolo
        return GameObject.FindGameObjectsWithTag("quad").Length == 1;
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
