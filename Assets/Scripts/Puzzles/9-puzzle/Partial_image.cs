using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partial_image : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite subImage;
    private Vector3 ini,fin;
    private Partial_image source;

    public object x { get; private set; }

    public void setSubImage(Sprite s) {
        GetComponent<SpriteRenderer>().sprite = s;
    }

    public void setPosition(float x, float y) {
        transform.position = new Vector3(x, y, transform.position.z);
    }



    public Vector3 GetSector(float x, float y) {

        Vector3 point;

        if (x < -0.045)
        {
            //columna de la izquierda
            if (y < -1.54)
            {
                //fila de inferior
                point = new Vector3(-1.09f, -3.04f, transform.position.z);
            }
            else {
                if (y < 1.43)
                {
                    //fila central
                    point = new Vector3(-1.09f, -0.07f, transform.position.z);
                }
                else {
                    //fila superior
                    point = new Vector3(-1.09f, 2.9f, transform.position.z);
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
                    point = new Vector3(1, -3.04f, transform.position.z);
                }
                else
                {
                    if (y < 1.43)
                    {
                        //fila central
                        point = new Vector3(1, -0.07f, transform.position.z);
                    }
                    else
                    {
                        //fila superior
                        point = new Vector3(1, 2.9f, transform.position.z);
                    }
                }
            }
            else {
                //columna de la derecha
                if (y < -1.54)
                {
                    //fila de inferior
                    point = new Vector3(3.09f, -3.04f, transform.position.z);
                }
                else
                {
                    if (y < 1.43)
                    {
                        //fila central
                        point = new Vector3(3.09f, -0.07f, transform.position.z);
                    }
                    else
                    {
                        //fila superior
                        point = new Vector3(3.09f, 2.9f, transform.position.z);
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
    }
    private void OnMouseDrag()
    {
        Vector2 mousePos = GetMousePosition();
        this.gameObject.transform.localPosition = new Vector3(mousePos.x,mousePos.y,this.gameObject.transform.localPosition.z );

    }

    private void OnMouseUp() {
        Vector2 mouseMove = GetMousePosition();

        fin = GetSector(mouseMove.x, mouseMove.y);
        this.gameObject.transform.localPosition = new Vector3(fin.x, fin.y, fin.z);

        Partial_image[] allObjects = UnityEngine.Object.FindObjectsOfType<Partial_image>();
        foreach (Partial_image go in allObjects)
            if (go.transform.localPosition == fin && go.Equals(this) == false) {
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
