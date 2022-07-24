using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AritmeticWindow : MonoBehaviour
{

    public Toggle view_toggle;
    public GameObject operation;
    public List<GameObject> list_of_operations = new List<GameObject>();
    private Vector3 visiblePosition;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<CanvasRenderer>().SetAlpha(0);
        view_toggle.onValueChanged.AddListener(switchVisibility);
        visiblePosition = transform.position;
        transform.position = visiblePosition + new Vector3(0, 10000, 0);

        switchVisibility(view_toggle.isOn);
    }

    // Update is called once per frame
    void Update()
    {
        float y_position = 300;
        foreach (GameObject o in list_of_operations)
        {            
            y_position -= operation.GetComponent<RectTransform>().rect.height * operation.GetComponent<RectTransform>().localScale.y * 1.1f;
            o.transform.localPosition=new Vector3(0, y_position, 0);
        }
    }

    //crea una nuova operazione, e torna un puntatore all'operazione creata
    public GameObject addOperation(GameObject referenceQuad)
    {
        GameObject new_operation = Instantiate(operation);
        new_operation.transform.SetParent(transform, true);
        list_of_operations.Add(new_operation);
        new_operation.transform.Find("Number1").GetComponent<OperationInputField>().referenceQuad = referenceQuad;
        new_operation.transform.Find("Number2").GetComponent<OperationInputField>().referenceQuad = referenceQuad;
        new_operation.transform.Find("Number3").GetComponent<OperationInputField>().referenceQuad = referenceQuad;


        return new_operation;
    }

    public void removeOperation(GameObject oper)
    {
        list_of_operations.Remove(oper);
        Destroy(oper);
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

    public bool isVisible()
    {
        return transform.position.y < 10000;
    }
}