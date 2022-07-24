using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
    public Material referenceMaterial;

    private Camera cam;

    private float width = 0.1f;
    private float height = 0.1f;

    private ToolBox toolBox;
    private int state = 0;
    private bool clicked = false;
    private Vector3 pointClickedDelta;
    private float gridWidth, gridHeight;
    

    private Material material;

    //RectTransform of canvas used to render width height and Area Text Labels
    private RectTransform internalCanvasTransform;

    //used with cutTool to say to this quad that is already completly Formed
    public void changeState()
    {
        state = 2;
    }
    public int getState()
    {
        return state;
    }

    public void setColor(Color color)
    {
        material.color = color;
    }

    public Color getColor()
    {
        return material.color;
    }

    public Vector2 getSize()
    {
        return new Vector2(width, height);
    }

    public void setSize(Vector2 size)
    {
        width = size.x;
        height = size.y;
    }

    private void Awake()
    {
        material = new Material(referenceMaterial);
        GridScript gridScript = GameObject.FindObjectOfType<GridScript>();
        gridWidth = gridScript.width;
        gridHeight = gridScript.height;

        GetComponent<Renderer>().material = material;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        toolBox = GameObject.Find("Toolbox").GetComponent<ToolBox>();

        GameObject canvas = transform.Find("QuadUI").gameObject;

        canvas.transform.SetParent(transform);
        internalCanvasTransform = canvas.GetComponent<RectTransform>();
    }

    

    // Update is called once per frame
    void Update()
    {
        //update quad's position and size
        transform.localScale = new Vector3(width, height, 1);

        //update internal canvas size according to quad size
        //internalCanvasTransform.localPosition = new Vector3(gameObject.GetComponent<Quad>().getSize().x / 2, gameObject.GetComponent<Quad>().getSize().y / 2, 0);
        internalCanvasTransform.sizeDelta = new Vector3(gameObject.GetComponent<Quad>().getSize().x, gameObject.GetComponent<Quad>().getSize().y,1);
        internalCanvasTransform.transform.localScale = new Vector3(1.0f / transform.localScale.x, 1.0f / transform.localScale.y, 1);

       
        if (state < 2)
        {
            if (toolBox.IsDrawSelected())
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 pos = Input.mousePosition;
                    Vector3 point = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cam.nearClipPlane));
                                      
                    if (state == 0)
                    {
                        int x = Mathf.FloorToInt(point.x - transform.position.x);
                        if (x > 0 && x != width)
                        {
                            float temp = width;
                            ChangeWidth(x);
                            if (IntersectingOtherQuads() || OutOfGrid())
                            {
                                ChangeWidth(temp);
                            }
                        }
                    }
                    else if (state == 1 && clicked)
                    {
                        int y = Mathf.FloorToInt(point.y - transform.position.y);
                        if (y > 0 && y != height)
                        {
                            float temp = height;
                            ChangeHeight(y);
                            if (IntersectingOtherQuads() || OutOfGrid())
                            {
                                ChangeHeight(temp);
                            }
                        }
                    }

                }
                if (Input.GetMouseButtonUp(0))
                {                    
                    if (state == 0)
                    {
                        if (width > 0.2)
                        {
                            state = 1;
                            StartCoroutine(DestroyIfUnfinished());
                        }
                        else
                        {
                            Destroy(this.gameObject);
                        }
                    }
                    else if (state == 1 && height > 0.5)
                    {
                        state = 2;
                    }
                }
            }

        }
        else
        {
            GetComponent<QuadUI>().operation_reference.SetActive(true);
        }

        if (toolBox.IsMoveSelected() && clicked && Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cam.nearClipPlane));
            point.x = Mathf.FloorToInt(point.x);
            point.y = Mathf.FloorToInt(point.y);
            point.z = 0;
            if (point.x- pointClickedDelta.x >= 0 && point.y - pointClickedDelta .y>= 0)
            {
                if (point.x + width-pointClickedDelta.x <= gridWidth && point.y + height -pointClickedDelta.y<= gridHeight)
                {
                    Vector3 tempPosition = transform.position;
                    transform.position = point-pointClickedDelta;
                    if (IntersectingOtherQuads() || OutOfGrid())
                    {
                        transform.position = tempPosition;
                    }
                }
            }
        }
    }

    //atomatically destroy a quad if not finished within 10 seconds
    IEnumerator DestroyIfUnfinished()
    {
        yield return new WaitForSeconds(10);
        if (state != 2)
        {
            Destroy(gameObject);
        }
    }

    public bool ContainPoint(Vector3 point)
    {
        if (point.x >= transform.position.x && point.x < transform.position.x + width)
        {
            if (point.y >= transform.position.y && point.y < transform.position.y + height)
            {
                return true;
            }
        }

        return false;
    }

    public bool IntersectQuad(Quad quad)
    {        
        Rect rectA = new Rect(transform.position.x, transform.position.y, width, height);
        Rect rectB = new Rect(quad.transform.position.x, quad.transform.position.y, quad.width, quad.height);

        return rectA.Overlaps(rectB);
    }

    
    private bool IntersectingOtherQuads()
    {
        foreach (GameObject quad in GameObject.FindGameObjectsWithTag("quad"))
        {
            //ignore self collision
            if (quad != this.gameObject)
            {
                if (IntersectQuad(quad.GetComponent<Quad>()))
                {
                    return true;
                }
            }
        }
        return false;
    }


    private bool OutOfGrid()
    {
        if (this.transform.position.x + width > FindObjectOfType<GridScript>().width) { return true; }
        if (this.transform.position.y + height > FindObjectOfType<GridScript>().height) { return true; }
        return false;
    }


    private void ChangeWidth(float value)
    {
        width = value;
    }

    private void ChangeHeight(float value)
    {
        height = value;
    }

    public bool IsComplete()
    {
        return state == 2;
    }

    public void SetClicked(bool isClicked, Vector3 whereIsClicked)
    {
        clicked = isClicked;
        pointClickedDelta = whereIsClicked-transform.position;
    }

    public void SetClicked(bool isClicked)
    {
        SetClicked(isClicked, new Vector3(0, 0));
    }

    public bool isClicked()
    {
        return clicked;
    }

    public void Rotate90Degrees()
    {
        int x = Mathf.FloorToInt(width);
        int y = Mathf.FloorToInt(height);
        ChangeWidth(y);
        ChangeHeight(x);
    }

    public void OnDestroy()
    {
        GameObject.FindObjectOfType<GridScript>().available_colors.Add(this.getColor());
    }
}
