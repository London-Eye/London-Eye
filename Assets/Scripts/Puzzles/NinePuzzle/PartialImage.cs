﻿using System;
using UnityEngine;

public class PartialImage : MonoBehaviour
{
    private Vector2 ini, fin;
    private PartialImage source;
    private ImageSetter controller;
    public readonly float[] posX = new float[16] { -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f };
    private readonly float[] posY = new float[16] { 3.4f, 3.4f, 3.4f, 3.4f, 1.14f, 1.14f, 1.14f, 1.14f, -1.12f, -1.12f, -1.12f, -1.12f, -3.38f, -3.38f, -3.38f, -3.38f };
    public object X { get; private set; }


    public void SetSubImage(Sprite s)
    {
        GetComponent<SpriteRenderer>().sprite = s;
    }

    public void SetPosition(float x, float y)
    {
        controller = GameObject.Find("image_container").GetComponent<ImageSetter>();
        transform.position = new Vector3(x, y, transform.position.z);
    }

    public string GetSpriteName(PartialImage pi)
    {
        return pi.GetComponent<SpriteRenderer>().sprite.name;
    }


    public Vector2 GetSector(float x, float y)
    {

        Vector2 point = new Vector2();
        float min = 100000.0f;
        float diff;

        for (int i = 0; i < posY.Length; i++)
        {
            diff = (float)Math.Sqrt(Math.Pow(posX[i] - x, 2.0) + Math.Pow(posY[i] - y, 2.0));
            if (diff < min)
            {
                min = diff;
                point.x = posX[i];
                point.y = posY[i];
            }
        }
        return point;
    }

    private void OnMouseDown()
    {
        Debug.Log(!controller.pause.activeSelf);
        if (!controller.Completed && !controller.pause.activeSelf)
        {
            Debug.Log("In");
            source = this;
            Vector2 mouseMove = GetMousePosition();
            ini = GetSector(mouseMove.x, mouseMove.y);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1);
        }
    }
    private void OnMouseDrag()
    {
        if (!controller.pause.activeSelf)
        {
            Vector2 mousePos = GetMousePosition();
            transform.localPosition = new Vector3(mousePos.x, mousePos.y, transform.localPosition.z);
        }
    }

    private void OnMouseUp()
    {
        if (!controller.pause.activeSelf)
        {
            Vector2 mouseMove = GetMousePosition();

            fin = GetSector(mouseMove.x, mouseMove.y);

            PartialImage[] allObjects = FindObjectsOfType<PartialImage>();
            foreach (PartialImage go in allObjects)
                if ((Vector2)go.transform.localPosition == fin)
                {
                    go.SetPosition(ini.x, ini.y);
                    break;
                }

            transform.localPosition = new Vector3(fin.x, fin.y, transform.localPosition.z + 1);
        }
    }

    private bool IsBetween(float a, float b, float c)
    {
        return a >= b && a <= c;
    }
    private static Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}