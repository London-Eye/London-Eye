using UnityEngine;
using Yarn.Unity;

namespace Assets.Scripts.Common
{
    public class DialogueUtilities : MonoBehaviour
    {
        [YarnCommand("BackToMainMenu")]
        public void BackToMainMenu() => PauseController.GoToMainMenu();
    }
}
