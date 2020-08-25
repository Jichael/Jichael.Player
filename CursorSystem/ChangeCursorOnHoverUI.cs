using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomPackages.Silicom.Player.CursorSystem
{
    public class ChangeCursorOnHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] private CursorInfo hoverCursor;

        public void OnPointerEnter(PointerEventData eventData)
        {
            CursorManager.Instance.SetCursor(hoverCursor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            CursorManager.Instance.ResetDefaultCursor();
        }
    }
}
