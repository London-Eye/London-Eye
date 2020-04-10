using Assets.Scripts.Dialogue.Variables.Attributes;
using Assets.Scripts.Dialogue.Variables.Storages;
using UnityEngine;
using Yarn.Unity;

namespace Assets.Scripts.Common
{
    public class DialogueUtilities : MonoBehaviour
    {
        #region Randomizers
        [YarnAccess]
        public int RandomIntMin { get; set; } = 0;
        [YarnAccess]
        public int RandomIntMax { get; set; } = 1;
        [YarnAccess]
        public int RandomInt => Random.Range(RandomIntMin, RandomIntMax);
        #endregion

        #region CodeRelayVariableStorage Binding
        public void BindPersistentStorage(CodeRelayVariableStorage relay)
        {
            if (CharacterCreation.Instance != null)
            {
                relay.BindStorage(CharacterCreation.Instance.VariableStorage);
            }
        }

        public void ResetBindingPersistentStorage(CodeRelayVariableStorage relay)
        {
            BindPersistentStorage(relay);
            relay.ResetToDefaults();
        }
        #endregion

        #region PostGame Dialogue
        public void StartPostGameDialogue(GameObject gameObjectToDeactivate)
        {
            gameObjectToDeactivate.SetActive(false);
            StartPostGameDialogue();
        }

        public void StartPostGameDialogue() => Utilities.StartPostGameDialogue(); 
        #endregion


        [YarnCommand("BackToMainMenu")]
        public void BackToMainMenu() => PauseController.GoToMainMenu();
    }
}