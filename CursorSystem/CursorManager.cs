﻿using System;
using CustomPackages.Silicom.Localization.Runtime;
using CustomPackages.Silicom.Player.Players;
using UnityEngine;

namespace CustomPackages.Silicom.Player.CursorSystem
{
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance { get; private set; }
        
        public bool IsLocked { get; private set; }
        
        public Vector3 CenterScreen { get; private set; }

        public event Action<bool> OnLockStateChanged;

        [SerializeField] private CursorBase fixedCursor;
        [SerializeField] private CursorBase defaultCursor;
        [SerializeField] private CursorBase zoomCursor;
        [SerializeField] private CursorBase handCursor;
        [SerializeField] private CursorBase informationCursor;
        [SerializeField] private CursorBase textCursor;
        [SerializeField] private CursorBase unzoomCursor;

        public CursorHintLevel hintLevel;

        [SerializeField] private CursorInfo defaultCursorInfo;        
        [SerializeField] private CursorInfo fixedCursorInfo;        

        private CursorBase _currentCursor;

        private Vector3 _cursorPosition;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Cursor.visible = false;

            defaultCursor.HideCursor();
            zoomCursor.HideCursor();
            handCursor.HideCursor();
            informationCursor.HideCursor();
            fixedCursor.HideCursor();
            textCursor.HideCursor();
            unzoomCursor.HideCursor();
            SetCursorNoHide(defaultCursorInfo);
            SetLockState(false);
        }

        private void OnDestroy()
        {
            if(Instance == this) Instance = null;
        }

        public void SetCursorPosition(Vector3 position)
        {
            _cursorPosition = position;
            _currentCursor.transform.position = _cursorPosition;
        }

        public void SetClickState(bool clickState)
        {
            _currentCursor.SetClickState(clickState);
        }

        public void SetCursor(CursorInfo cursorInfo)
        {
            _currentCursor.HideCursor();
            SetCursorNoHide(cursorInfo);
        }

        private void SetCursorNoHide(CursorInfo cursorInfo)
        {
            _currentCursor = CursorTypeToCursor(cursorInfo.cursorType);
            string hint = cursorInfo.GetHint(hintLevel);
        
            if (string.IsNullOrEmpty(hint))
            {
                _currentCursor.HideHint();
            }
            else
            {
                _currentCursor.ShowHint(hint);
            }

            if (cursorInfo.showInputIcon)
            {
                _currentCursor.ShowDisplayIcon(InputReferences.Instance.InputActionToIcon(InputsHandler.Instance.UseAction));
            }
            else
            {
                _currentCursor.HideDisplayIcon();
            }
        
            _currentCursor.ShowCursor();
            _currentCursor.transform.position = _cursorPosition;
        }
    
        public void ResetDefaultCursor()
        {
            SetCursor(IsLocked ? fixedCursorInfo : defaultCursorInfo);
        }

        public void SetLockState(bool isLocked)
        {
            IsLocked = isLocked;
            
            Cursor.lockState = IsLocked ? CursorLockMode.Locked : CursorLockMode.Confined;
            
            if (IsLocked)
            {
                CenterScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
                _cursorPosition = CenterScreen;
            }

            ResetDefaultCursor();
            OnLockStateChanged?.Invoke(IsLocked);
        }

        private CursorBase CursorTypeToCursor(CursorType cursorType)
        {
            switch (cursorType)
            {
                case CursorType.Default:
                    return defaultCursor;
                case CursorType.Zoom:
                    return zoomCursor;
                case CursorType.Hand:
                    return handCursor;
                case CursorType.Information:
                    return informationCursor;
                case CursorType.Fixed:
                    return fixedCursor;
                case CursorType.Text:
                    return textCursor;
                case CursorType.Unzoom:
                    return unzoomCursor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cursorType), cursorType, null);
            }
        }
    
    }

    public enum CursorType
    {
        Default,
        Zoom,
        Hand,
        Information,
        Fixed,
        Text,
        Unzoom
    }

    public enum CursorHintLevel
    {
        None,
        Simple,
        Detailed
    }

    [Serializable]
    public class CursorInfo
    {
        public CursorType cursorType;
        public bool showInputIcon;
        public string cursorHintSimple;
        public string cursorHintDetailed;
        [SerializeField] private bool needTranslation;

        public string GetHint(CursorHintLevel hintLevel)
        {
            if(needTranslation) TranslateHints();
            switch (hintLevel)
            {
                case CursorHintLevel.None:
                    return string.Empty;
                case CursorHintLevel.Simple:
                    return cursorHintSimple;
                case CursorHintLevel.Detailed:
                    return cursorHintDetailed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hintLevel), hintLevel, null);
            }
        }
    
        private void TranslateHints()
        {
            needTranslation = false;
            if(!string.IsNullOrEmpty(cursorHintSimple)) cursorHintSimple = LanguageManager.Instance.RequestValue(cursorHintSimple);
            if(!string.IsNullOrEmpty(cursorHintDetailed)) cursorHintDetailed = LanguageManager.Instance.RequestValue(cursorHintDetailed);
        }
    }
}