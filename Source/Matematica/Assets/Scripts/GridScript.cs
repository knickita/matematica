using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScript : MonoBehaviour
{
    public float width;
    public float height;
    public GameObject quad;
    
    public Material gridMaterial;

    public List<Color> available_colors;

    private Camera cam;
    private ToolBox toolBox;

    public GameObject quadSelected;
    

    public bool gridVisibility = true;
    private TutorialManager tutorialManager;

    //used to restore a color if not used
    Color lastColorPicked;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        toolBox = GameObject.Find("Toolbox").GetComponent<ToolBox>();
        tutorialManager = GameObject.FindObjectOfType<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x > 102 || Input.mousePosition.y < Screen.height-602)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MyOnMouseDown();
            }
            if (Input.GetMouseButtonUp(0))
            {
                MyOnMouseUp();
            }
        }
    }
    
    private void OnRenderObject()
    {
        if (gridVisibility)
        {
            GL.Begin(GL.LINES);
            gridMaterial.SetPass(0);
            for (int i = 0; i < width + 1; i++)
            {
                GL.Vertex3(i, 0, -2);
                GL.Vertex3(i, height, -2);
            }
            for (int i = 0; i < height + 1; i++)
            {
                GL.Vertex3(0, i, -2);
                GL.Vertex3(width, i, -2);
            }
            GL.End();
        }

        //set the position and the size of the grid
        transform.position = new Vector3(width / 2, height / 2, 10);
        transform.localScale = new Vector3(width / 10, 0, height / 10);
    }
 

    private void MyOnMouseDown()
    {
        Vector2 pos = Input.mousePosition;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cam.nearClipPlane));

        if (toolBox.IsDrawSelected())
        {
            point.x = Mathf.FloorToInt(point.x);
            point.y = Mathf.FloorToInt(point.y);
            point.z = 0;
            foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
            {
                if (quad.GetComponent<Quad>().ContainPoint(point))
                {
                    quad.GetComponent<Quad>().SetClicked(true, point);
                }
            }
            if (!AreYouBuildingAnotherQuad())
            {
                if (point.x >= 0 && point.x<width && point.y >= 0 && point.y<height)
                {
                    Color color = pickRandomColor();
                    if (color != Color.clear)
                    {
                        GameObject new_quad = Instantiate(quad, point, Quaternion.Euler(0, 0, 0));
                        new_quad.GetComponent<Quad>().setColor(color);
                    }
                }
            }
        }
        else if (toolBox.IsDeleteSelected())
        {
            foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
            {
                if (quad.GetComponent<Quad>().ContainPoint(point))
                {
                    Destroy(quad);
                }
            }
        }
        else if (toolBox.IsMoveSelected())
        {
            point.x = Mathf.FloorToInt(point.x);
            point.y = Mathf.FloorToInt(point.y);
            point.z = 0;

            foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
            {
                if (quad.GetComponent<Quad>().ContainPoint(point))
                {
                    quad.GetComponent<Quad>().SetClicked(true, point);
                }
            }
        }
    }

    private void MyOnMouseUp()
    {
        Vector2 pos = Input.mousePosition;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cam.nearClipPlane));

        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {

            if (quad.GetComponent<Quad>().isClicked())
            {
                if (toolBox.IsDrawSelected())
                {
                    if (quad.GetComponent<Quad>().getState() == 2)
                    {
                        quad.GetComponent<QuadUI>().EnableInput();
                    }
                }
            }
            else
            {
                quad.GetComponent<QuadUI>().DisableInput();
            }


            quad.GetComponent<Quad>().SetClicked(false);



            if (toolBox.IsRotateSelected())
            {
                if (quad.GetComponent<Quad>().ContainPoint(point))
                {
                    quad.GetComponent<Quad>().Rotate90Degrees();
                }
            }
            else if (toolBox.IsGlueSelected())
            {
                if (quad.GetComponent<Quad>().ContainPoint(point))
                {
                    Vector2 quadCenter = new Vector2(quad.transform.position.x, quad.transform.position.y);
                    quadCenter+= quad.GetComponent<Quad>().getSize() / 2;

                    Vector2 mouseDelta = new Vector2(point.x, point.y) - quadCenter;                    
                    //normalize MouseDelta individual axes to be in -1/+1 range
                    mouseDelta /= quad.GetComponent<Quad>().getSize();

                    //rispetto al centro posso trovarmi a sinistra/destra/alto/basso... 
                    if (Mathf.Abs(mouseDelta.x) > Mathf.Abs(mouseDelta.y))
                    {
                        mouseDelta.y = 0;
                    }
                    else
                    {
                        mouseDelta.x = 0;
                    }

                    mouseDelta.Normalize();
                    //not checking ==1 to avoid float rounding problems caused by Normalize
                    if (mouseDelta.x >0.5)
                    {
                        mouseDelta.x = quad.GetComponent<Quad>().getSize().x;
                    }
                    if (mouseDelta.y>0.5)
                    {
                        mouseDelta.y = quad.GetComponent<Quad>().getSize().y;
                    }
                    Vector3 searchPosition=quad.transform.position+new Vector3(mouseDelta.x,mouseDelta.y,0);
                    //controllare se in quella direzione c'è un'altro Quad attaccato con dimensioni compatibili
                    GameObject otherQuad = null;

                    foreach (GameObject obj in GameObject.FindGameObjectsWithTag("quad"))
                    {
                        if (obj.GetComponent<Quad>().ContainPoint(searchPosition))
                        {
                            otherQuad = obj;
                        }
                    }

                    if (otherQuad != null && otherQuad!=quad)
                    {
                        if (Mathf.Abs(mouseDelta.y) < 0.5)
                        {
                            if (quad.GetComponent<Quad>().getSize().y == otherQuad.GetComponent<Quad>().getSize().y)
                            {
                                if (quad.transform.position.y == otherQuad.transform.position.y)
                                {
                                    Vector2 newSize = quad.GetComponent<Quad>().getSize();
                                    newSize.x+= otherQuad.GetComponent<Quad>().getSize().x;
                                    if (quad.transform.position.x < otherQuad.transform.position.x)
                                    {
                                        quad.GetComponent<Quad>().setSize(newSize);
                                        Destroy(otherQuad);
                                    }
                                    else
                                    {
                                        otherQuad.GetComponent<Quad>().setSize(newSize);
                                        Destroy(quad);
                                    }
                                }
                            }
                        }
                        else if (quad.GetComponent<Quad>().getSize().x == otherQuad.GetComponent<Quad>().getSize().x)
                        {
                            if (quad.transform.position.x == otherQuad.transform.position.x)
                            {
                                Vector2 newSize = quad.GetComponent<Quad>().getSize();
                                newSize.y += otherQuad.GetComponent<Quad>().getSize().y;
                                if (quad.transform.position.y < otherQuad.transform.position.y)
                                {
                                    quad.GetComponent<Quad>().setSize(newSize);
                                    Destroy(otherQuad);
                                }
                                else
                                {
                                    otherQuad.GetComponent<Quad>().setSize(newSize);
                                    Destroy(quad);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    bool AreYouBuildingAnotherQuad()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("quad"))
        {
            if (!obj.GetComponent<Quad>().IsComplete())
            {
                return true;
            }
        }
        return false;
    }
    
    public Color pickRandomColor()
    {
        if (available_colors.Count == 0)
        {
            return Color.clear;
        }
        if (tutorialManager != null)
        {
            if (GameObject.FindGameObjectsWithTag("quad").Length >= tutorialManager.MaxAvailableQuads())
            {
                return Color.clear;
            }
        }

        int select = Mathf.FloorToInt(Random.Range(0, available_colors.Count - 0.1f));
        Color color = available_colors[select];
        available_colors.RemoveAt(select);

        lastColorPicked = color;

        return color;
    }    
}
