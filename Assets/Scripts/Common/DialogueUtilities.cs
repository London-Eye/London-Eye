using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Variables.Attributes;
using Assets.Scripts.Dialogue.Variables.Storages;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Assets.Scripts.Common
{
    public class DialogueUtilities : MonoBehaviour
    {
        public DialogueRunner dialogueRunner;
        public ComplexDialogueUI dialogueUI;
        public SimpleAccessibleVariableStorage myVariableStorage;

        private void Awake()
        {
            dialogueUI.onDialogueStart.AddListener(() =>
            {
                myVariableStorage.AddIndicesFrom(this);
                myVariableStorage.AddIndicesFrom(dialogueUI);
            });
        }

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

        public void StartPostGameDialogue() => Utilities.StartPostGameDialogue(dialogueRunner); 
        #endregion


        [YarnCommand("BackToMainMenu")]
        public void BackToMainMenu() => PauseController.GoToMainMenu();
    }
}