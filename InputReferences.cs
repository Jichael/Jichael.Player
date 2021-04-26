using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace CustomPackages.Silicom.Player
{
    public class InputReferences : MonoBehaviour
    {
        public static InputReferences Instance { get; private set; }

        [OdinSerialize]
        public Dictionary<string, Sprite> test;

        public InputReference mouseLeftClick;
        public InputReference mouseRightClick;

        public InputReference keyboard1;
        public InputReference keyboard2;
        public InputReference keyboard3;
        public InputReference keyboard4;
        public InputReference keyboard5;
        public InputReference keyboard6;
        public InputReference keyboard7;
        public InputReference keyboard8;
        public InputReference keyboard9;
        public InputReference keyboard0;
        public InputReference keyboardA;
        public InputReference keyboardZ;
        public InputReference keyboardE;
        public InputReference keyboardR;
        public InputReference keyboardT;
        public InputReference keyboardY;
        public InputReference keyboardU;
        public InputReference keyboardI;
        public InputReference keyboardO;
        public InputReference keyboardP;
        public InputReference keyboardQ;
        public InputReference keyboardS;
        public InputReference keyboardD;
        public InputReference keyboardF;
        public InputReference keyboardG;
        public InputReference keyboardH;
        public InputReference keyboardJ;
        public InputReference keyboardK;
        public InputReference keyboardL;
        public InputReference keyboardM;
        public InputReference keyboardW;
        public InputReference keyboardX;
        public InputReference keyboardC;
        public InputReference keyboardV;
        public InputReference keyboardB;
        public InputReference keyboardN;
        public InputReference keyboardArrowLeft;
        public InputReference keyboardArrowUp;
        public InputReference keyboardArrowRight;
        public InputReference keyboardArrowDown;
        public InputReference keyboardSpace;
        public InputReference keyboardTab;
        public InputReference keyboardEnter;
        public InputReference keyboardMajLeft;
        public InputReference keyboardMajRight;
        public InputReference keyboardCtrlLeft;
        public InputReference keyboardCtrlRight;
        public InputReference keyboardAltLeft;
        public InputReference keyboardAltRight;
        public InputReference keyboardEscape;
        
        [Serializable]
        public class InputReference
        {
            public Sprite icon;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public Sprite InputActionToIcon(InputActionReference action)
        {
            ReadOnlyArray<InputControl> bindings = action.action.controls;
            
            if (bindings.Count != 1) return null;

            return bindings[0].displayName switch
            {
                "A" => keyboardA.icon,
                "Z" => keyboardZ.icon,
                "E" => keyboardE.icon,
                "R" => keyboardR.icon,
                "T" => keyboardT.icon,
                "Y" => keyboardY.icon,
                "U" => keyboardU.icon,
                "I" => keyboardI.icon,
                "O" => keyboardO.icon,
                "P" => keyboardP.icon,
                "Q" => keyboardQ.icon,
                "S" => keyboardS.icon,
                "D" => keyboardD.icon,
                "F" => keyboardF.icon,
                "G" => keyboardG.icon,
                "H" => keyboardH.icon,
                "J" => keyboardJ.icon,
                "K" => keyboardK.icon,
                "L" => keyboardL.icon,
                "M" => keyboardM.icon,
                "W" => keyboardW.icon,
                "X" => keyboardX.icon,
                "C" => keyboardC.icon,
                "V" => keyboardV.icon,
                "B" => keyboardB.icon,
                "N" => keyboardN.icon,
                "1" => keyboard1.icon,
                "2" => keyboard2.icon,
                "3" => keyboard3.icon,
                "4" => keyboard4.icon,
                "5" => keyboard5.icon,
                "6" => keyboard6.icon,
                "7" => keyboard7.icon,
                "8" => keyboard8.icon,
                "9" => keyboard9.icon,
                "0" => keyboard0.icon,
                _ => null
            };
        }
    }
}