using Assets.Scripts.Common;
using UnityEngine;
using Yarn.Unity;

public class DialogueUtilities : MonoBehaviour
{
    public void StartPostGameDialogue(GameObject gameObjectToDeactivate = null)
    {
        gameObjectToDeactivate.SetActive(false);
        StartPostGameDialogue();
    }

    public void StartPostGameDialogue() => Utilities.StartPostGameDialogue();

    [YarnCommand("BackToMainMenu")]
    public void BackToMainMenu()
    {
        // Temporary solution to update the number of evidences.
        // TODO: When updating Yarn, use our new Variable Storage solution to store the variable accordingly.
        CharacterCreation.Instance.CurrentSuspect.evidencesFound
            += (int) GetComponentInChildren<VariableStorageBehaviour>().GetValue("$numero_pruebas").AsNumber;
        
        PauseController.GoToMainMenu();
    }
}
