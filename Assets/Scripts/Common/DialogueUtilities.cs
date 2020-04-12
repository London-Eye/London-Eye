using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Variables.Attributes;
using Assets.Scripts.Dialogue.Variables.Storages;
using System.Collections.Generic;
using UnityEngine;
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

        private SelectorPool<int> randomIntPool;
        private int randomIntPoolMin, randomIntPoolMax;

        [YarnAccess]
        public int RandomIntPool
        {
            get
            {
                if (randomIntPool == null || randomIntPoolMin != RandomIntMin || randomIntPoolMax != RandomIntMax)
                {
                    randomIntPoolMin = RandomIntMin;
                    randomIntPoolMax = RandomIntMax;

                    HashSet<int> pool = new HashSet<int>();
                    Utilities.AddIntRange(pool, randomIntPoolMin, randomIntPoolMax);

                    randomIntPool = new SelectorPool<int>(pool)
                    {
                        AutoRefill = true
                    };
                }

                return randomIntPool.Select();
            }
        }
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