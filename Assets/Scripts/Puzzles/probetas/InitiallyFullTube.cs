using System.Collections.Generic;
using UnityEngine;

public class InitiallyFullTube : MonoBehaviour
{
    [SerializeField] public string initial_status;
    private ProbetasGameController controller;
    public Stack<Color> pila;
    private const int max_size = 4;
    public GameObject basse;
    public GameObject middleBase;
    public GameObject middleTop;
    public GameObject top;
    private float oldY;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<ProbetasGameController>();
        basse = gameObject.transform.GetChild(0).gameObject;
        middleBase = gameObject.transform.GetChild(1).gameObject;
        middleTop = gameObject.transform.GetChild(2).gameObject;
        top = gameObject.transform.GetChild(3).gameObject;

    }

    private void OnMouseDown()
    {

        if (!controller.ps.IsPaused && controller.GameRunning)
        {
            Debug.Log("clicado");
            if (controller.first) //primera bola que pillas
            {
                Debug.Log("primera bola");
                if (Size(pila) > 0) //si has clicado una probeta con bolas
                {
                    Debug.Log("First");
                    controller.ballTomove = pila.ToArray()[0];
                    controller.first = !controller.first;
                    controller.sourceTube = this;
                    DoAnimation(this);
                }
            }
            else
            {
                //TODO: 
                // - que la probeta esté vacía o que tenga en la pila a alguien de su color.

                if (Size(pila) < max_size) //con la bola ya pillada, si la bola cabe
                {
                    Debug.Log("Cabe");
                    if (Size(pila) == 0)
                    {
                        Debug.Log("Case vacia");
                        UndoAnimation(controller.sourceTube);
                        pila.Push(controller.sourceTube.pila.Pop());
                    }
                    else
                    {
                        Debug.Log("case con algo");
                        if (pila.ToArray()[0].Equals(controller.ballTomove))
                        {
                            Debug.Log("Es algo del mismo color");
                            UndoAnimation(controller.sourceTube);
                            pila.Push(controller.sourceTube.pila.Pop());

                            PrintStackStatus(pila, this);
                        }
                        else
                        {
                            Debug.Log("no es del mismo color");
                            UndoAnimation(controller.sourceTube);
                            PrintStackStatus(controller.sourceTube.pila, controller.sourceTube);
                        }
                    }/*
                undoAnimation(controller.sourceTube);
                printStackStatus(this.pila, this);*/

                    PrintStackStatus(pila, this);
                    PrintStackStatus(controller.sourceTube.pila, controller.sourceTube);
                }
                else
                { // la bola no cabe
                    Debug.Log("No cabe");
                    UndoAnimation(controller.sourceTube);
                    PrintStackStatus(pila, this);
                }
                controller.ballTomove = Color.white;
                controller.first = !controller.first;
                controller.sourceTube = null;
                controller.isEndgame();
            }
        }

    }

    public int Size(Stack<Color> s)
    {
        return s.ToArray().Length;
    }

    public void PrintStackStatus(Stack<Color> s, InitiallyFullTube tb)
    {

        switch (Size(s))
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
    public bool IsComplete()
    {
        if (Size(pila) == 0)
        {
            return true;
        }
        else
        {
            if (Size(pila) == 4)
            {
                for (int i = 0; i < Size(pila) - 1; i++)
                {
                    if (!pila.ToArray()[i].Equals(pila.ToArray()[i + 1]))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    public void DoAnimation(InitiallyFullTube tb)
    {
        switch (Size(tb.pila))
        {
            case 1:
                oldY = tb.basse.transform.position.y;
                tb.basse.transform.position = new Vector3(tb.basse.transform.position.x, tb.oldY + 0.5f, tb.basse.transform.position.z);
                break;
            case 2:
                oldY = tb.middleBase.transform.position.y;
                tb.middleBase.transform.position = new Vector3(tb.middleBase.transform.position.x, tb.oldY + 0.5f, tb.middleBase.transform.position.z);
                break;
            case 3:
                oldY = tb.middleTop.transform.position.y;
                tb.middleTop.transform.position = new Vector3(tb.middleTop.transform.position.x, tb.oldY + 0.5f, tb.middleTop.transform.position.z);
                break;
            case 4:
                oldY = tb.top.transform.position.y;
                tb.top.transform.position = new Vector3(tb.top.transform.position.x, tb.oldY + 0.5f, tb.top.transform.position.z);
                break;
        }
    }

    public void UndoAnimation(InitiallyFullTube tb)
    {

        switch (Size(tb.pila))
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