using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] public FreezePipes container;

    private Vector3 initialPos;
    private Vector3 prevPos;
    private Vector3 actualPos;

    private Rigidbody2D rb;
    
    private int cont = 0;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        prevPos = initialPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf && cont == 60)
        {
            actualPos = gameObject.transform.position;

            if (IsOutOfBounds(actualPos) || (actualPos.x == prevPos.x && actualPos.y == prevPos.y))
            {
                container.pipeFreezer();
                gameObject.transform.position = new Vector3(initialPos.x, initialPos.y, initialPos.z);
                gameObject.SetActive(false);
                
            }
            prevPos = actualPos;
            cont = 0;
        }
        cont++;
    }

    public void Unfreeze()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private bool IsOutOfBounds(Vector3 pos) => pos.y < -15 || pos.y > 10 || pos.x < -10 || pos.x > 10;
}
