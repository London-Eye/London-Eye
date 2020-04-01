using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] public PauseController ps;
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
    }

    // Update is called once per frame
    void Update()
    {
        endgame = isEndgame();
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
}
