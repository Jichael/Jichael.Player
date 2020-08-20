using CustomPackages.SilicomPlayer.CursorSystem;

namespace CustomPackages.SilicomPlayer.Interactions
{
    public interface IMouseInteract : IBaseInteract
    {
        void LeftClick();
        void RightClick();
        void HoverEnter();
        void HoverStay();
        void HoverExit();
    
        CursorInfo HoverCursor { get; }
    }

    public static class IMouseInteractExtensions
    {
        public static void LeftClick(this IMouseInteract[] mouseInteractions)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].LeftClick();
            }
        }
    
        public static void RightClick(this IMouseInteract[] mouseInteractions)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].RightClick();
            }
        }
    
        public static void HoverEnter(this IMouseInteract[] mouseInteractions)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].HoverEnter();
            }
        }
    
        public static void HoverStay(this IMouseInteract[] mouseInteractions)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].HoverStay();
            }
        }
    
        public static void HoverExit(this IMouseInteract[] mouseInteractions)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].HoverExit();
            }
        }

        // TODO : Jerome : comment on gere ca ? premier, dernier, autre ?
        // add a bool SwapCursor on interactions and check for it ?
        public static void SetHoverCursor(this IMouseInteract[] mouseInteractions)
        {
            if (mouseInteractions == null || mouseInteractions.Length == 0)
            {
                CursorManager.Instance.ResetDefaultCursor();
            }
            else
            {
                CursorManager.Instance.SetCursor(mouseInteractions[0].HoverCursor);
            }
        }
    }
}