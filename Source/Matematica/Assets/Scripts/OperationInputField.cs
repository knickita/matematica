using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OperationInputField : MonoBehaviour, IPointerClickHandler
{
    private VirtualKeyboard virtualKeyboard;
    public GameObject referenceQuad;
    // Start is called before the first frame update
    void Start()
    {
        virtualKeyboard = GameObject.Find("VirtualKeyboard").GetComponent<VirtualKeyboard>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (virtualKeyboard.isEnabled)
        {
            virtualKeyboard.ShowKeyboard(GetComponent<UnityEngine.UI.InputField>());
        }
    }
}
