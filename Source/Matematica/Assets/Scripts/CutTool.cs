using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTool : MonoBehaviour
{
    private Camera cam;
    private GridScript gridScript;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        gridScript = GameObject.FindObjectOfType<GridScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = Input.mousePosition;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, cam.nearClipPlane));

        GameObject quad = null;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("quad"))
        {
            if (obj.GetComponent<Quad>() != null)
            {
                if (obj.GetComponent<Quad>().ContainPoint(point))
                {
                    quad = obj;
                }
            }
        }

        if (quad == null)
        {
            GetComponent<SpriteRenderer>().size = new Vector2(0.1f, 0.1f);
        }
        else
        {

            Vector2 mouseDelta = new Vector2(Mathf.Repeat(point.x, 1.0f) - 0.5f, Mathf.Repeat(point.y, 1.0f) - 0.5f);

            //rispetto al centro posso trovarmi a sinistra/destra/alto/basso... 
            if (Mathf.Abs(mouseDelta.x) < Mathf.Abs(mouseDelta.y))
            {
                GetComponent<SpriteRenderer>().size = new Vector2(quad.GetComponent<Quad>().getSize().x + 1, 0.1f);
                transform.position = new Vector3(quad.transform.position.x - 0.5f, Mathf.Round(point.y), -1);
            }
            else
            {
                GetComponent<SpriteRenderer>().size = new Vector2(0.1f, quad.GetComponent<Quad>().getSize().y + 1);
                transform.position = new Vector3(Mathf.Round(point.x), quad.transform.position.y - 0.5f, -1);
            }

            if (
                   transform.position.x == quad.transform.position.x
                || transform.position.y == quad.transform.position.y
                || transform.position.x == quad.transform.position.x + quad.GetComponent<Quad>().getSize().x
                || transform.position.y == quad.transform.position.y + quad.GetComponent<Quad>().getSize().y)
            {
                quad = null;
                GetComponent<SpriteRenderer>().size = new Vector2(0.1f, 0.1f);
            }
        }
        if (quad != null) {
            if (Input.GetMouseButtonUp(0))
            {
                Color color = gridScript.pickRandomColor();
                if (color != Color.clear)
                {
                    if (GetComponent<SpriteRenderer>().size.x > 1)
                    {

                        GameObject newQuad = Instantiate(quad.gameObject);
                        newQuad.GetComponent<Quad>().setColor(color);
                        Vector2 oldSize = quad.GetComponent<Quad>().getSize();
                        float cutPoint = Mathf.Floor(transform.position.y - quad.transform.position.y);

                        newQuad.transform.position = new Vector3(quad.transform.position.x, cutPoint + quad.transform.position.y, 0);
                        newQuad.GetComponent<Quad>().setSize(new Vector2(oldSize.x, oldSize.y - cutPoint));
                        newQuad.GetComponent<Quad>().changeState();

                        quad.GetComponent<Quad>().setSize(new Vector2(oldSize.x, cutPoint));
                    }
                    else
                    {
                        GameObject newQuad = Instantiate(quad.gameObject);
                        newQuad.GetComponent<Quad>().setColor(color);
                        Vector2 oldSize = quad.GetComponent<Quad>().getSize();
                        float cutPoint = Mathf.Floor(transform.position.x - quad.transform.position.x);

                        newQuad.transform.position = new Vector3(cutPoint + quad.transform.position.x, quad.transform.position.y, 0);
                        newQuad.GetComponent<Quad>().setSize(new Vector2(oldSize.x - cutPoint, oldSize.y));
                        newQuad.GetComponent<Quad>().changeState();

                        quad.GetComponent<Quad>().setSize(new Vector2(cutPoint, oldSize.y));
                    }
                }
            }
        }
    }
}
