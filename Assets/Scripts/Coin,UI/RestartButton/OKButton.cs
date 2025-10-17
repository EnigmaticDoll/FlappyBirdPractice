using UnityEngine;
using UnityEngine.EventSystems;

public class OKButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIController.Instance != null)
        {
            //UIController.Instance.RestartGame();
            SendMessageUpwards("OnPressRestartButton");
        }
    }
}