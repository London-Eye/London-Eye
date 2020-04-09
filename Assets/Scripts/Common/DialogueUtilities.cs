using Assets.Scripts.Dialogue.Texts.Variables;
using UnityEngine;
using Yarn.Unity;

namespace Assets.Scripts.Common
{
    public class DialogueUtilities : MonoBehaviour
    {
        public void BindPersistentStorage(CodeRelayVariableStorage relay)
        {
            if (CharacterCreation.Instance != null)
            {
                relay.BindStorage(CharacterCreation.Instance.gameObject.GetComponent<VariableStorageBehaviour>());
            }
        }

        public void ResetBindingPersistentStorage(CodeRelayVariableStorage relay)
        {
            BindPersistentStorage(relay);
            relay.ResetToDefaults();
        }

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