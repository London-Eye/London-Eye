using Assets.Scripts.Common;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    public bool StartDialogue = true;
    public bool InactiveAfterDestroy = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            // TODO: Add end animation before destroying the block
            Destroy(collision.gameObject);

            if (StartDialogue)
            {
                Utilities.StartPostGameDialogue();
            }
            else
            {
                Debug.Log("Block destroyed");
            }

            if (InactiveAfterDestroy)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
