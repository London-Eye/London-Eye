using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    private int _score = 0;
    private int _movimientos = 0;
    private int _maxMV = 4;

    public int gridRows = 1;
    public int gridCols = 1;
    public float header = 2f;
    public float margin = 1f;
    void Start()
    {
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
            Debug.Log(j);
            for (int i = 0; i < gridCols; i++)
            {
                Debug.Log(i);
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
        get { return _secondRevealed == null && _movimientos < _maxMV; }
    }
    public void CardRevealed(MemoryCard card)
    {
        if (_movimientos >= _maxMV)
        {
            Debug.Log("limite máximo de mv alcanzado");
        }
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
        
    }
    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
            _score++;
            Debug.Log("Score: " + _score);
            Debug.Log("Movimientos: " + _movimientos);
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            _firstRevealed.Unreveal();
            _secondRevealed.Unreveal();
        }
        _firstRevealed = _secondRevealed = null;
    }
}
