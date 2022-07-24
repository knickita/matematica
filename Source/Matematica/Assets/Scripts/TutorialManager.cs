using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialType
    {
        FirstTutorial,
        Muretti,
        Incollali,
        Tetris
    }

    public GameObject bubble;
    public GameObject arrowTip;
    public TutorialType tutorialType = TutorialType.FirstTutorial;
    List<TutorialStep> steps;
    
    [HideInInspector]
    public TutorialStep actualTutorialStep;

    private int currentTutorialStepIndex = 0;
    private int maxAvailableQuads = 7;
    private bool cameraMovementEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        switch (tutorialType)
        {
            case TutorialType.FirstTutorial: steps = FirstTutorial.Load(); break;
            case TutorialType.Muretti: steps = MurettiTutorial.Load(); break;
            case TutorialType.Incollali: steps = IncollaliTutorial.Load(); break;
            case TutorialType.Tetris: steps = TetrisTutorial.Load(); break;
        }
        
        loadTutorialStep(0);
    }

    void loadTutorialStep(int index)
    {
        if (index < 0)
        {
            Debug.LogWarning("Cannot set Tutorial index to negative numbers! passed value: "+index.ToString());
            return;
        }
        if (index >= steps.Count)
        {
            Debug.LogWarning("Tutorial index is too high!, tutorial Turned OFF passed value: " + index.ToString());
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            index = 0;
            return;
        }

        currentTutorialStepIndex = index;
        actualTutorialStep = steps[currentTutorialStepIndex];

        arrowTip.GetComponent<RectTransform>().localPosition = actualTutorialStep.targetPosition;
        bubble.GetComponent<RectTransform>().localPosition = actualTutorialStep.bubblePosition;
        bubble.GetComponentInChildren<UnityEngine.UI.Text>().text = actualTutorialStep.text;

        if (actualTutorialStep.unselectAllTools)
        {
            FindObjectOfType<ToolBox>().SelectNoTool();
        }

        if (actualTutorialStep.changemaxAvailableQuads > -1)
        {
            maxAvailableQuads = actualTutorialStep.changemaxAvailableQuads;
        }

        if (actualTutorialStep.changeEnableCameraMove == "true")
        {
            cameraMovementEnabled = true;
        }
        else if (actualTutorialStep.changeEnableCameraMove == "false")
        {
            cameraMovementEnabled = false;
        }

        if (actualTutorialStep.goToStep != "")
        {
            for (int i = 0; i < steps.Count; i++)
            {
                if (steps[i].tag == actualTutorialStep.goToStep)
                {
                    loadTutorialStep(i);
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (actualTutorialStep.endCondition())
        {
            loadTutorialStep(currentTutorialStepIndex + 1);
        }       
    }

    public bool isDeleteEnabled()
    {
        return actualTutorialStep.enableDeleteTool;
    }
    public bool isDrawEnabled()
    {
        return actualTutorialStep.enableDrawTool;
    }
    public bool isMoveEnabled()
    {
        return actualTutorialStep.enableMoveTool;
    }
    public bool isCutEnabled()
    {
        return actualTutorialStep.enableCutTool;
    }
    public bool isGlueEnabled()
    {
        return actualTutorialStep.enableGlueTool;
    }
    public bool isRotateEnabled()
    {
        return actualTutorialStep.enableRotateTool;
    }
    public bool isSettingsEnabled()
    {
        return actualTutorialStep.enableSettingsButton;
    }

    public int MaxAvailableQuads()
    {
        return maxAvailableQuads;
    }

    public bool CameraMovementEnabled()
    {
        return cameraMovementEnabled;
    }



    [System.Serializable]
    public class TutorialStep
    {
        public bool enableDeleteTool;
        public bool enableDrawTool;
        public bool enableMoveTool;
        public bool enableCutTool;
        public bool enableGlueTool;
        public bool enableRotateTool;
        public bool enableSettingsButton;

        public bool unselectAllTools;

        public int changemaxAvailableQuads;
        public string changeEnableCameraMove;


        public System.Func<bool> endCondition;

        public string text;
        public Vector2 targetPosition;
        public Vector2 bubblePosition;

        //used to mark the step with a specific name
        public string tag;
        public string goToStep = "";
    }

}

