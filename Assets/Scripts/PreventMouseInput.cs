using UnityEngine;
using UnityEngine.EventSystems;

public class PreventMouseInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Disable following when the mouse enters the UI element
        FruitHandler.Instance.SetCanTakeMouseInput(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Enable following when the mouse exits the UI element
        FruitHandler.Instance.SetCanTakeMouseInput(true);
    }
}
