﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CustomPackages.Silicom.Player.CursorSystem
{
    public class CursorBase : MonoBehaviour
    {
        public GameObject hint;
        public TextMeshProUGUI hintText;
        public GameObject inputIcon;
        public Image inputIconImage;

        [SerializeField] private Animator animator;

        private static readonly int ClickState = Animator.StringToHash("ClickState");

        public new RectTransform transform;

        private bool _clicked;

        public void SetClickState(bool clickState)
        {
            if (clickState && !_clicked)
            {
                OnStartClick();
            }
            else if (!clickState && _clicked)
            {
                OnStopClick();
            }
        }

        private void OnStartClick()
        {
            _clicked = true;
            SetAnimatorClickState();
        }
    
        private void OnStopClick()
        {
            _clicked = false;
            SetAnimatorClickState();
        }

        private void SetAnimatorClickState()
        {
            animator.SetBool(ClickState, _clicked);
        }

        public void ShowCursor()
        {
            gameObject.SetActive(true);
        }

        public void HideCursor()
        {
            gameObject.SetActive(false);
            HideHint();
        }

        public void ShowHint(string text)
        {
            hintText.text = text;
            hint.SetActive(true);
        }

        public void HideHint()
        {
            hint.SetActive(false);
        }

        public void ShowDisplayIcon(Sprite icon)
        {
            inputIconImage.sprite = icon;
            inputIcon.SetActive(true);
        }

        public void HideDisplayIcon()
        {
            inputIcon.SetActive(false);
        }
    }
}
