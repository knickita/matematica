using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MurettiTutorial
{
    static List<TutorialManager.TutorialStep> tutorialSteps;

    static int altezzaTotale=0;
    static int altezzaLivelli;
    static int larghezzaLivelli;


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
        AddStep("", "",0,0,0,0,GenerateBaseRectangle);
        AddStep("draw", "Benvenuto nella modalità Muretti.\n\nL'obbiettivo del gioco è costruire il muro più alto possibile.\n\nPer cominciare fai click sulla MATITA", 107,-146,290,-45, SelezionaDraw);
        AddStep("draw,settings", "Quello che vedi è il primo livello del Muro le regole sono semplici:\n1- Il nuovo livello che costruirai deve essere APPOGGIATO a quello vecchio e Alto quanto quello vecchio.\n2- Devi riempire tutto il nuovo livello con mattoni più piccoli di quelli che ci sono nel livello vecchio\n3- I nuovi mattoni non possono essere a cavallo tra 2 mattoni vecchi\n\nProva a costruire il Muretto più alto possibile!\n(ricorda che puoi spostarti col tasto destro del mouse)", 190, 100, 440, -60, ControlloLivello);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorTopRight);
        AddStep("draw", "Il Muretto è completo!\n\nOra scrivi tutte le misure dei lati e completa le uguaglianze.", 190, 100, -460, -30, ControlloCalcoli);
        AddStep("","",0,0,0,0,AggiungiRettangoloInvisibile);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorBottomRight);
        AddStep("draw", "Ottimo lavoro!\n\nQuale sono le misure totali del Muretto?.\n\nScrivilo nell'ultima riga della Finestra Aritmetica", 190, 100, -150, 150, ControlloFinale);
        AddStep("settings", "Esatto!\n\nPer tornare al menù fai click sul bottone IMPOSTAZIONI", -100, 100, -300, 300, ApriImpostazioni);

        return tutorialSteps;
    }

    static bool GenerateBaseRectangle()
    {
        GameObject.FindObjectOfType<AritmeticWindow>().view_toggle.isOn = true;
        Camera.main.transform.position = new Vector3(6, 5, -10);

        Vector2 rectSize = new Vector2(Random.Range(5, 11), Random.Range(1, 6));
        larghezzaLivelli=(int)(rectSize.x);
        altezzaLivelli = (int)(rectSize.y);
        altezzaTotale += altezzaLivelli;

        GameObject.FindObjectOfType<GridScript>().width = larghezzaLivelli;

        GridScript gs = GameObject.FindObjectOfType<GridScript>();
        GameObject newQuad = MonoBehaviour.Instantiate(gs.quad);

        Color color = gs.pickRandomColor();
        newQuad.GetComponent<Quad>().setColor(color);
        newQuad.transform.position = new Vector3(0, 0, 0);
        newQuad.GetComponent<Quad>().setSize(rectSize);
        newQuad.GetComponent<Quad>().changeState();

        newQuad.GetComponent<QuadUI>().operation_reference.transform.Find("Number1").GetComponent<UnityEngine.UI.InputField>().text = rectSize.x.ToString();
        newQuad.GetComponent<QuadUI>().operation_reference.transform.Find("Number2").GetComponent<UnityEngine.UI.InputField>().text = rectSize.y.ToString();
        newQuad.GetComponent<QuadUI>().operation_reference.transform.Find("Number3").GetComponent<UnityEngine.UI.InputField>().text = (rectSize.x*rectSize.y).ToString();

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
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            if (quad.GetComponent<Quad>().getState() < 2)
            {
                //impedisce ai quadrati di essere posizionati ad una quota diversa da quella imposta
                if (quad.transform.position.y != altezzaTotale)
                {
                    MonoBehaviour.Destroy(quad);
                    return false;
                }
                //impedisce ai quadrati di essere posizionati (in larghezza) fuori dal livello precedente
                if (quad.transform.position.x > 2 + larghezzaLivelli -1)
                {
                    MonoBehaviour.Destroy(quad);
                    return false;
                }
            }
            else
            {
                //impedisce ai quadrati di essere di un'altezza diversa da quella imposta
                if (quad.GetComponent<Quad>().getSize().y != altezzaLivelli)
                {
                    MonoBehaviour.Destroy(quad);
                    return false;
                }
                //impedisce ai quadrati di essere di sbordare (in larghezza) dal livello precedente
                if (quad.GetComponent<Quad>().transform.position.x + quad.GetComponent<Quad>().getSize().x > larghezzaLivelli)
                {
                    MonoBehaviour.Destroy(quad);
                    return false;
                }
                //impedisce che rettangolo disegnato sia più largo di quello sotto di lui, oppure che sia a cavallo tra due quadrati sotto di lui
                foreach (GameObject baseQuad in GameObject.FindGameObjectsWithTag("quad"))
                {
                    if (baseQuad.GetComponent<Quad>().ContainPoint(quad.transform.position + Vector3.down))
                    {
                        //controlla che il quadrato su cui ci si appoggia sia più largo di quello che si sta disegnando
                        if (quad.GetComponent<Quad>().getSize().x >= baseQuad.GetComponent<Quad>().getSize().x)
                        {
                            MonoBehaviour.Destroy(quad);
                            return false;
                        }
                        //controlla che non ci siano accavallamenti
                        else if (!baseQuad.GetComponent<Quad>().ContainPoint(quad.transform.position + Vector3.down + Vector3.right * (quad.GetComponent<Quad>().getSize().x-1)))
                        {
                            MonoBehaviour.Destroy(quad);
                            return false;
                        }
                    }
                }

            }
        }

        //controlla che tutta la riga attuale sia riempita
        for (int k = 0; k < larghezzaLivelli; k++) {
            bool found = false;
            foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
            {
                if (quad.GetComponent<Quad>().getState() == 2)
                {
                    Vector3 check = new Vector3(k + 0.5f, altezzaTotale + 0.5f);
                    if (quad.GetComponent<Quad>().ContainPoint(check))
                    {
                        found = true;
                    }
                }
            }
            if (!found)
            {
                return false;
            }
        }
        altezzaTotale += altezzaLivelli;

        //controlla che non ci siano rettangoli larghi 1
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            if (quad.GetComponent<Quad>().getSize().x == 1)
            {
                return true;
            }
        }
        
        return false;
    }

    public static bool ControlloCalcoli()
    {
        bool check = true;
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            //impedisce di costruire altri Rettangoli
            if (quad.GetComponent<Quad>().getState() < 2)
            {
                MonoBehaviour.Destroy(quad);
                return false;
            }
            if (quad.GetComponent<QuadUI>().buttonX.image.color != new Color(0, 255, 0))
            {
                check = false;
            }
            if (quad.GetComponent<QuadUI>().buttonY.image.color != new Color(0, 255, 0))
            {
                check = false;
            }
            if (quad.GetComponent<QuadUI>().buttonA.image.color != new Color(0, 255, 0))
            {
                check = false;
            }
        }
        return check;
    }

    public static bool AggiungiRettangoloInvisibile()
    {
        Vector2 rectSize = new Vector2(larghezzaLivelli,altezzaTotale);

        GridScript gs = GameObject.FindObjectOfType<GridScript>();
        GameObject newQuad = MonoBehaviour.Instantiate(gs.quad);

        newQuad.GetComponent<Quad>().setColor(new Color(0,0,0));
        newQuad.transform.position = new Vector3(2, 0, 0);
        newQuad.GetComponent<Quad>().setSize(rectSize);
        newQuad.GetComponent<Quad>().changeState();

        newQuad.GetComponent<QuadUI>().operation_reference.transform.Find("Number1").GetComponent<UnityEngine.UI.InputField>().text = "0";
        newQuad.GetComponent<QuadUI>().operation_reference.transform.Find("Number2").GetComponent<UnityEngine.UI.InputField>().text = "0";
        newQuad.GetComponent<QuadUI>().operation_reference.transform.Find("Number3").GetComponent<UnityEngine.UI.InputField>().text = "0";

        newQuad.transform.Find("QuadUI").Find("ButtonX").transform.position = new Vector3(0, 0, 10);
        newQuad.transform.Find("QuadUI").Find("ButtonY").transform.position = new Vector3(0, 0, 10);
        newQuad.transform.Find("QuadUI").Find("ButtonA").transform.position = new Vector3(0, 0, 10);

        newQuad.GetComponent<MeshRenderer>().enabled = false;

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
            if (quad.GetComponent<Quad>().getSize().y == altezzaTotale)
            {
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
        }
        return false;
    }

    public static bool ApriImpostazioni()
    {
        return GameObject.FindObjectOfType<Menu_Interaction>().isMenuOpen();
    }
}
