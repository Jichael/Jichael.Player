using CustomPackages.Silicom.Player.Players;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockInteractionsOnHoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PlayerController _player;

    private bool _locked;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _player = PlayerController.Current;
        _locked = _player.LockedInteractions;
        if(!_locked) _player.LockInteractions();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!_locked) _player.UnlockInteractions();
    }

    private void OnDisable()
    {
        if(_player && !_locked) _player.UnlockInteractions();
    }
}
