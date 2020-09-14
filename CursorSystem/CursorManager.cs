using System;
using Jichaels.Localization;
using UnityEngine;

namespace CustomPackages.Silicom.Player.CursorSystem
{
    public class CursorManager : MonoBehaviour
    {
        public static CursorManager Instance { get; private set; }
        
        public bool IsLocked { get; private set; }
        
        public Vector3 CenterScreen { get; private set; }

        [SerializeField] private CursorBase fixedCursor;
        [SerializeField] private CursorBase defaultCursor;
        [SerializeField] private CursorBase zoomCursor;
        [SerializeField] private CursorBase handCursor;
        [SerializeField] private CursorBase informationCursor;

        public CursorHintLevel hintLevel;

        [SerializeField] private CursorInfo defaultCursorInfo;        
        [SerializeField] private CursorInfo fixedCursorInfo;        

        private CursorBase _currentCursor;

        private Vector3 _cursorPosition;

        private void Awake()
        {
            Cursor.visible = false;

            SetCursorNoHide(defaultCursorInfo);
            zoomCursor.HideCursor();
            handCursor.HideCursor();
            informationCursor.HideCursor();
            fixedCursor.HideCursor();
            SetLockState(false);
        }

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }

        public void SetCursorPosition(Vector3 position)
        {
            if (IsLocked) return;
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
            
            Cursor.lockState = IsLocked ? CursorLockMode.Locked : CursorLockMode.None;
            
            if (IsLocked)
            {
                CenterScreen = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
                _cursorPosition = CenterScreen;
            }

            ResetDefaultCursor();
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
        Fixed
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
        [SerializeField] private string cursorHintSimple;
        [SerializeField] private string cursorHintDetailed;
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
            cursorHintSimple = LanguageManager.Instance.RequestValue(cursorHintSimple);
            cursorHintDetailed = LanguageManager.Instance.RequestValue(cursorHintDetailed);
        }
    }
}