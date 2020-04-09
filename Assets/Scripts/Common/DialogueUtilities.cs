using Assets.Scripts.Dialogue.Texts.Variables;
using UnityEngine;
using Yarn.Unity;

namespace Assets.Scripts.Common
{
    public class DialogueUtilities : MonoBehaviour
    {
        public void BindPersistentStorage(CodeRelayVariableStorage relay)
            => relay.BindStorage(CharacterCreation.Instance.gameObject.GetComponent<VariableStorageBehaviour>());


        public void StartPostGameDialogue(GameObject gameObjectToDeactivate)
        {
            gameObjectToDeactivate.SetActive(false);
            StartPostGameDialogue();
        }

        public void StartPostGameDialogue() => Utilities.StartPostGameDialogue();

        [YarnCommand("BackToMainMenu")]
        public void BackToMainMenu() => PauseController.GoToMainMenu();
    }
}