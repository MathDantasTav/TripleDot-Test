using UnityEngine;

public class ScreenDebug : ScreenInstance
{
    private BottomBarView _bottomBarView;
    
    public void Populate(BottomBarView bottomBarView)
    {
        _bottomBarView = bottomBarView;
    }

    public void ToggleBottomBarVisible()
    {
        if (_bottomBarView == null) return;
        
        _bottomBarView.Visible = !_bottomBarView.Visible;
    }
    
    public void ToggleBottomBarButtonLocked(int button)
    {
        if (_bottomBarView == null) return;
        if (_bottomBarView.Buttons.Count <= button) return;
        
        var buttonInstance = _bottomBarView.Buttons[button];
        
        buttonInstance.Locked = !buttonInstance.Locked;
    }
    
    public void TriggerBottomBarButtonIconAnimation(int button)
    {
        if (_bottomBarView == null) return;
        if (_bottomBarView.Buttons.Count <= button) return;
        
        var buttonInstance = _bottomBarView.Buttons[button];
        
        buttonInstance.TriggerIconAnimation();
    }
}
