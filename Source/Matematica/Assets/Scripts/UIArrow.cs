using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArrow : MonoBehaviour
{
    public GameObject bubble;
    public GameObject arrowShaft;
    public GameObject arrowTip;
    public GameObject text;
    public float thickness=10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move the arrow shaft to connect the bubble with the tip
        float deltaX = bubble.GetComponent<RectTransform>().localPosition.x - arrowTip.GetComponent<RectTransform>().localPosition.x;
        float deltaY = bubble.GetComponent<RectTransform>().localPosition.y - arrowTip.GetComponent<RectTransform>().localPosition.y;
        float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

        float distance = new Vector2(deltaX, deltaY).magnitude-4.0f;

        arrowShaft.GetComponent<RectTransform>().localPosition = bubble.GetComponent<RectTransform>().localPosition;
        arrowShaft.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, angle);
        arrowShaft.GetComponent<RectTransform>().sizeDelta = new Vector2(distance, thickness);

        //rotate the tip of the arrow
        arrowTip.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, angle);
    }
}

