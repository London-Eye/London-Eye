using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    private int _score = 0;
    private int _movimientos = 0;

    void Start()
    {
        EndgameMenu.SetActive(false);
        pauseMenu.IsPaused = false;

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

    void LateUpdate()
    {
        if (pauseMenu.IsPaused) return;

        if (_movimientos >= _maxMV)
        {
            StartCoroutine(Endgame());
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

    private IEnumerator Endgame()
    {
        yield return new WaitForSeconds(1.0f);

        finalScore.color = Color.yellow;
        if (_score >= 10) { finalScore.color = Color.green; }
        if (_score <= 5) { finalScore.color = Color.red; }
        finalScore.text = _score + "/" + (gridCols * gridRows / 2);

        EndgameMenu.SetActive(true);

        pauseMenu.IsPaused = false;
        pauseMenu.enabled = false;
    }
}
