using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initially_full_tube : MonoBehaviour
{
    [SerializeField] circle[] Balls;
    [SerializeField] string initial_status;
    private GameController controller;
    public Stack<circle> pila;
    private int max_size = 4;
    private GameObject basse;
    private GameObject middleBase;
    private GameObject middleTop;
    private GameObject top;
    private float oldY;
    // Start is called before the first frame update
    void Start()
    {
        pila = new Stack<circle>();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        basse = this.gameObject.transform.GetChild(0).gameObject;
        middleBase = this.gameObject.transform.GetChild(1).gameObject;
        middleTop = this.gameObject.transform.GetChild(2).gameObject;
        top = this.gameObject.transform.GetChild(3).gameObject;

        if (initial_status == "full")
        {
            for (int i = Balls.Length - 1; i >= 0; i--)
            {
                pila.Push(Balls[i]);
            }
            basse.GetComponent<SpriteRenderer>().color = Balls[0].getColor();
            middleBase.GetComponent<SpriteRenderer>().color = Balls[1].getColor();
            middleTop.GetComponent<SpriteRenderer>().color = Balls[2].getColor();
            top.GetComponent<SpriteRenderer>().color = Balls[3].getColor();
            oldY = top.transform.position.y;
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
        Debug.Log(controller.first);
        if (size(pila) > 0 && controller.first) //primera bola que pillas
        {
            controller.ballTomove = pila.Peek();
            controller.first = !controller.first;
            Debug.Log(controller.ballTomove.ToString());
            controller.sourceTube = this;
            oldY = top.transform.position.y;
            top.transform.position = new Vector3(top.transform.position.x, oldY + 0.65f, top.transform.position.z);
        }
        else
        {
            if (size(pila) < max_size) //con la bola ya pillada, si la bola cabe
            {
                if (controller.ballTomove != null) //  y la bola no es null
                {
                    controller.sourceTube.pila.Pop();
                    pila.Push(controller.ballTomove);
                    Debug.Log(pila.ToArray().ToString());
                    top.transform.position = new Vector3(top.transform.position.x, oldY, top.transform.position.z);
                    printStackStatus(pila);
                }
            }
            else { // la bola no cabe
                controller.sourceTube.gameObject.transform.GetChild(3).transform.position = new Vector3(controller.sourceTube.gameObject.transform.GetChild(3).transform.position.x, controller.sourceTube.oldY, controller.sourceTube.gameObject.transform.GetChild(3).transform.position.z);
                printStackStatus(pila);
            }
            controller.ballTomove = null;
            controller.first = !controller.first;
            controller.sourceTube = null;
        }
        
    }

    public int size(Stack<circle> s) {
        return s.ToArray().Length;
    }

    public void printStackStatus(Stack<circle> s) {

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
}
