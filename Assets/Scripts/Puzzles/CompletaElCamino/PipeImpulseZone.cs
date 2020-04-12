using UnityEngine;

public class PipeImpulseZone : MonoBehaviour
{
    public Vector2 ImpulseFactor;

    [Header("Freeze impulse by direction")]
    public bool FreezeImpulseUp;
    public bool FreezeImpulseLeft, FreezeImpulseRight, FreezeImpulseDown;

    private bool ImpulseFrozen => (FreezeImpulseUp && transform.right == Vector3.up)
                     || (FreezeImpulseDown && transform.right == Vector3.down)
                     || (FreezeImpulseLeft && transform.right == Vector3.left)
                     || (FreezeImpulseRight && transform.right == Vector3.right);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb != null && Vector2.Angle(rb.velocity, (transform.up * -1)) < 45f)
        {
            if (ImpulseFrozen)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                rb.AddForce(transform.right * ImpulseFactor, ForceMode2D.Impulse);
            }
        }
    }
}
