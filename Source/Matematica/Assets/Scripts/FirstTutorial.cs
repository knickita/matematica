using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class FirstTutorial
{
    static List<TutorialManager.TutorialStep> tutorialSteps;

    static void AddStep(string toolsEnabled, string text, float targetPositionX, float targetPositionY, float bubblePositionX, float bubblePositionY, System.Func<bool> endCondition, string tag = "")
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

        EnableCameraMove(false);
        ChangeMaxAvailableQuads(1);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorTopLeft);
        AddStep("draw", "Benvenuto in Rectables\nPer cominciare a disegnare fai click sulla MATITA",85,-150,280,-150,SelezionaDraw);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorBottomLeft);
        AddStep("draw", "Molto bene!\nOra tieni premuto sulla GRIGLIA e trascina verso destra", 389, 45, 179, 45, EsisteQuadratoDaFinire);
        AddStep("draw", "Ora tieni premuto sulla RIGA COLORATA e trascina in alto", -372, 45, 194, 45, EsisteQuadratoFinito);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorTopLeft);
        AddStep("none", "Complimenti!\n\nHai appena creato il tuo primo RETTANGOLO!\n\nFai click sullo schermo per continuare", 283, -57, 283, -57, ClickRilevato);
        AddStep("delete", "Per cancellare seleziona la GOMMA", 88, -50, 240, -50, SelezionaDelete);
        AddStep("delete", "Eccellente!\n\n\nAdesso fai click sul tuo RETTANGOLO", 283, -50, 283, -57, NessunQuadratoPresente);
        AddStep("draw,delete", "Hai cancellato il RETTANGOLO!\n\nOttimo!\n\nProva a disegnare un RETTANGOLO largo 7 e alto 6", 283, -57, 283, -66, EsisteQuadratoFinito7x6);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorCustom1);
        AddStep("draw", "Adesso inseriamo le misure.\nCon la MATITA selezionata fai click sul RETTANGOLO", 0, 174, 0, 200, ControlloSelezione);
        AddStep("draw", "Ora fai click su QUESTO pulsante per inserire il valore della larghezza", 0, 20, 0, 200, VirtualKeyboardVisibile);
        AddStep("draw", "Questa è la TASTIERA!\n\nLa TASTIERA può essere spostata trascinando i bordi\n\nInserisci i numeri premendo i tasti\n\nUsa la X rossa per cancellare\n\nQuando pensi che il numero sia corretto premi la V verde e la tastiera scomparirà", 0, 200, 0, 200, ControlloNumeroInserito1);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorCustom2);
        AddStep("draw", "Bene!\n\nOra inserisci l'altezza\n\nRicorda di fare click sul rettangolo\nper far apparire i comandi delle misure.", 20, 0, 200, 0, ControlloNumeroInserito2);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorBottomRight);
        AddStep("settings", "Adesso ti insegno un altro modo per inserire i numeri\n\nApri il menù delle IMPOSTAZIONI", -90, 54, -290, 54, ApriImpostazioni);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorCustom3);
        AddStep("settings", "Ora seleziona FINESTRA ARITMETICA", -169, 0, -335, 0, FinestraAritmenticaAperta);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorBottomRight);
        AddStep("settings", "E poi chiudi il menù delle IMPOSTAZIONI cliccando di nuovo il bottone", -90, 54, -350, 54, ChiudiImpostazioni);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorCustom4);
        AddStep("draw", "Adesso seleziona questa casella\nPoi completa l'uguaglianza", -8, -26, -93, -203, ControlloNumeroInserito3);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorCustom5);
        AddStep("", "Esatto, ottimo lavoro!\n\nRicorda che, usando la MATITA, puoi scrivere o modificare il prodotto anche cliccando QUA\n\nPer proseguire fai click in un qualunque punto dello schermo", -19, -17, -239, -122, ClickRilevato);
        ChangeMaxAvailableQuads(2);
        AddStep("none", "", 0, 0, 0, 0, ChangeUIAnchorTopLeft);        
        AddStep("cut", "Per facilitare il calcolo del prodotto, puoi spezzare un fattore e tagliare il RETTANGOLO\n\nSeleziona le FORBICI", 61, -345, 376, -345, SelezionaCut);
        AddStep("", "", 0, 0, 0, 0, ChangeUIAnchorBottomLeft);
        AddStep("cut", "Adesso fai click sul RETTANGOLO in modo da spezzare il lato da 7 in due lati da 5 e da 2", -372, 45, 290, 45, ControlloTaglio);
        AddStep("draw,settings", "Molto bene! Ora completa inserendo i fattori e i prodotti dei nuovi RETTANGOLI!\nSe vuoi scrivere direttamente su un RETTANGOLO ricorda di selezionare prima la MATITA", -131, 32, 290, 32, ControlloNumeroInserito4);
        AddStep("none", "", 0, 0, 0, 0, ChangeUIAnchorTopLeft);
        AddStep("move", "Corretto!\n\nOra ti insegnerò un nuovo comando:\n\nil comando MUOVI!   Selezionalo", 69, -250, 245, -250, SelezionaMove);
        EnableCameraMove(true);
        AddStep("move", "Adesso prova a spostare un RETTANGOLO\nSe ti serve più spazio puoi spostarti col tasto destro del mouse e fare uno zoom con la rotella\n\nProva a lasciare un po' di spazio tra i due rettangoli", 426, -40, 426, -46, ControlloMovimento);
        AddStep("move,rotate", "Ora seleziona lo strumento RUOTA", 79, -549, 230, -549, SelezionaRotate);
        AddStep("move,rotate", "Ruota il RETTANGOLO 2x6\nSe non ti basta lo spazio puoi sempre usare il comando MUOVI\n\nRicorda che puoi anche spostarti col tasto destro del mouse e fare zoom con la rotella", 443, -40, 443, -45, ControlloMovimento2a);
        AddStep("move,rotate", "Ottimo!\n\n Ora Ruota il RETTANGOLO 5x6\nSe non ti basta lo spazio puoi sempre usare il comando MUOVI\n\nRicorda che puoi anche spostarti col tasto destro del mouse e fare zoom con la rotella", 443, -45, 443, -55, ControlloMovimento2);
        AddStep("none,move,draw,settings", "Molto bene! Però adesso la larghezza e l’altezza dei rettangoli sono cambiate\n\nRiesci a correggerle?", 442, -5, 443, -34, ControlloNumeroInserito4);
        AddStep("none", "Benissimo!\nAbbiamo quasi finito\nDobbiamo solo imparare il comando COLLA\nFai click sullo schermo per continuare...", 443, -45, 443, -41, ClickRilevato);
        AddStep("none,move", "Per poter incollare due RETTANGOLI devi prima far combaciare un lato.\n\nUsa il comando MUOVI per spostare i RETTANGOLI uno sotto l'altro,\nin modo da far combaciare il lato da 6", 443, -40, 443, -45, ControlloMovimento3);
        AddStep("none,glue", "Fantastico! Ora non resta che selezionare il comando COLLA", 93, -446, 325, -446, SelezionaGlue);
        AddStep("glue", "Fai click tra i due RETTANGOLI per INCOLLARLI", 443, 24, 443, -13, ControlloIncolla);
        ChangeMaxAvailableQuads(1);
        AddStep("none,draw,settings", "Ottimo lavoro! Vedi che 6x7 = 6x5 + 6x2 ?\nCompleta inserendo i fattori e il prodotto del nuovo rettangolo", 443, -5, 443, -26, ControlloNumeroInserito5);
        AddStep("rotate", "Per finire, ruota il RETTANGOLO un'ultima volta", 443, 10, 443, -13, ControlloMovimento4);
        AddStep("none,delete", "Così sei tornato al punto di partenza!\nPer terminare il Tutorial e cominciare ad usare il programma CANCELLA il rettangolo", 443, -45, 443, -27, ControlloCancella);
        ChangeMaxAvailableQuads(10);


        return tutorialSteps;
    }

    public static bool SelezionaDraw()
    {
        return GameObject.FindObjectOfType<ToolBox>().IsDrawSelected();
    }
    public static bool SelezionaDelete()
    {
        return GameObject.FindObjectOfType<ToolBox>().IsDeleteSelected();
    }
    public static bool SelezionaCut()
    {
        return GameObject.FindObjectOfType<ToolBox>().IsCutSelected();
    }
    public static bool SelezionaMove()
    {
        return GameObject.FindObjectOfType<ToolBox>().IsMoveSelected();
    }
    public static bool SelezionaRotate()
    {
        return GameObject.FindObjectOfType<ToolBox>().IsRotateSelected();
    }
    public static bool SelezionaGlue()
    {
        return GameObject.FindObjectOfType<ToolBox>().IsGlueSelected();
    }
    public static bool ApriImpostazioni()
    {
        return GameObject.FindObjectOfType<Menu_Interaction>().isMenuOpen();
    }
    public static bool ChiudiImpostazioni()
    {
        return !GameObject.FindObjectOfType<Menu_Interaction>().isMenuOpen();
    }
    public static bool FinestraAritmenticaAperta()
    {
        return GameObject.FindObjectOfType<AritmeticWindow>().isVisible();
    }

    public static bool EsisteQuadratoDaFinire()
    {
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            if (quad.GetComponent<Quad>().getState() == 1)
            {
                return true;
            }
        }
        return false;
    }

    public static bool EsisteQuadratoFinito()
    {
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            if (quad.GetComponent<Quad>().getState() == 2)
            {
                return true;
            }
        }
        return false;
    }
    public static bool EsisteQuadratoFinito7x6()
    {
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            if (quad.GetComponent<Quad>().getState() == 2)
            {
                if (quad.GetComponent<Quad>().getSize() == new Vector2(7, 6))
                {
                    return true;
                }
                else
                {
                    GameObject.Destroy(quad);
                }
            }
        }
        return false;
    }

    public static bool NessunQuadratoPresente()
    {
        return GameObject.FindGameObjectsWithTag("quad").Length==0;
    }

    public static bool ClickRilevato()
    {
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            quad.GetComponent<QuadUI>().DisableInput();
        }
        return Input.GetMouseButtonUp(0);
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
    public static bool ChangeUIAnchorCustom1()
    {
        RectTransform transform = GameObject.FindObjectOfType<QuadUI>().labelX.rectTransform;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(screenPosition.x/Camera.main.pixelWidth, screenPosition.y/Camera.main.pixelHeight);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        return true;
    }
    public static bool ChangeUIAnchorCustom2()
    {
        RectTransform transform = GameObject.FindObjectOfType<QuadUI>().labelY.rectTransform;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        return true;
    }
    public static bool ChangeUIAnchorCustom3()
    {
        RectTransform transform = (RectTransform)GameObject.Find("settings_view_aritmetic_window").transform;
        Vector3 screenPosition = transform.position;
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        return true;
    }
    public static bool ChangeUIAnchorCustom4()
    {
        RectTransform transform = (RectTransform)GameObject.Find("Number3").transform;
        Vector3 screenPosition = transform.position;
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        return true;
    }
    public static bool ChangeUIAnchorCustom5()
    {
        RectTransform transform = GameObject.FindObjectOfType<QuadUI>().labelA.rectTransform;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMin = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        GameObject.Find("TextBubble").GetComponent<RectTransform>().anchorMax = new Vector2(screenPosition.x / Camera.main.pixelWidth, screenPosition.y / Camera.main.pixelHeight);
        return true;
    }

    public static bool VirtualKeyboardVisibile()
    {
        return GameObject.FindObjectOfType<VirtualKeyboard>().isVisible();
    }

    public static bool ControlloSelezione()
    {
        return GameObject.FindObjectOfType<QuadUI>().IsInputEnabled();
    }

    public static bool ControlloNumeroInserito1()
    {
        if (!GameObject.FindObjectOfType<VirtualKeyboard>().isVisible())
        {
            if(GameObject.FindObjectOfType<QuadUI>().buttonX.image.color == new Color(0, 255, 0))
            {
                return true;
            }
        }
        return false;
    }
    public static bool ControlloNumeroInserito2()
    {
        if (!GameObject.FindObjectOfType<VirtualKeyboard>().isVisible())
        {
            if (GameObject.FindObjectOfType<QuadUI>().buttonY.image.color == new Color(0, 255, 0))
            {
                return true;
            }
        }
        return false;
    }
    public static bool ControlloNumeroInserito3()
    {
        if (!GameObject.FindObjectOfType<VirtualKeyboard>().isVisible())
        {
            if (GameObject.FindObjectOfType<QuadUI>().buttonA.image.color == new Color(0, 255, 0))
            {
                return true;
            }
        }
        return false;
    }
    public static bool ControlloNumeroInserito4()
    {
        Quad[] quads = GameObject.FindObjectsOfType<Quad>();
        if (quads.Length>1)
        {
            if (quads[0].GetComponent<QuadUI>().buttonX.image.color != new Color(0, 255, 0)) { return false; }
            if (quads[0].GetComponent<QuadUI>().buttonY.image.color != new Color(0, 255, 0)) { return false; }
            if (quads[0].GetComponent<QuadUI>().buttonA.image.color != new Color(0, 255, 0)) { return false; }
            if (quads[1].GetComponent<QuadUI>().buttonX.image.color != new Color(0, 255, 0)) { return false; }
            if (quads[1].GetComponent<QuadUI>().buttonY.image.color != new Color(0, 255, 0)) { return false; }
            if (quads[1].GetComponent<QuadUI>().buttonA.image.color != new Color(0, 255, 0)) { return false; }
            return true;
        }
        return false;
    }
    public static bool ControlloNumeroInserito5()
    {
        Quad[] quads = GameObject.FindObjectsOfType<Quad>();
        if (quads[0].GetComponent<QuadUI>().buttonX.image.color != new Color(0, 255, 0)) { return false; }
        if (quads[0].GetComponent<QuadUI>().buttonY.image.color != new Color(0, 255, 0)) { return false; }
        if (quads[0].GetComponent<QuadUI>().buttonA.image.color != new Color(0, 255, 0)) { return false; }
        return true;

    }
    public static bool ControlloTaglio()
    {
        Quad[] quads = GameObject.FindObjectsOfType<Quad>();
        if (quads.Length>1)
        {
            if(quads[0].getSize() == new Vector2(2, 6) || quads[0].getSize() == new Vector2(5, 6))
            {
                if (quads[1].getSize() == new Vector2(2, 6) || quads[1].getSize() == new Vector2(5, 6))
                {
                    return true;
                }
            }

            GameObject.Destroy(quads[0].gameObject);
            quads[1].setSize(new Vector2(7, 6));
        }
        return false;
    }
    public static bool ControlloMovimento()
    {
        Quad[] quads = GameObject.FindObjectsOfType<Quad>();
        if (quads.Length > 1)
        {
            if (quads[0].transform.localPosition.x + quads[0].getSize().x < quads[1].transform.localPosition.x) { return true; }
            if (quads[0].transform.localPosition.y + quads[0].getSize().y < quads[1].transform.localPosition.y) { return true; }
            if (quads[1].transform.localPosition.x + quads[1].getSize().x < quads[0].transform.localPosition.x) { return true; }
            if (quads[1].transform.localPosition.y + quads[1].getSize().y < quads[0].transform.localPosition.y) { return true; }
        }
        return false;
    }
    public static bool ControlloMovimento2()
    {
        Quad[] quads = GameObject.FindObjectsOfType<Quad>();
        if (quads.Length > 1)
        {
            if (quads[0].getSize() == new Vector2(2, 6))
            {
                quads[0].setSize(new Vector2(6, 2));
            }
            if (quads[1].getSize() == new Vector2(2, 6))
            {
                quads[1].setSize(new Vector2(6, 2));
            }
            if (quads[0].getSize() == new Vector2(6, 5) || quads[1].getSize() == new Vector2(6, 5))
            {
                return true;
            }
        }
        return false;
    }
    public static bool ControlloMovimento2a()
    {
        Quad[] quads = GameObject.FindObjectsOfType<Quad>();
        if (quads.Length > 1)
        {
            if (quads[0].getSize() == new Vector2(6, 5))
            {
                quads[0].setSize(new Vector2(5, 6));
            }
            if (quads[1].getSize()==new Vector2(6, 5))
            {
                quads[1].setSize(new Vector2(5, 6));
            }
            if (quads[0].getSize() == new Vector2(6, 2) || quads[1].getSize() == new Vector2(6, 2))
            {
                return true;
            }
        }
        return false;
    }
    public static bool ControlloMovimento3()
    {
        Quad[] quads = GameObject.FindObjectsOfType<Quad>();
        if (quads.Length > 1)
        {
            if (quads[0].transform.localPosition.x== quads[1].transform.localPosition.x)
            {
                if (quads[0].transform.localPosition.y + quads[0].getSize().y == quads[1].transform.localPosition.y) { return true; }
                if (quads[1].transform.localPosition.y + quads[1].getSize().y == quads[0].transform.localPosition.y) { return true; }
            }
        }
        return false;
    }
    public static bool ControlloMovimento4()
    {
        return GameObject.FindObjectOfType<Quad>().getSize() == new Vector2(7, 6);
    }
    public static bool ControlloIncolla()
    {
        return GameObject.FindObjectsOfType<Quad>().Length == 1;
    }
    public static bool ControlloCancella()
    {
        return GameObject.FindObjectsOfType<Quad>().Length == 0;
    }
}
