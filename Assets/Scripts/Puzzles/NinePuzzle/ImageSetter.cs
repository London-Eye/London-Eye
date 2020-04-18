using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSetter : PuzzleSetter
{
    // Start is called before the first frame update
    [SerializeField] private PartialImage partial_im;
    [SerializeField] private Sprite[] letterBroken;
    [SerializeField] private Sprite[] letterBurned;
    [SerializeField] private Sprite[] letterScrached;
    [SerializeField] private Sprite[] letterWasted;
    [SerializeField] private GameObject Ibro;
    [SerializeField] private GameObject IScr;
    [SerializeField] private GameObject IBur;
    [SerializeField] private GameObject IWas;
    private static readonly float[] posX = new float[16] { -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f };
    private static readonly float[] posY = new float[16] { 3.4f, 3.4f, 3.4f, 3.4f, 1.14f, 1.14f, 1.14f, 1.14f, -1.12f, -1.12f, -1.12f, -1.12f, -3.38f, -3.38f, -3.38f, -3.38f };
    public Dictionary<string, Vector3> correct = new Dictionary<string, Vector3>();

    [SerializeField] private GameObject EndgameMenu;

    public bool GameRunning { get; private set; }

    private int selector;

    protected override void SetPuzzle(int selector)
    {
        this.selector = selector;

        Sprite[] images = letterBroken;
        switch (selector)
        {
            case 0:
                images = letterBroken;
                break;
            case 1:
                images = letterBurned;
                break;
            case 2:
                images = letterScrached;
                break;
            case 3:
                images = letterWasted;
                break;
        }
        EndgameMenu.SetActive(false);
        for (int i = 0; i < images.Length; i++)
        {
            Vector3 aux = new Vector3(posX[i], posY[i], 0);
            correct.Add(images[i].name, aux);
        }
        images.Shuffle();

        for (int i = 0; i < images.Length; i++)
        {
            PartialImage subImage = Instantiate(partial_im) as PartialImage;
            subImage.SetSubImage(images[i]);
            subImage.SetPosition(posX[i], posY[i]);
            subImage.transform.SetParent(transform);
        }

        GameRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameRunning) CheckSolution();
    }

    private void CheckSolution()
    {
        bool completed = true;
        PartialImage[] allObjects = FindObjectsOfType<PartialImage>();
        foreach (PartialImage go in allObjects)
        {
            string name = go.GetComponent<SpriteRenderer>().sprite.name;

            Vector3 pos = go.transform.localPosition;
            if (correct[name] != pos)
            {
                completed = false;
            }
        }
        if (completed) { StartCoroutine(PuzzleCompleted()); }
    }

    private IEnumerator PuzzleCompleted()
    {
        GameRunning = false;

        yield return new WaitForSeconds(0.75f);

        EndgameMenu.SetActive(true);
        switch (selector)
        {
            case 1:
                Ibro.SetActive(true);
                break;
            case 2:
                IBur.SetActive(true);
                break;
            case 3:
                IScr.SetActive(true);
                break;
            case 4:
                IWas.SetActive(true);
                break;
        }

    }
}