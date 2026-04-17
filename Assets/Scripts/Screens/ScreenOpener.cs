using UnityEngine;

public class ScreenOpener : MonoBehaviour
{
    [SerializeField] protected ScreensManager.ScreenType _screenType;
    
    public virtual void OpenScreen()
    {
        ScreensManager.Instance.OpenScreen(_screenType);
    }
}
