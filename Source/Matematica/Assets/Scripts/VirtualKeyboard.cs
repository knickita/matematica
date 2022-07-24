using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualKeyboard : MonoBehaviour
{
    public Toggle view_toggle;
    
    private Vector3 visiblePosition;
    private int actualValue=0;
    private InputField caller=null;
    
    //used to drag&drop the keyboard
    private Vector2 translationDelta;

    public Button button1, button2, button3, button4, button5, button6, button7, button8, button9, button0, buttonCanc, buttonOk;
    
    [HideInInspector]
    public bool isEnabled;


    // Start is called before the first frame update
    void Start()
    {
        view_toggle.onValueChanged.AddListener(switchAbilitation);
        isEnabled = view_toggle.isOn;

        visiblePosition = transform.position;
        transform.position = visiblePosition + new Vector3(0, 10000, 0);

        button1.onClick.AddListener(Button1OnClick);
        button2.onClick.AddListener(Button2OnClick);
        button3.onClick.AddListener(Button3OnClick);
        button4.onClick.AddListener(Button4OnClick);
        button5.onClick.AddListener(Button5OnClick);
        button6.onClick.AddListener(Button6OnClick);
        button7.onClick.AddListener(Button7OnClick);
        button8.onClick.AddListener(Button8OnClick);
        button9.onClick.AddListener(Button9OnClick);
        button0.onClick.AddListener(Button0OnClick);
        buttonCanc.onClick.AddListener(ButtonCancOnClick);
        buttonOk.onClick.AddListener(ButtonOkOnClick);
    }

    public void ShowKeyboard(InputField cal)
    {
        caller = cal;
        switchVisibility(true);     
    }

    public bool isVisible()
    {
        return transform.position.y <10000;
    }

    void switchVisibility(bool state)
    {
        if (state)
        {
            transform.position = visiblePosition;
        }
        else
        {
            transform.position = visiblePosition + new Vector3(0, 10000, 0);
        }
    }

    public void Update()
    {
        if (caller != null)
        {
            actualValue = 0;
            if (caller.text != "")
            {
                actualValue = int.Parse(caller.text);
            }
            //keeps the quad on Focus while the Virtual Kewyboard is active
            caller.gameObject.GetComponent<OperationInputField>().referenceQuad.GetComponent<QuadUI>().EnableInput();

            if (!caller.isFocused)
            {
                CheckKeyboardPressed();
            }
        }
    }

    void CheckKeyboardPressed()
    {
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            ButtonOkOnClick();
        }
        if (Input.GetKeyUp(KeyCode.Delete) || Input.GetKeyUp(KeyCode.Backspace))
        {
            ButtonCancOnClick();
        }

        if (Input.GetKeyUp(KeyCode.Alpha0) || Input.GetKeyUp(KeyCode.Keypad0))
        {
            Button0OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1))
        {
            Button1OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Keypad2))
        {
            Button2OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha3) || Input.GetKeyUp(KeyCode.Keypad3))
        {
            Button3OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha4) || Input.GetKeyUp(KeyCode.Keypad4))
        {
            Button4OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha5) || Input.GetKeyUp(KeyCode.Keypad5))
        {
            Button5OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha6) || Input.GetKeyUp(KeyCode.Keypad6))
        {
            Button6OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha7) || Input.GetKeyUp(KeyCode.Keypad7))
        {
            Button7OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha8) || Input.GetKeyUp(KeyCode.Keypad8))
        {
            Button8OnClick();
        }
        if (Input.GetKeyUp(KeyCode.Alpha9) || Input.GetKeyUp(KeyCode.Keypad9))
        {
            Button9OnClick();
        }
    }

    //connected to the toggle in the settings menu, to choose when to use the virtual keyboard or not
    void switchAbilitation(bool state)
    {
        isEnabled = state;
    }

    void Button1OnClick()
    {
        actualValue *= 10;
        actualValue += 1;
        caller.text = actualValue.ToString();
    }
    void Button2OnClick()
    {
        actualValue *= 10;
        actualValue += 2;
        caller.text = actualValue.ToString();
    }
    void Button3OnClick()
    {
        actualValue *= 10;
        actualValue += 3;
        caller.text = actualValue.ToString();
    }
    void Button4OnClick()
    {
        actualValue *= 10;
        actualValue += 4;
        caller.text = actualValue.ToString();
    }
    void Button5OnClick()
    {
        actualValue *= 10;
        actualValue += 5;
        caller.text = actualValue.ToString();
    }
    void Button6OnClick()
    {
        actualValue *= 10;
        actualValue += 6;
        caller.text = actualValue.ToString();
    }
    void Button7OnClick()
    {
        actualValue *= 10;
        actualValue += 7;
        caller.text = actualValue.ToString();
    }
    void Button8OnClick()
    {
        actualValue *= 10;
        actualValue += 8;
        caller.text = actualValue.ToString();
    }
    void Button9OnClick()
    {
        actualValue *= 10;
        actualValue += 9;
        caller.text = actualValue.ToString();
    }
    void Button0OnClick()
    {
        actualValue *= 10;
        actualValue += 0;
        caller.text = actualValue.ToString();
    }
    void ButtonCancOnClick()
    {
        actualValue = 0;
        caller.text = actualValue.ToString();
    }
    void ButtonOkOnClick()
    {
        caller.text = actualValue.ToString();
        actualValue = 0;
        caller = null;
        switchVisibility(false);
    }

    public void StartDrag()
    {
        translationDelta = Input.mousePosition - transform.position;
    }

    public void Drag()
    {
        transform.position = (Vector2)Input.mousePosition - translationDelta;
        visiblePosition = transform.position;
    }
}
