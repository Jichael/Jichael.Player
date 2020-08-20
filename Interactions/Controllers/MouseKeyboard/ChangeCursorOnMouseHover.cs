using CustomPackages.SilicomPlayer.CursorSystem;
using CustomPackages.SilicomPlayer.Interactions;
using CustomPackages.SilicomPlayer.Players.MouseKeyboard;
using UnityEngine;

public class ChangeCursorOnMouseHover : MonoBehaviour, IMouseInteract
{
    [SerializeField] private CursorInfo cursor;

    public bool DisableInteraction { get; set; }
    public void LeftClick(MouseController mouseController) { }

    public void RightClick(MouseController mouseController) { }
    
    public void HoverEnter(MouseController mouseController)
    {
        CursorManager.Instance.SetCursor(cursor);
    }

    public void HoverStay(MouseController mouseController) { }

    public void HoverExit(MouseController mouseController)
    {
        CursorManager.Instance.ResetDefaultCursor();
    }
}
