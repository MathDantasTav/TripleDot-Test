using UnityEngine;

public class ScreenOpenerDebug : ScreenOpener
{
    [SerializeField] private BottomBarView _bottomBarView;

    public override void OpenScreen()
    {
        var screenDebug = (ScreenDebug)ScreensManager.Instance.OpenScreen(_screenType);
        if (screenDebug == null)
        {
            return;
        }
        screenDebug.Populate(_bottomBarView);
    }
}
