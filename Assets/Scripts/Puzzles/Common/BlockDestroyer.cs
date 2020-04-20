using Assets.Scripts.Common;
using UnityEngine;
using UnityEngine.Events;

public class BlockDestroyer : MonoBehaviour
{
    public bool StartDialogue = true;
    public bool InactiveAfterDestroy = true;

    public UnityEvent onBlockDestroyed;

    private bool active = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Block"))
        {
            // TODO: Add end animation before destroying the block
            Destroy(collision.gameObject);

            if (StartDialogue)
            {
                FindObjectOfType<DialogueController>().StartPostGameDialogue();
            }
            else
            {
                Debug.Log("Block destroyed");
            }

            onBlockDestroyed.Invoke();

            if (InactiveAfterDestroy)
            {
                this.active = false;
            }
        }
    }

    public void MoveHere(Transform movedTransform)
    {
        movedTransform.position = transform.position;
    }
}
