using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle : MonoBehaviour
{/*
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    */

    public Color getColor() {
        return this.gameObject.GetComponent<SpriteRenderer>().color;
    }
    
}
