using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initially_full_tube : MonoBehaviour
{
    [SerializeField] string initial_status;
    private GameController controller;
    public Stack<Color> pila;
    private int max_size = 4;
    private GameObject basse;
    private GameObject middleBase;
    private GameObject middleTop;
    private GameObject top;
    private float oldY;
    // Start is called before the first frame update
    void Start()
    {
        pila = new Stack<Color>();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        basse = this.gameObject.transform.GetChild(0).gameObject;
        middleBase = this.gameObject.transform.GetChild(1).gameObject;
        middleTop = this.gameObject.transform.GetChild(2).gameObject;
        top = this.gameObject.transform.GetChild(3).gameObject;

        if (initial_status == "full")
        {
            pila.Push(basse.GetComponent<SpriteRenderer>().color);
            pila.Push(middleBase.GetComponent<SpriteRenderer>().color);
            pila.Push(middleTop.GetComponent<SpriteRenderer>().color);
            pila.Push(top.GetComponent<SpriteRenderer>().color);
        }
        else {
            basse.SetActive(false);
            middleBase.SetActive(false);
            middleTop.SetActive(false);
            top.SetActive(false);
        }
}

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown() {

        if (!controller.ps.IsPaused && !controller.endgame) {

            if (controller.first) //primera bola que pillas
            {
                if (size(this.pila) > 0) //si has clicado una probeta con bolas
                {
                    Debug.Log("First");
                    controller.ballTomove = this.pila.ToArray()[0];
                    controller.first = !controller.first;
                    controller.sourceTube = this;
                    doAnimation(this);
                }
            }
            else
            {
                //TODO: 
                // - que la probeta esté vacía o que tenga en la pila a alguien de su color.

                if (size(this.pila) < max_size) //con la bola ya pillada, si la bola cabe
                {
                    Debug.Log("Cabe");
                    if (size(this.pila) == 0) {
                        Debug.Log("Case vacia");
                        undoAnimation(controller.sourceTube);
                        this.pila.Push(controller.sourceTube.pila.Pop());
                    }
                    else {
                        Debug.Log("case con algo");
                        if (this.pila.ToArray()[0].Equals(controller.ballTomove))
                        {
                            Debug.Log("Es algo del mismo color");
                            undoAnimation(controller.sourceTube);
                            this.pila.Push(controller.sourceTube.pila.Pop());
                            
                            printStackStatus(this.pila, this);
                        }
                        else {
                            Debug.Log("no es del mismo color");
                            undoAnimation(controller.sourceTube);
                            printStackStatus(controller.sourceTube.pila, controller.sourceTube);
                        }
                    }/*
                    undoAnimation(controller.sourceTube);
                    printStackStatus(this.pila, this);*/

                    printStackStatus(this.pila, this);
                    printStackStatus(controller.sourceTube.pila, controller.sourceTube);
                }
                else
                { // la bola no cabe
                    Debug.Log("No cabe");
                     undoAnimation(controller.sourceTube);
                     printStackStatus(this.pila, this);
                }
                controller.ballTomove = Color.white;
                controller.first = !controller.first;
                controller.sourceTube = null;
            }
        }

    }

    public int size(Stack<Color> s) {
        return s.ToArray().Length;
    }

    public void printStackStatus(Stack<Color> s, initially_full_tube tb) {

        switch (size(s))
        {
            case 0:
                tb.basse.SetActive(false);
                tb.middleBase.SetActive(false);
                tb.middleTop.SetActive(false);
                tb.top.SetActive(false);
                break;

            case 1:
                tb.basse.SetActive(true);
                tb.basse.GetComponent<SpriteRenderer>().color = s.ToArray()[0];
                tb.middleBase.SetActive(false);
                tb.middleTop.SetActive(false);
                tb.top.SetActive(false);
                //Debug.Log("case 1");
                break;

            case 2:
                tb.basse.SetActive(true);
                tb.basse.GetComponent<SpriteRenderer>().color = s.ToArray()[1];
                tb.middleBase.SetActive(true);
                tb.middleBase.GetComponent<SpriteRenderer>().color = s.ToArray()[0];
                tb.middleTop.SetActive(false);
                tb.top.SetActive(false);
               // Debug.Log("case 2");
                break;

            case 3:
                tb.basse.SetActive(true);
                tb.basse.GetComponent<SpriteRenderer>().color = s.ToArray()[2];
                tb.middleBase.SetActive(true);
                tb.middleBase.GetComponent<SpriteRenderer>().color = s.ToArray()[1];
                tb.middleTop.SetActive(true);
                tb.middleTop.GetComponent<SpriteRenderer>().color = s.ToArray()[0];
                tb.top.SetActive(false);
                //Debug.Log("case 3");
                break;

            case 4:
                tb.basse.SetActive(true);
                tb.basse.GetComponent<SpriteRenderer>().color = s.ToArray()[3];
                tb.middleBase.SetActive(true);
                tb.middleBase.GetComponent<SpriteRenderer>().color = s.ToArray()[2];
                tb.middleTop.SetActive(true);
                tb.middleTop.GetComponent<SpriteRenderer>().color = s.ToArray()[1];
                tb.top.SetActive(true);
                tb.top.GetComponent<SpriteRenderer>().color = s.ToArray()[0];
                //Debug.Log("case 4");
                break;
            default:
                break;
        }
    }
    public bool isComplete() {
        if (size(this.pila) == 0)
        {
            return true;
        }
        else {
            if (size(this.pila) == 4)
            {
                for (int i = 0; i < size(this.pila) - 1; i++)
                {
                    if (!this.pila.ToArray()[i].Equals(this.pila.ToArray()[i + 1]))
                    {
                        return false;
                    }
                }
            }
            else {
                return false;
            }
        }
        return true;
    }
    public void doAnimation(initially_full_tube tb) {
        switch (size(tb.pila)) {
            case 1:
                oldY = tb.basse.transform.position.y;
                tb.basse.transform.position = new Vector3(tb.basse.transform.position.x, tb.oldY + 0.65f, tb.basse.transform.position.z);
                break;
            case 2:
                oldY = tb.middleBase.transform.position.y;
                tb.middleBase.transform.position = new Vector3(tb.middleBase.transform.position.x, tb.oldY + 0.65f, tb.middleBase.transform.position.z);
                break;
            case 3:
                oldY = tb.middleTop.transform.position.y;
                tb.middleTop.transform.position = new Vector3(tb.middleTop.transform.position.x, tb.oldY + 0.65f, tb.middleTop.transform.position.z);
                break;
            case 4:
                oldY = tb.top.transform.position.y;
                tb.top.transform.position = new Vector3(tb.top.transform.position.x, tb.oldY + 0.65f, tb.top.transform.position.z);
                break;
        }
    }

    public void undoAnimation(initially_full_tube tb) {
        
        switch (size(tb.pila))
        {
            case 1:
                tb.basse.transform.position = new Vector3(tb.basse.transform.position.x, tb.oldY, tb.basse.transform.position.z);
                break;
            case 2:
                tb.middleBase.transform.position = new Vector3(tb.middleBase.transform.position.x, tb.oldY, tb.middleBase.transform.position.z);
                break;
            case 3:
                tb.middleTop.transform.position = new Vector3(tb.middleTop.transform.position.x, tb.oldY, tb.middleTop.transform.position.z);
                break;
            case 4:
                tb.top.transform.position = new Vector3(tb.top.transform.position.x, tb.oldY, tb.top.transform.position.z);
                break;
        }
        
    }
}
