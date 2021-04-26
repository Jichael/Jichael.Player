using System;
using CustomPackages.Silicom.Player.Interactions.Views;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace CustomPackages.Silicom.Player.CursorSystem
{
    [Obsolete("OBSOLETE : Use EventsOnUI + ChangeCursor instead")]
    public class ChangeCursorOnHoverUI : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
    {

        public CursorInfo hoverCursor;
        public bool disabled;
        
        /* OBSOLETE : Use EventsOnUI + ChangeCursor instead

        private bool _isHovering;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovering = true;
            SetCursor();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovering = false;
            SetCursor();
        }

        private void OnDisable()
        {
            if (!_isHovering) return;
            _isHovering = false;
            if(CursorManager.Instance) SetCursor();
        }

        public void SetCursor()
        {
            if(disabled) return;
            if (_isHovering)
            {
                CursorManager.Instance.SetCursor(hoverCursor);
            }
            else
            {
                CursorManager.Instance.ResetDefaultCursor();
            }
        }
        
        */

#if UNITY_EDITOR
        
        
        private void OnValidate()
        {
            Debug.Log($"{name} has obsolete component {nameof(ChangeCursorOnHoverUI)} that should be removed", this);
            //EditorApplication.delayCall += Destroy;
        }
        

        private void Destroy()
        {
            Undo.RecordObject(gameObject, "Changed to EventsOnUI");
            EventsOnUI evui = GetComponent<EventsOnUI>();
            if (evui == null) evui = gameObject.AddComponent<EventsOnUI>();

            ChangeCursor c = GetComponent<ChangeCursor>();
            if(c == null) c = gameObject.AddComponent<ChangeCursor>();
            
            c.cursor = hoverCursor;
            
            evui.pointerEnterEvents = new UnityEvent[1];
            evui.pointerEnterEvents[0] = new UnityEvent();
            
            var targetInfo = UnityEvent.GetValidMethodInfo(c, nameof(c.SetCursor), new Type[0]);
            UnityAction methodDelegate = Delegate.CreateDelegate(typeof(UnityAction), c, targetInfo) as UnityAction;
            UnityEventTools.AddPersistentListener(evui.pointerEnterEvents[0], methodDelegate);
            
            evui.pointerExitEvents = new UnityEvent[1];
            evui.pointerExitEvents[0] = new UnityEvent();
            
            targetInfo = UnityEvent.GetValidMethodInfo(c, nameof(c.ResetCursor), new Type[0]);
            methodDelegate = Delegate.CreateDelegate(typeof(UnityAction), c, targetInfo) as UnityAction;
            UnityEventTools.AddPersistentListener(evui.pointerExitEvents[0], methodDelegate);

            Debug.Log($"Destroying ChangeCursorOnHoverUI on {name}", gameObject);
            DestroyImmediate(this);
        }
#endif
    }
}
