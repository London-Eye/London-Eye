using UnityEngine;

public class PipeImpulseZone : MonoBehaviour
{
    public Vector2 ImpulseFactor;

    [Header("Disable impulse by direction")]
    public bool DisableImpulseUp;
    public bool DisableImpulseLeft, DisableImpulseRight, DisableImpulseDown;

    private bool ImpulseEnabled => !((DisableImpulseUp && transform.right == Vector3.up)
                     || (DisableImpulseDown && transform.right == Vector3.down)
                     || (DisableImpulseLeft && transform.right == Vector3.left)
                     || (DisableImpulseRight && transform.right == Vector3.right));

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb != null && Vector2.Angle(rb.velocity, (transform.up * -1)) < 45f)
        {
            rb.AddForce(transform.right * ImpulseFactor, ForceMode2D.Impulse);
        }
    }

    public void CheckRotationStatus() => this.gameObject.SetActive(ImpulseEnabled);
}
