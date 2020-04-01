using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initially_empty_tube : MonoBehaviour
{/*
    private GameController controller;
    public Stack<circle> pila;
    private int max_size = 4;
    private GameObject basse;
    private GameObject middleBase;
    private GameObject middleTop;
    private GameObject top;
    private float oldY;
    void Start()
    {
        pila = new Stack<circle>();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        GameObject basse = this.gameObject.transform.GetChild(0).gameObject;
        GameObject middleBase = this.gameObject.transform.GetChild(1).gameObject;
        GameObject middleTop = this.gameObject.transform.GetChild(2).gameObject;
        GameObject top = this.gameObject.transform.GetChild(3).gameObject;
        
        basse.SetActive(false);
        middleBase.SetActive(false);
        middleTop.SetActive(false);
        top.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        Debug.Log(controller.first);
        if (size(pila) > 0 && controller.first) //primera bola que pillas
        {
            controller.ballTomove = pila.Pop();
            controller.first = !controller.first;
            Debug.Log(controller.ballTomove.ToString());
            controller.sourceStack = this.pila;
            oldY = top.transform.position.y;
            top.transform.position = new Vector3(top.transform.position.x, oldY + 0.65f, top.transform.position.z);
        }
        else
        {
            if (size(pila) < max_size) //con la bola ya pillada, si la bola cabe
            {
                if (controller.ballTomove != null) //  y la bola no es null
                {
                    pila.Push(controller.ballTomove);
                    controller.first = !controller.first;
                    controller.ballTomove = null;
                    Debug.Log(pila.ToArray().ToString());
                    top.transform.position = new Vector3(top.transform.position.x, oldY, top.transform.position.z);
                    printStackStatus(pila);
                }
            }
            else
            { // la bola no cabe
                controller.sourceStack.Push(controller.ballTomove);
                controller.ballTomove = null;
                controller.first = !controller.first;
                top.transform.position = new Vector3(top.transform.position.x, oldY, top.transform.position.z);
                printStackStatus(pila);
            }
        }

    }

    public int size(Stack<circle> s)
    {
        return s.ToArray().Length;
    }

    public void printStackStatus(Stack<circle> s)
    {

        switch (size(s))
        {
            case 0:
                basse.SetActive(false);
                middleBase.SetActive(false);
                middleTop.SetActive(false);
                top.SetActive(false);
                break;

            case 1:
                basse.SetActive(true);
                basse.GetComponent<SpriteRenderer>().color = s.ToArray()[0].getColor();
                break;

            case 2:
                basse.SetActive(true);
                basse.GetComponent<SpriteRenderer>().color = s.ToArray()[0].getColor();
                middleBase.SetActive(true);
                middleBase.GetComponent<SpriteRenderer>().color = s.ToArray()[1].getColor();
                break;

            case 3:
                basse.SetActive(true);
                basse.GetComponent<SpriteRenderer>().color = s.ToArray()[0].getColor();
                middleBase.SetActive(true);
                middleBase.GetComponent<SpriteRenderer>().color = s.ToArray()[1].getColor();
                middleTop.SetActive(true);
                middleTop.GetComponent<SpriteRenderer>().color = s.ToArray()[2].getColor();
                break;

            case 4:
                basse.SetActive(true);
                basse.GetComponent<SpriteRenderer>().color = s.ToArray()[0].getColor();
                middleBase.SetActive(true);
                middleBase.GetComponent<SpriteRenderer>().color = s.ToArray()[1].getColor();
                middleTop.SetActive(true);
                middleTop.GetComponent<SpriteRenderer>().color = s.ToArray()[2].getColor();
                top.SetActive(true);
                top.GetComponent<SpriteRenderer>().color = s.ToArray()[3].getColor();
                break;

        }
    }
*/
}
