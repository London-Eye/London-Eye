using Assets.Scripts.Common;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SceneController : MonoBehaviour
{
    public int _maxMV = 0;

    public int gridRows = 1;
    public int gridCols = 1;
    public float header = 2f;
    public float margin = 1f;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    [SerializeField] private TextMesh movesLabel;
    [SerializeField] private PauseController pauseMenu;
    [SerializeField] private GameObject EndgameMenu;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private TextMeshProUGUI finalMessage;

    private int MaxScore => gridCols * gridRows / 2;

    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    private int _score = 0;
    private int _movimientos = 0;

    private bool gameEnded;

    void Start()
    {
        EndgameMenu.SetActive(false);
        pauseMenu.IsPaused = false;
        gameEnded = false;

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
                float posY = (j + 0.75f) * cardHeight - totalheight / 2 + margin;
                card.transform.position = new Vector3(posX, posY, originalCard.transform.position.z);
            }
        }
    }

    void LateUpdate()
    {
        if (pauseMenu.IsPaused) return;

        if (!gameEnded && (_movimientos >= _maxMV || _score == MaxScore))
        {
            StartCoroutine(EndGame());
        }
    }

    public bool CanReveal => _secondRevealed == null && _movimientos < _maxMV && !pauseMenu.IsPaused;

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

        movesLabel.text = "Mov. restantes: " + (_maxMV - _movimientos);
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
            scoreLabel.text = "Parejas: " + _score;
        }
        else
        {
            yield return new WaitForSeconds(.25f);
            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }
        _firstRevealed = _secondRevealed = null;
    }

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
            if (_score >= 2 * MaxScore / 3) currentScoreRank = GoodRank;
            else if (_score <= MaxScore / 3) currentScoreRank = BadRank;
            else currentScoreRank = NormalRank;
        }
        return currentScoreRank.Value;
    }

    private IEnumerator EndGame()
    {
        gameEnded = true;

        yield return new WaitForSeconds(1.0f);

        ScoreRank scoreRank = GetScoreRank(true);

        finalMessage.text = scoreRank.Message;

        finalScore.color = scoreRank.Color;
        finalScore.text = _score + "/" + MaxScore;

        pauseMenu.IsPaused = false;
        pauseMenu.enabled = false;

        EndgameMenu.SetActive(true);
    }

    public void StartGame()
    {
        EndgameMenu.SetActive(false);

        CharacterCreation.Instance.NumberOfSuspects = GetScoreRank().NumberOfSuspects;

        FindObjectOfType<DialogueUI>().onDialogueEnd.AddListener(() =>
        {
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(0); // Load Main Menu
            loadSceneOperation.completed += op => FindObjectOfType<CharacterCreation>().CreateSuspects();
        });

        Utilities.StartPostGameDialogue();
    }
}
