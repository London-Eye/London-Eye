using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;
    [SerializeField] private TextMesh movesLabel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject EndgameMenu;
    [SerializeField] private TextMeshProUGUI finalScore;
    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    private int _score = 0;
    private int _movimientos = 0;
    public int _maxMV = 0;

    public int gridRows = 1;
    public int gridCols = 1;
    public float header = 2f;
    public float margin = 1f;
    private bool endgame = false;

    private bool _paused;

    public void ShowPauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }

    public bool paused {
        get {
            return _paused; 
        }
        set {
            _paused = !_paused;
            ShowPauseMenu(_paused);
        }
    }

    public void restart() {
        SceneManager.LoadScene("CardGame");
    }

    public void goToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !endgame)
        {
            Debug.Log("escape");
            _paused = !_paused;
            ShowPauseMenu(_paused);
        }
        if (_paused) return;

        if (_movimientos >= _maxMV) {
            StartCoroutine(Endgame());
        }
    }

        void Start()
    {
        EndgameMenu.SetActive(false);
        _paused = false;
        pauseMenu.SetActive(_paused);
        int[] ids = new int[gridRows * gridCols];
        for (int i = 0; i < ids.Length; i++)
        {
            ids[i] = i / 2;
        }
        
        ids = ShuffleArray(ids);

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
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length - 1; i++)
        {
            int r = Random.Range(i, newArray.Length);
            int tmp = newArray[i];
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public bool canReveal
    {
        get { return _secondRevealed == null && _movimientos < _maxMV && !paused; }
    }
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

    private IEnumerator CheckMoves() {

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

    private IEnumerator Endgame() {
        endgame = true;
        yield return new WaitForSeconds(1.0f);
        finalScore.color = Color.yellow;
        if (_score >= 10) { finalScore.color = Color.green; }
        if (_score <= 5) { finalScore.color = Color.red; }
        finalScore.text = _score + "/" + (gridCols*gridRows/2);
        EndgameMenu.SetActive(true);
    }
}
