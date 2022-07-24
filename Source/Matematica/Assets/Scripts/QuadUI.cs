using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class QuadUI : MonoBehaviour
{
    public Button buttonX, buttonY, buttonA;
    public Text labelX, labelY, labelA;

    private VirtualKeyboard virtualKeyboard;

    [HideInInspector]
    public GameObject operation_reference;

    private bool inputEnabled;

    private void Awake()
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        operation_reference = GameObject.Find("AritmeticWindow").GetComponent<AritmeticWindow>().addOperation(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {        
        operation_reference.GetComponent<UnityEngine.UI.Image>().color = GetComponent<Quad>().getColor();
        operation_reference.SetActive(false);

        virtualKeyboard = GameObject.Find("VirtualKeyboard").GetComponent<VirtualKeyboard>();


        buttonX.onClick.AddListener(delegate { buttonClicked(operation_reference.transform.Find("Number1").GetComponent<UnityEngine.UI.InputField>()); });
        buttonY.onClick.AddListener(delegate { buttonClicked(operation_reference.transform.Find("Number2").GetComponent<UnityEngine.UI.InputField>()); });
        buttonA.onClick.AddListener(delegate { buttonClicked(operation_reference.transform.Find("Number3").GetComponent<UnityEngine.UI.InputField>()); });
    }

    // Update is called once per frame
    void Update()
    {
        checkNumericvalues();
        labelX.text = operation_reference.transform.Find("Number1").GetComponent<UnityEngine.UI.InputField>().text;
        labelY.text = operation_reference.transform.Find("Number2").GetComponent<UnityEngine.UI.InputField>().text;
        labelA.text = operation_reference.transform.Find("Number3").GetComponent<UnityEngine.UI.InputField>().text;

        if (labelX.text == "") { labelX.text = "..."; }
        if (labelY.text == "") { labelY.text = "..."; }
        if (labelA.text == "") { labelA.text = "..."; }
    }

    

    public bool IsInputEnabled()
    {
        return inputEnabled;
    }

    void buttonClicked(InputField pointerToInputField)
    {
        //used to confirm the this object is currently being used
        EnableInput();


        if (virtualKeyboard.isEnabled)
        {
            virtualKeyboard.ShowKeyboard(pointerToInputField);
        }
    }

    public void EnableInput()
    {
        buttonX.enabled = true;
        buttonY.enabled = true;
        buttonA.enabled = true;
        buttonX.GetComponent<Image>().enabled = true;
        buttonY.GetComponent<Image>().enabled = true;
        buttonA.GetComponent<Image>().enabled = true;
        labelX.enabled = true;
        labelY.enabled = true;
        labelA.enabled = true;
        labelX.color = new Color(0, 0, 0);
        labelY.color = new Color(0, 0, 0);
        labelA.color = new Color(0, 0, 0);
        inputEnabled = true;
    }
    public void DisableInput()
    {
        buttonX.enabled = false;
        buttonY.enabled = false;
        buttonA.enabled = false;
        buttonX.GetComponent<Image>().enabled = false;
        buttonY.GetComponent<Image>().enabled = false;
        buttonA.GetComponent<Image>().enabled = false;
        labelX.enabled = false;
        labelY.enabled = false;
        labelA.enabled = false;

        //calculate luminosity and change text color accordingly
        Color color = GetComponent<Quad>().getColor();
        float parameter = (color.r + color.g + color.b)/(3.0f);
        if (parameter < 0.5f)
        {
            labelX.color = new Color(255, 255, 255);
            labelY.color = new Color(255, 255, 255);
            labelA.color = new Color(255, 255, 255);
        }
        else
        {
            labelX.color = new Color(0, 0, 0);
            labelY.color = new Color(0, 0, 0);
            labelA.color = new Color(0, 0, 0);
        }

        inputEnabled = false;
    }

    //checks for valid and correct values inserted
    void checkNumericvalues()
    {
        UnityEngine.UI.InputField inputX = operation_reference.transform.Find("Number1").GetComponent<UnityEngine.UI.InputField>();
        UnityEngine.UI.InputField inputY = operation_reference.transform.Find("Number2").GetComponent<UnityEngine.UI.InputField>();
        UnityEngine.UI.InputField inputA = operation_reference.transform.Find("Number3").GetComponent<UnityEngine.UI.InputField>();
        //clamp valid character between 0-9
        inputX.text = Regex.Replace(inputX.text, @"[^0-9]", "");
        inputY.text = Regex.Replace(inputY.text, @"[^0-9]", "");
        inputA.text = Regex.Replace(inputA.text, @"[^0-9]", "");

        //color TextLabels
        if (inputX.text.Length > 0)
        {
            if (int.Parse(inputX.text) == gameObject.GetComponent<Quad>().getSize().x)
            {
                inputX.image.color = new Color(0, 255, 0);
                buttonX.image.color = new Color(0, 255, 0);
                labelX.enabled = true;
            }
            else
            {
                inputX.image.color = new Color(255, 0, 0);
                buttonX.image.color = new Color(255, 0, 0);
            }
        }
        else
        {
            inputX.image.color = new Color(255, 255, 255);
            buttonX.image.color = new Color(255, 255, 255);
        }

        if (inputY.text.Length > 0)
        {
            if (int.Parse(inputY.text) == GetComponent<Quad>().getSize().y)
            {
                inputY.image.color = new Color(0, 255, 0);
                buttonY.image.color = new Color(0, 255, 0);
                labelY.enabled = true;
            }
            else
            {
                inputY.image.color = new Color(255, 0, 0);
                buttonY.image.color = new Color(255, 0, 0);
            }
        }
        else
        {
            inputY.image.color = new Color(255, 255, 255);
            buttonY.image.color = new Color(255, 255, 255);
        }

        if (inputA.text.Length > 0)
        {
            if (int.Parse(inputA.text) == GetComponent<Quad>().getSize().x * GetComponent<Quad>().getSize().y)
            {
                inputA.image.color = new Color(0, 255, 0);
                buttonA.image.color = new Color(0, 255, 0);
                labelA.enabled = true;
            }
            else
            {
                inputA.image.color = new Color(255, 0, 0);
                buttonA.image.color = new Color(255, 0, 0);
            }
        }
        else
        {
            inputA.image.color = new Color(255, 255, 255);
            buttonA.image.color = new Color(255, 255, 255);
        }

    }

    public void OnDestroy()
    {
        {
            GameObject.Find("AritmeticWindow").GetComponent<AritmeticWindow>().removeOperation(operation_reference);
        }
    }

}
