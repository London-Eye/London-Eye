using UnityEngine;

public class Impulser : MonoBehaviour
{
    public Rigidbody2D objectToImpulse;
    public Vector2 force;

    public void Impulse() => Impulse(force);

    public void Impulse(Vector2 velocity)
    {
        objectToImpulse.gameObject.SetActive(true);
        objectToImpulse.AddForce(velocity, ForceMode2D.Impulse);
    }
}
