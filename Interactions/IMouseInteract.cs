using CustomPackages.SilicomPlayer.Players.MouseKeyboard;

namespace CustomPackages.SilicomPlayer.Interactions
{
    public interface IMouseInteract : IBaseInteract
    {
        void LeftClick(MouseController mouseController);
        void RightClick(MouseController mouseController);
        void HoverEnter(MouseController mouseController);
        void HoverStay(MouseController mouseController);
        void HoverExit(MouseController mouseController);
    }

    public static class IMouseInteractExtensions
    {
        public static void LeftClick(this IMouseInteract[] mouseInteractions, MouseController mouseController)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].LeftClick(mouseController);
            }
        }
    
        public static void RightClick(this IMouseInteract[] mouseInteractions, MouseController mouseController)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].RightClick(mouseController);
            }
        }
    
        public static void HoverEnter(this IMouseInteract[] mouseInteractions, MouseController mouseController)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].HoverEnter(mouseController);
            }
        }
    
        public static void HoverStay(this IMouseInteract[] mouseInteractions, MouseController mouseController)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].HoverStay(mouseController);
            }
        }
    
        public static void HoverExit(this IMouseInteract[] mouseInteractions, MouseController mouseController)
        {
            for (int i = 0; i < mouseInteractions.Length; i++)
            {
                if(mouseInteractions[i].DisableInteraction) continue;
                mouseInteractions[i].HoverExit(mouseController);
            }
        }
    }
}