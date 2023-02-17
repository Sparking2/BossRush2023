using UnityEngine;

namespace UserInterface
{
    public class MainMessagesUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject winFlyer;
        [SerializeField]
        private GameObject loseFlyer;

        public void DisplayWin()
        {
            winFlyer.gameObject.SetActive(true);
        }

        public void DisplayLose()
        {
            loseFlyer.gameObject.SetActive(true);
        }
    }
}