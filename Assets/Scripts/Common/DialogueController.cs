using Assets.Scripts.Dialogue;
using Assets.Scripts.Dialogue.Variables.Attributes;
using Assets.Scripts.Dialogue.Variables.Storages;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

namespace Assets.Scripts.Common
{
    public class DialogueController : MonoBehaviour
    {
        public DialogueRunner dialogueRunner;
        public ComplexDialogueUI dialogueUI;

        public SimpleAccessibleVariableStorage myVariableStorage;

        public VariableStorageSelector variableStorageSelector;
        public VariableStorageSelector variableStorageSelectorFallback;

        public VariableStorageBehaviour localVariableStorage;
        public CodeRelayVariableStorage globalVariableStorage;

        public VariableStorageBehaviour localVariableStorageFallback;
        public CodeRelayVariableStorage globalVariableStorageFallback;

        private void Awake()
        {
            dialogueUI.onDialogueStart.AddListener(() =>
            {
                myVariableStorage.AddIndicesFrom(this);
                myVariableStorage.AddIndicesFrom(dialogueUI);
            });

            SelectVariableStorageMode();
        }

        public enum VariableStorageModes { Local, Global }

        [SerializeField]
        private VariableStorageModes variableStorageMode;

        public VariableStorageModes VariableStorageMode
        {
            get => variableStorageMode;
            set
            {
                variableStorageMode = value;
                SelectVariableStorageMode();
            }
        }

        private void SelectVariableStorageMode()
        {
            int variableStorageModeIndex = (int)VariableStorageMode;
            variableStorageSelector.Select(variableStorageModeIndex);
            variableStorageSelectorFallback.Select(variableStorageModeIndex);
        }

        [YarnAccess(name = nameof(VariableStorageMode))]
        public string VariableStorageModeAsString
        {
            get => VariableStorageMode.ToString();
            set => VariableStorageMode = (VariableStorageModes)System.Enum.Parse(typeof(VariableStorageModes), value, true);
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
        public void BindPersistentStorage()
        {
            if (CharacterCreation.Instance != null)
            {
                globalVariableStorage.BindStorage(CharacterCreation.Instance.VariableStorage);
            }
        }

        public void BindPersistentStorageFallback()
        {
            if (CharacterCreation.Instance != null)
            {
                globalVariableStorageFallback.BindStorage(CharacterCreation.Instance.FallbackVariableStorage);
            }
        }

        public void ResetBindingPersistentStorage()
        {
            BindPersistentStorage();
            globalVariableStorage.ResetToDefaults();
        }

        public void ResetBindingPersistentStorageFallback()
        {
            BindPersistentStorageFallback();
            globalVariableStorageFallback.ResetToDefaults();
        }
        #endregion

        #region PostGame Dialogue
        public const string PostGameDialogueTag = "PostGame";

        public static string PostGameDialogueNode => $"{SceneManager.GetActiveScene().name}-{PostGameDialogueTag}";

        public void StartPostGameDialogue(GameObject gameObjectToDeactivate)
        {
            gameObjectToDeactivate.SetActive(false);
            StartPostGameDialogue();
        }

        public void StartPostGameDialogue() => dialogueRunner.StartDialogue(PostGameDialogueNode);
        #endregion
    }
}