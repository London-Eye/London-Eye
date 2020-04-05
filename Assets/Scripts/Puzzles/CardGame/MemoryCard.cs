using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private GameObject cardBack;
    private SceneController controller;

    public int Id { get; private set; }

    public void SetCard(int id, Sprite image)
    {
        this.Id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void Start()
    {
        controller = FindObjectOfType<SceneController>();
    }

    public void OnMouseDown()
    {
        if (cardBack.activeSelf && controller.CanReveal)
        {
            cardBack.SetActive(false);
            controller.CardRevealed(this);
        }
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }
}
