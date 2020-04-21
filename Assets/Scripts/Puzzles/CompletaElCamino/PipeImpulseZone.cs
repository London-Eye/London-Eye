using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipeImpulseZone : MonoBehaviour
{
    public Vector2 ImpulseFactor;
    public Vector2 ImpulseDirection;
    public Space directionSpace;
    public bool relativeToCurrentVelocity = false;
    public bool addForce;

    public List<Vector2> allowedEntryDirections;

    [Header("Disable impulse by direction")]
    public bool DisableImpulseUp;
    public bool DisableImpulseLeft, DisableImpulseRight, DisableImpulseDown;

    [Tooltip("If true, it will change the velocity to 0, instead of not doing anything")]
    public bool freezeOnDisabled;

    private Vector2 CalculatedImpulseDirection(Vector2 velocity)
    {
        Vector2 impulseDirection = (directionSpace
            == Space.Self ? (Vector2)transform.TransformDirection(ImpulseDirection) : ImpulseDirection);

        if (relativeToCurrentVelocity) impulseDirection += velocity;

        return impulseDirection.normalized;
    }

    private bool IsImpulseDisabled(Vector2 velocity)
    {
        Vector2 impulseDirection = CalculatedImpulseDirection(velocity);

        return (DisableImpulseUp && IsCloserToDirection(impulseDirection, Vector2.up))
        || (DisableImpulseDown && IsCloserToDirection(impulseDirection, Vector2.down))
        || (DisableImpulseLeft && IsCloserToDirection(impulseDirection, Vector2.left))
        || (DisableImpulseRight && IsCloserToDirection(impulseDirection, Vector2.right));
    }

    private bool IsCloserToDirection(Vector2 vector, Vector2 direction)
        => Vector2.Angle(vector, direction) < 45f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb != null && IsEntryDirectionAllowed(rb.velocity))
        {
            if (IsImpulseDisabled(rb.velocity))
            {
                if (freezeOnDisabled)
                {
                    rb.velocity = Vector2.zero;
                }
            }
            else
            {
                var force = CalculatedImpulseDirection(rb.velocity) * ImpulseFactor;

                if (Mathf.Abs(force.x) > Mathf.Abs(force.y)) rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                else if (Mathf.Abs(force.y) > Mathf.Abs(force.x)) rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

                if (addForce) rb.AddForce(force, ForceMode2D.Impulse);
                else rb.velocity = force;
            }
        }
    }

    private bool IsEntryDirectionAllowed(Vector2 entryDirection) => allowedEntryDirections == null
        || allowedEntryDirections.Count == 0
        || allowedEntryDirections.Any(allowedDirection 
            => IsCloserToDirection(entryDirection, transform.TransformDirection(allowedDirection)));
}
