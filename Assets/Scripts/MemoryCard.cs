using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    private SceneController controller;
    private int _id;
    public int id
    {
        get { return _id; }
    }
    public void SetCard(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }
        public void Start()
    {
        controller = GameObject.Find("GameController").GetComponent<SceneController>();
    }
    public void OnMouseDown()
    {
        if (cardBack.activeSelf /*&& controller.canReveal*/)
        {
            cardBack.SetActive(false);
            controller.CardRevealed(this);
            /*src = cardBack.GetComponent<SpriteRenderer>().;*/
        }
    }
    public void Unreveal()
    {
        cardBack.SetActive(true);
    }
}
