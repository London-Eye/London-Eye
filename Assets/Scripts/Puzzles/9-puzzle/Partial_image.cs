using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partial_image : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Sprite subImage;
    private Vector2 ini,fin;
    private Partial_image source;
    public float[] posX = new float[16] { -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f };
    private float[] posY = new float[16] { 3.4f, 3.4f, 3.4f, 3.4f, 1.14f, 1.14f, 1.14f, 1.14f, -1.12f, -1.12f, -1.12f, -1.12f, -3.38f, -3.38f, -3.38f, -3.38f };
    public object x { get; private set; }

    public void setSubImage(Sprite s) {
        GetComponent<SpriteRenderer>().sprite = s;
    }

    public void setPosition(float x, float y) {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public string getSpriteName(Partial_image pi) {
        return pi.GetComponent<SpriteRenderer>().sprite.name;
    }


    public Vector2 GetSector(float x, float y) {

        Vector2 point = new Vector2();
        float min = 100000.0f;
        float diff;

        for (int i = 0; i < posY.Length; i++) {
                diff = (float)Math.Sqrt((Math.Pow((posX[i] - x), 2.0) + Math.Pow((posY[i] - y), 2.0)));
                if (diff < min) {
                    min = diff;
                    point.x = posX[i];
                    point.y = posY[i];
                }
        }
        return point;
    }

    private void OnMouseDown() {
        source = this;
        Vector2 mouseMove = GetMousePosition();
        ini = GetSector(mouseMove.x, mouseMove.y);
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1);
        Debug.Log(ini);
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
