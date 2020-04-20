using Assets.Scripts.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSetter : PuzzleSetter
{
    [SerializeField] private PartialImage partial_im;
    [SerializeField] private Sprite[] letterBroken;
    [SerializeField] private Sprite[] letterBurned;
    [SerializeField] private Sprite[] letterScrached;
    [SerializeField] private Sprite[] letterWasted;
    
    private static readonly float[] posX = new float[16] { -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f, -1.55f, 0.04f, 1.63f, 3.22f };
    private static readonly float[] posY = new float[16] { 3.4f, 3.4f, 3.4f, 3.4f, 1.14f, 1.14f, 1.14f, 1.14f, -1.12f, -1.12f, -1.12f, -1.12f, -3.38f, -3.38f, -3.38f, -3.38f };
    
    public Dictionary<string, Vector3> correct = new Dictionary<string, Vector3>();

    public bool Completed { get; private set; }

    public bool GameRunning { get; private set; }

    protected override void SetPuzzle(int selector)
    {
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
        Completed = true;
        PartialImage[] allObjects = FindObjectsOfType<PartialImage>();
        foreach (PartialImage go in allObjects)
        {
            string name = go.GetComponent<SpriteRenderer>().sprite.name;

            Vector3 pos = go.transform.localPosition;
            if (correct[name] != pos)
            {
                Completed = false;
            }
        }
        if (Completed) { StartCoroutine(PuzzleCompleted()); }
    }

    private IEnumerator PuzzleCompleted()
    {
        GameRunning = false;

        yield return new WaitForSeconds(1f);

        dialogueController.StartPostGameDialogue();
    }
}