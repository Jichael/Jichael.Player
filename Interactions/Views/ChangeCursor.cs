using CustomPackages.Silicom.Player.CursorSystem;
using UnityEngine;

namespace CustomPackages.Silicom.Player.Interactions.Views
{
    public class ChangeCursor : MonoBehaviour
    {
        public CursorInfo cursor;

        public void SetCursor()
        {
            CursorManager.Instance.SetCursor(cursor);
        }

        public void ResetCursor()
        {
            CursorManager.Instance.ResetDefaultCursor();
        }
    }
}
