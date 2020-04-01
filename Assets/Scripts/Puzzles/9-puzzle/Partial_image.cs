using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partial_image : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite subImage;
    private Vector2 ini,fin;
    private Partial_image source;

    public object x { get; private set; }

    public void setSubImage(Sprite s) {
        GetComponent<SpriteRenderer>().sprite = s;
    }

    public void setPosition(float x, float y) {
        transform.position = new Vector3(x, y, transform.position.z);
    }



    public Vector2 GetSector(float x, float y) {

        Vector2 point;

        if (x < -0.045)
        {
            //columna de la izquierda
            if (y < -1.54)
            {
                //fila de inferior
                point = new Vector2(-1.09f, -3.04f);
            }
            else {
                if (y < 1.43)
                {
                    //fila central
                    point = new Vector2(-1.09f, -0.07f);
                }
                else {
                    //fila superior
                    point = new Vector2(-1.09f, 2.9f);
                }
            }
        }
        else {
            if (x < 2.045)
            {
                //columna central
                if (y < -1.54)
                {
                    //fila de inferior
                    point = new Vector2(1, -3.04f);
                }
                else
                {
                    if (y < 1.43)
                    {
                        //fila central
                        point = new Vector2(1, -0.07f);
                    }
                    else
                    {
                        //fila superior
                        point = new Vector2(1, 2.9f);
                    }
                }
            }
            else {
                //columna de la derecha
                if (y < -1.54)
                {
                    //fila de inferior
                    point = new Vector2(3.09f, -3.04f);
                }
                else
                {
                    if (y < 1.43)
                    {
                        //fila central
                        point = new Vector2(3.09f, -0.07f);
                    }
                    else
                    {
                        //fila superior
                        point = new Vector2(3.09f, 2.9f);
                    }
                }
            }
        }
        return point;
    }

    private void OnMouseDown() {
        source = this;
        Vector2 mouseMove = GetMousePosition();
        ini = GetSector(mouseMove.x,mouseMove.y);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1);
    }
    private void OnMouseDrag()
    {
        Vector2 mousePos = GetMousePosition();
        transform.localPosition = new Vector3(mousePos.x, mousePos.y, transform.localPosition.z);

    }

    private void OnMouseUp() {
        Vector2 mouseMove = GetMousePosition();

        fin = GetSector(mouseMove.x, mouseMove.y);
        transform.localPosition = new Vector3(fin.x, fin.y, transform.localPosition.z + 1);

        Partial_image[] allObjects = FindObjectsOfType<Partial_image>();
        foreach (Partial_image go in allObjects)
            if (((Vector2) go.transform.localPosition) == fin && this != go) {
                go.setPosition(ini.x, ini.y);
                break;
            } 
    }

    private bool isBetween(float a, float b, float c)
    {
        return a >= b && a <= c;
    }
    private static Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}
