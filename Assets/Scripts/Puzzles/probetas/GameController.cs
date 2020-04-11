using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] public PauseController ps;
    [SerializeField] public Color[] c1;
    [SerializeField] public Color[] c2 ;
    [SerializeField] public Color[] c3;
    [SerializeField] public Color[] c4;
    [SerializeField] public Color[] c5;

    [SerializeField] private GameObject EndgameMenu;
    public bool first;
    public Color ballTomove;
    public initially_full_tube sourceTube;
    private initially_full_tube[] tubes;
    public int selector;
    public Color[] c = new Color[24];

    public bool GameRunning { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        selector = Random.Range(1, 5);
        switch (selector) {
            case 1:
                c = c1;
                break;
            case 2:
                c = c2;
                break;
            case 3:
                c = c3;
                break;
            case 4:
                c = c4;
                break;
            case 5:
                c = c5;
                break;
        }
           
        first = true;
        GameRunning = true;
        EndgameMenu.SetActive(false);
        setTubes();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (GameRunning && isEndgame())
        {
            GameRunning = false;
            EndgameMenu.SetActive(true);
        }*/
    }

    public void isEndgame() {
        tubes = GameObject.FindObjectsOfType<initially_full_tube>();
        bool completed = true;
        for (int i = 0; i < tubes.Length; i++) {
            if (!tubes[i].isComplete()) {
                completed = false;
            }
        }
        EndgameMenu.SetActive(completed);

    }

    public void setTubes() {
        tubes = GameObject.FindObjectsOfType<initially_full_tube>();
        int empty_tubes = tubes.Length - (c.Length / 4);
        Debug.Log(c.Length);
        for (int i = 0; i < empty_tubes; i++) {
            tubes[i].initial_status = "empty";
            tubes[i].transform.GetChild(0).gameObject.SetActive(false);
            tubes[i].transform.GetChild(1).gameObject.SetActive(false);
            tubes[i].transform.GetChild(2).gameObject.SetActive(false);
            tubes[i].transform.GetChild(3).gameObject.SetActive(false);
        }
        int k = 0;
        for (int j = empty_tubes; j < tubes.Length; j++) {
            Debug.Log("In");
            tubes[j].initial_status = "full";
            tubes[j].transform.GetChild(3).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(2).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(0).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].pila.Push(tubes[j].transform.GetChild(0).GetComponent<SpriteRenderer>().color);
            tubes[j].pila.Push(tubes[j].transform.GetChild(1).GetComponent<SpriteRenderer>().color);
            tubes[j].pila.Push(tubes[j].transform.GetChild(2).GetComponent<SpriteRenderer>().color);
            tubes[j].pila.Push(tubes[j].transform.GetChild(3).GetComponent<SpriteRenderer>().color);


        }
    }
}
