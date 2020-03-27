using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public float speed = 5f;

    private const float colliderSizeReductionFactor = 0.9f;


    private Vector2 directionAxis;
    private float offset;

    private Rigidbody2D _rb;
    private BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();

        directionAxis = transform.localScale.y > transform.localScale.x ? Vector2.up : Vector2.right;
        offset = transform.localScale.y > transform.localScale.x ? transform.position.y : transform.position.x;
        offset -= Mathf.FloorToInt(offset);
    }

    private void OnMouseDown()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;

        // Reduce a bit the collider size opposite to the block's direction, to avoid unwanted collisions
        if (directionAxis.y > 0)
        {
            _collider.size = new Vector2(_collider.size.x * colliderSizeReductionFactor, _collider.size.y);
        }
        else
        {
            _collider.size = new Vector2(_collider.size.x, _collider.size.y * colliderSizeReductionFactor);
        }
    }

    private void OnMouseUp()
    {
        _rb.velocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Static;

        RoundPositionToInt();

        // Increase collider size opposite to the block's direction back to its original size
        if (directionAxis.y > 0)
        {
            _collider.size = new Vector2(_collider.size.x / colliderSizeReductionFactor, _collider.size.y);
        }
        else
        {
            _collider.size = new Vector2(_collider.size.x, _collider.size.y / colliderSizeReductionFactor);
        }
    }

    private void OnMouseDrag()
    {
        Vector2 mouseMove = (GetMousePosition() - (Vector2)transform.position);

        float relevantAxis = directionAxis.y > 0 ? mouseMove.y : mouseMove.x;

        if (!Mathf.Approximately(relevantAxis, 0))
        {
            _rb.velocity = directionAxis * relevantAxis * speed;
        }
    }

    private void RoundPositionToInt()
    {
        // Round to closest .5 (0, 0.5, 1...)

        if (directionAxis.y > 0)
        {
            float y = Mathf.RoundToInt(transform.position.y + offset) - offset;
            //float y = Mathf.RoundToInt(transform.position.y * 2) / 2f;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        else
        {
            float x = Mathf.RoundToInt(transform.position.x + offset) - offset;
            //float x = Mathf.RoundToInt(transform.position.x * 2) / 2f;
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
    }

    private static Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}
