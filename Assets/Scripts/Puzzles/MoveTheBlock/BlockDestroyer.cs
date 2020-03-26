using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class BlockDestroyer : MonoBehaviour
{
    private const string PostGameDialogueTag = "PostGame";

    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            // TODO: Add end animation before destroying the block
            Destroy(collision.gameObject);

            FindObjectOfType<DialogueRunner>().StartDialogue($"{SceneManager.GetActiveScene().name}-{PostGameDialogueTag}");
        }
    }
}
