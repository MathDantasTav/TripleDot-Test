using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BottomBarView : MonoBehaviour
{
    private static readonly int VisibleHash = Animator.StringToHash("Visible");
    
    public List<BottomBarButton> Buttons;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _startingPageIndex = 2;

    public UnityEvent<int> ContentActivated;
    public UnityEvent Closed;

    private bool _visible = true;
    public bool Visible
    {
        get { return _visible; }
        set
        {
            if (value == _visible)
            {
                return;
            }
            
            _visible = value;
            _animator.SetBool(VisibleHash, value);
        }
    }

    private void Awake()
    {
        for (int i = 0; i < Buttons.Count; i++)
        {
            var button = Buttons[i];
            button.ButtonIndex = i;
            button.View = this;
        }

        if (_startingPageIndex >= Buttons.Count) return;
        ButtonSelected(_startingPageIndex);
        
        // Temporary listeners added to demonstrate UnityEvent functionality,
        // since no external systems are currently hooked into these events
        ContentActivated.AddListener((int id) => { Debug.Log("[UI][BottomBar] Content Activated: " + id); });
        Closed.AddListener(() => { Debug.Log("[UI][BottomBar] Content Closed"); });
    }

    public void ButtonSelected(int buttonIndex, bool selected = true)
    {
        if (buttonIndex < 0 || buttonIndex >= Buttons.Count) return;
        
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i == buttonIndex) continue;

            if (Buttons[i].Selected) Buttons[i].Selected = false;
        }
        Buttons[buttonIndex].Selected = selected;
        
        if (selected)
            ContentActivated?.Invoke(buttonIndex);
        else
            Closed?.Invoke();
    }
}
