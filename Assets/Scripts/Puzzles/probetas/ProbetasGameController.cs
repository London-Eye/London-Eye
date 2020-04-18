using System.Collections.Generic;
using UnityEngine;

public class ProbetasGameController : MonoBehaviour
{
    [SerializeField] public PauseController ps;
    [SerializeField] public Color[] c1;
    [SerializeField] public Color[] c2;
    [SerializeField] public Color[] c3;
    [SerializeField] public Color[] c4;
    [SerializeField] public Color[] c5;

    [SerializeField] private GameObject EndgameMenu;
    [SerializeField] private InitiallyFullTube tube;
    [SerializeField] private GameObject container;
    public bool first;
    public Color ballTomove;
    public InitiallyFullTube sourceTube;
    private InitiallyFullTube[] tubes = new InitiallyFullTube[8];
    public int selector;
    public Color[] c = new Color[24];
    private static readonly Vector2[] position = new Vector2[8] { new Vector2(-3.5f, -2f), new Vector2(-2f, -2f), new Vector2(-0.5f, -2f), new Vector2(1f, -2f), new Vector2(-3.5f, 1.25f), new Vector2(-2f, 1.25f), new Vector2(-0.5f, 1.25f), new Vector2(1f, 1.25f) };
    private static readonly Vector2[] positionLR = new Vector2[8] { new Vector2(-2f, -2f), new Vector2(-1f, -2f), new Vector2(0f, -2f), new Vector2(1f, -2f), new Vector2(-2f, 1.25f), new Vector2(-1f, 1.25f), new Vector2(0f, 1.25f), new Vector2(1f, 1.25f) };
    public bool GameRunning { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        selector = CharacterCreation.Instance.PuzzleCombinationPools[this.GetType()].Select();
        switch (selector)
        {
            case 0:
                c = c1;
                break;
            case 1:
                c = c2;
                break;
            case 2:
                c = c3;
                break;
            case 3:
                c = c4;
                break;
            case 4:
                c = c5;
                break;
        }
        first = true;
        GameRunning = true;
        EndgameMenu.SetActive(false);

        for (int i = 0; i < position.Length; i++)
        {
            InitiallyFullTube t = Instantiate(tube) as InitiallyFullTube;
            t.transform.SetParent(container.GetComponent<Transform>());
            t.transform.position = new Vector3(position[i][0], position[i][1], container.transform.position.z);
            t.transform.localScale = new Vector3(container.transform.localScale.x + 77.18328f, container.transform.localScale.y + 77.18328f, container.transform.localScale.z + 77.18328f);
            Debug.Log("before if");
            if (Screen.currentResolution.width != 1920 && Screen.currentResolution.height != 1080)
            {
                Debug.Log("in Screen");
                t.transform.localScale = new Vector3(t.transform.localScale.x / 2, t.transform.localScale.x / 2, 1);
                t.transform.position = new Vector3(positionLR[i][0], positionLR[i][1], container.transform.position.z);
            }
        }
        setTubes();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void isEndgame()
    {
        tubes = FindObjectsOfType<InitiallyFullTube>();
        bool completed = true;
        for (int i = 0; i < tubes.Length; i++)
        {
            if (!tubes[i].IsComplete())
            {
                completed = false;
            }
        }
        EndgameMenu.SetActive(completed);

    }

    public void setTubes()
    {
        tubes = FindObjectsOfType<InitiallyFullTube>();
        int empty_tubes = tubes.Length - c.Length / 4;
        Debug.Log(c.Length);
        Debug.Log(tubes.Length);
        for (int i = 0; i < empty_tubes; i++)
        {
            tubes[i].initial_status = "empty";
            tubes[i].transform.GetChild(0).gameObject.SetActive(false);
            tubes[i].transform.GetChild(1).gameObject.SetActive(false);
            tubes[i].transform.GetChild(2).gameObject.SetActive(false);
            tubes[i].transform.GetChild(3).gameObject.SetActive(false);
            tubes[i].pila = new Stack<Color>();
        }
        int k = 0;
        for (int j = empty_tubes; j < tubes.Length; j++)
        {
            Debug.Log(tubes[j]);
            tubes[j].initial_status = "full";
            tubes[j].transform.GetChild(3).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(2).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(1).GetComponent<SpriteRenderer>().color = c[k++];
            tubes[j].transform.GetChild(0).GetComponent<SpriteRenderer>().color = c[k++];
            fillStack(tubes[j]);

        }
    }
    private void fillStack(InitiallyFullTube t)
    {
        t.pila = new Stack<Color>();
        t.pila.Push(t.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
        t.pila.Push(t.transform.GetChild(1).GetComponent<SpriteRenderer>().color);
        t.pila.Push(t.transform.GetChild(2).GetComponent<SpriteRenderer>().color);
        t.pila.Push(t.transform.GetChild(3).GetComponent<SpriteRenderer>().color);
    }
}