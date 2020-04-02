using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] public PauseController ps;
    [SerializeField] public Color[] c ;
    [SerializeField] private GameObject EndgameMenu;
    public bool first;
    public Color ballTomove;
    public initially_full_tube sourceTube;
    public bool endgame;
    private initially_full_tube[] tubes;
    // Start is called before the first frame update
    void Start()
    {
        first = true;
        endgame = false;
        EndgameMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        endgame = isEndgame();
        if (endgame) {
            EndgameMenu.SetActive(true);
        }
    }

    private bool isEndgame() {
        tubes = GameObject.FindObjectsOfType<initially_full_tube>();
        for (int i = 0; i < tubes.Length; i++) {
            if (!tubes[i].isComplete()) {
                return false;
            }
        }
        return true;

    }

    public void setTubes() {
        tubes = GameObject.FindObjectsOfType<initially_full_tube>();
        int empty_tubes = tubes.Length - (c.Length / 4);
        for (int i = 0; i < empty_tubes; i++) {
            Debug.Log(i);
            tubes[i].initial_status = "empty";
        }
        int k = 0;
        for (int j = empty_tubes; j < tubes.Length; j++) {
            Debug.Log("In");
            tubes[j].initial_status = "full";
            tubes[j].transform.GetChild(3).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(2).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(0).GetComponent<SpriteRenderer>().color = c[k++];
            
        }
    }
}
