using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera cam;
    private Vector3 oldPoint;
    private TutorialManager tutorialManager;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        tutorialManager = GameObject.FindObjectOfType<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0) // zoom out
        {
            if (tutorialManager == null)
            {
                cam.orthographicSize++;
            }
            else if (tutorialManager.CameraMovementEnabled())
            {
                cam.orthographicSize++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) // zoom in
        {
            if (tutorialManager == null)
            {
                cam.orthographicSize--;
            }
            else if (tutorialManager.CameraMovementEnabled())
            {
                cam.orthographicSize--;
            }
        }
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 5, 20);

        Vector2 pos = Input.mousePosition;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cam.nearClipPlane));
        point -= transform.position;

        Vector3 translation = new Vector3(0, 0, 0);

        if (Input.GetMouseButtonDown(1))
        {
            oldPoint = new Vector3(point.x, point.y);
        }
        if (Input.GetMouseButton(1))
        {
            if (tutorialManager == null)
            {
                translation = point - oldPoint;
            }
            else if (tutorialManager.CameraMovementEnabled())
            {
                translation = point - oldPoint;
            }            
            oldPoint = point;
        }
        else
        {
            oldPoint = new Vector3(0, 0, 0);
        }

        translation.x = transform.position.x-translation.x;
        translation.y = transform.position.y - translation.y;
        translation.z = transform.position.z;

        transform.position = translation;
    }

}
