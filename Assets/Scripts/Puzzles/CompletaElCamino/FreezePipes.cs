using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePipes : MonoBehaviour
{
    public bool checking;
    // Start is called before the first frame update
    void Start()
    {
        checking = false;
    }

    // Update is called once per frame
    public void pipeFreezer()
    {
        checking = !checking;
        if (checking)
        {
            PipeRotator[] pipes = GameObject.FindObjectsOfType<PipeRotator>();
            for (int i = 0; i < pipes.Length; i++)
            {
                pipes[i].GetComponent<PipeRotator>().enabled = false;
            }
        }
        else {
            PipeRotator[] pipes = GameObject.FindObjectsOfType<PipeRotator>();
            for (int i = 0; i < pipes.Length; i++)
            {
                pipes[i].GetComponent<PipeRotator>().enabled = true;
            }
        }
    }
}
