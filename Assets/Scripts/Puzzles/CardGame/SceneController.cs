using Assets.Scripts.Common;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Yarn.Unity;
public class SceneController : MonoBehaviour
{
    public int MaxMoves = 0;

    public int gridRows = 1;
    public int gridCols = 1;
    public float header = 2f;
    public float margin = 1f;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI movesLabel;
    [SerializeField] private GameObject layout;

    public string ScoreColorName;

    public UnityEvent OnStartCardGame;

    public bool GameRunning { get; set; }

    private int MaxScore => gridCols * gridRows / 2;

    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    private int _score = 0;
    private int _movimientos = 0;

    private void Start()
    {
        PutCardsOnTable();
        layout.SetActive(false);
    }

    [YarnCommand("StartCardGame")]
    public void StartCardGame()
    {
        GameRunning = true;

        scoreLabel.gameObject.SetActive(true);
        scoreLabel.color = Color.red;

        movesLabel.gameObject.SetActive(true);
        StartCoroutine(CheckMoves());

        layout.SetActive(true);

        OnStartCardGame.Invoke();
    }

    private void PutCardsOnTable()
    {
        int[] ids = new int[gridRows * gridCols];
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = i / 2;
        }

        ids.Shuffle();

        float totalheight = Camera.main.orthographicSize * 2;
        float totalwidth = totalheight * Camera.main.aspect;
        float cardWidth = (totalwidth - 2 * margin) / gridCols;
        float cardHeight = (totalheight - header - margin) / gridRows;
        for (int j = 0; j < gridRows; j++)
        {
            for (int i = 0; i < gridCols; i++)
            {
                MemoryCard card = Instantiate(originalCard) as MemoryCard;
                int index = j * gridCols + i;
                int id = ids[index];
                card.SetCard(id, images[id]);
                float posX = (i + 0.5f) * cardWidth - totalwidth / 2 + margin;
                float posY = (j + 0.5f) * cardHeight - totalheight / 2 + margin;
                card.transform.position = new Vector3(posX, posY, originalCard.transform.position.z);
            }
        }
    }

    void LateUpdate()
    {
        if (GameRunning && (_movimientos >= MaxMoves || _score == MaxScore))
        {
            EndGame();
        }
    }

    public bool CanReveal => _secondRevealed == null && _movimientos < MaxMoves && GameRunning;

    public void CardRevealed(MemoryCard card)
    {
        if (_firstRevealed == null)
        {
            _firstRevealed = card; _movimientos++;
        }
        else
        {
            _secondRevealed = card;
            _movimientos++;
            StartCoroutine(CheckMatch());
        }
        StartCoroutine(CheckMoves());

    }

    private IEnumerator CheckMoves()
    {
        int movRest = MaxMoves - _movimientos;
        movesLabel.text = movRest.ToString();
        if (movRest < MaxMoves / 2) {
            movesLabel.color = Color.yellow;
        }
        if (movRest < MaxMoves / 5) {
            movesLabel.color = Color.red;
        }
        yield return null;
    }
    private IEnumerator CheckMatch()
    {
        string sp1, sp2;
        sp1 = _firstRevealed.GetComponent<SpriteRenderer>().sprite.name;
        sp2 = _secondRevealed.GetComponent<SpriteRenderer>().sprite.name;
        if (sp1.Equals(sp2))
        {
            _score++;
            ScoreRank scoreRank = GetScoreRank(true);
            scoreLabel.color = scoreRank.Color;
            scoreLabel.text = _score.ToString();
        }
        else
        {
            yield return new WaitForSeconds(0.75f);
            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }
        _firstRevealed = _secondRevealed = null;
    }

    public void SkipGame() { if (GameRunning) EndGame(); }

    // ENDGAME

    struct ScoreRank
    {
        public string Name { get; }
        public string Message { get; }
        public Color Color { get; }
        public int NumberOfSuspects { get; }

        public ScoreRank(string name, string message, Color color, int numberOfSuspects)
        {
            Name = name;
            Message = message;
            Color = color;
            NumberOfSuspects = numberOfSuspects;
        }
    }

    private static readonly ScoreRank
        GoodRank = new ScoreRank("Good", "¡Enhorabuena!", Color.green, CharacterCreation.maxNumberOfSuspects - 2),
        BadRank = new ScoreRank("Bad", "Ups...", Color.red, CharacterCreation.maxNumberOfSuspects),
        NormalRank = new ScoreRank("Normal", "No está mal", Color.yellow, CharacterCreation.maxNumberOfSuspects - 1);

    private ScoreRank? currentScoreRank;

    private ScoreRank GetScoreRank(bool recalculate = false)
    {
        if (recalculate || !currentScoreRank.HasValue)
        {
            if (_score >= MaxScore / 1.2) currentScoreRank = GoodRank;
            else if (_score < MaxScore / 2) currentScoreRank = BadRank;
            else currentScoreRank = NormalRank;
        }
        return currentScoreRank.Value;
    }

    private void EndGame()
    {
        GameRunning = false;

        ScoreRank scoreRank = GetScoreRank(true);

        CharacterCreation.Instance.NumberOfSuspects = scoreRank.NumberOfSuspects;

        string scoreColorAsString = '#' + ColorUtility.ToHtmlStringRGBA(scoreRank.Color);
        FindObjectOfType<DialogueRunner>().variableStorage.SetValueNoLeading(ScoreColorName, scoreColorAsString);

        FindObjectOfType<DialogueUI>().onDialogueEnd.AddListener(() =>
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(1); // Load Main Menu
            loadSceneOperation.completed += op =>
            {
                CharacterCreation.Instance.CreateSuspects();

                DialogueRunner dialogueRunner = FindObjectOfType<DialogueRunner>();
                dialogueRunner.startNode = "MainMenu-Return";
                dialogueRunner.startAutomatically = true;
            };
        });

        FindObjectOfType<DialogueController>().StartPostGameDialogue();
    }
}
