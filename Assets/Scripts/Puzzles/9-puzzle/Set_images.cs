using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_images : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Partial_image partial_im;
    [SerializeField] private Sprite[] letterBroken;
    [SerializeField] private Sprite[] letterBurned;
    [SerializeField] private Sprite[] letterScrached;
    private float[] posX = new float[9] { -1.09f, 1, 3.09f, -1.09f, 1, 3.09f, -1.09f, 1, 3.09f };
    private float[] posY = new float[9] { 2.9f, 2.9f, 2.9f, -0.07f,  -0.07f, -0.07f, -3.04f, -3.04f, -3.04f };
    public Dictionary<string, Vector3> correct = new Dictionary<string, Vector3>();
    public int selector;

    [SerializeField] private GameObject EndgameMenu;

    public bool GameRunning { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Sprite[] images;
        selector = Random.Range(0, 9);
        if (selector > 5)
        {
            images = letterBroken;
        }
        else {
            if (selector > 2)
            {
                images = letterBurned;
            }
            else {
                images = letterScrached;
            }
        }
        EndgameMenu.SetActive(false);
        for (int i = 0; i < images.Length; i++)
        {
            Vector3 aux = new Vector3(posX[i], posY[i], 0);
            correct.Add(images[i].name,aux);
        }
        images.Shuffle();
        
        for (int i = 0; i < images.Length; i++)
        {
            Partial_image subImage = Instantiate(partial_im) as Partial_image;
            subImage.setSubImage(images[i]);
            subImage.setPosition(posX[i], posY[i]);
            subImage.transform.SetParent(transform);
        }

        GameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameRunning) check_solution();
    }

    private void check_solution() {
        bool completed = true;
        Partial_image[] allObjects = UnityEngine.Object.FindObjectsOfType<Partial_image>();
        foreach (Partial_image go in allObjects)
        {
            string name = go.GetComponent<SpriteRenderer>().sprite.name;

            Vector3 pos = go.transform.localPosition;
            if (correct[name] != pos)
            {
                completed = false;
            }
        }
        if (completed) { StartCoroutine(puzzle_completed()); }
    }

    private IEnumerator puzzle_completed() {
        GameRunning = false;

        yield return new WaitForSeconds(0.75f);

        EndgameMenu.SetActive(true);
    }
}
