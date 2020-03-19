using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public float speed = 5f;

    private const float colliderSizeReductionFactor = 0.9f;

    private Vector2 directionAxis;
    private Rigidbody2D _rb;
    private BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        directionAxis = transform.localScale.y > transform.localScale.x ? Vector2.up : Vector2.right;
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

    private static Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}
