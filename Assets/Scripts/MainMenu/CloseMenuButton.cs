﻿using UnityEngine;

public class CloseMenuButton : MonoBehaviour
{

    public GameObject menu;

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    //void Update()
    //{

    //}

    public void CloseMenu()
    {
        menu.SetActive(false);
    }
}
