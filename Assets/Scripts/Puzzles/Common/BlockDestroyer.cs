using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class BlockDestroyer : MonoBehaviour
{
    private const string PostGameDialogueTag = "PostGame";

    public bool StartDialogue = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            // TODO: Add end animation before destroying the block
            Destroy(collision.gameObject);

            if (StartDialogue)
            {
                FindObjectOfType<DialogueRunner>().StartDialogue($"{SceneManager.GetActiveScene().name}-{PostGameDialogueTag}");
            }
            else
            {
                Debug.Log("Block destroyed");
            }
        }
    }
}
