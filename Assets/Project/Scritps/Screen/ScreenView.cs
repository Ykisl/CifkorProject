using UnityEngine;

namespace CifkorApp.Screen
{
    public class ScreenView : MonoBehaviour
    {
        public void SetIsScreenActive(bool isScreenActive)
        {
            gameObject.SetActive(isScreenActive);
        }
    }
}
