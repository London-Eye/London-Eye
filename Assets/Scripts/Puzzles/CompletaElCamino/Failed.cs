using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Failed : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 initialPos;
    private Vector3 prevPos;
    private Vector3 actualPos;
    int cont = 0;
    void Start()
    {
        initialPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        prevPos = initialPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && cont == 60)
        {
            actualPos = gameObject.transform.position;

            if (gameObject.transform.position.y < -15 || (actualPos.x == prevPos.x && actualPos.y == prevPos.y))
            {
                gameObject.transform.position = new Vector3(initialPos.x, initialPos.y, initialPos.z);
                gameObject.SetActive(false);
            }
            prevPos = actualPos;
            cont = 0;
        }
        cont++;
    }
}
