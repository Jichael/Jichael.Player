﻿using CustomPackages.Silicom.Player.Players.MouseKeyboard;

namespace CustomPackages.Silicom.Player.Interactions
{
    public interface IMouseInteract : IBaseInteract
    {
        void Use(MouseController mouseController);
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
                mouseInteractions[i].Use(mouseController);
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