using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    public enum ScreenType
    {
        Settings,
        Languages,
        LevelCompleted,
        Debug
    }
    
    public static ScreensManager Instance;
    [SerializeField] private List<ScreenInstance> _screens;
    [SerializeField] private Transform _popupParent;

    private void Awake()
    {
        Instance = this;
    }

    private ScreenInstance GetScreen(ScreenType screenType)
    {
        foreach (var screen in _screens)
        {
            if (screen.Type == screenType)
                return screen;
        }

        return null;
    }

    public ScreenInstance OpenScreen(ScreenType screenType)
    {
        var screen = GetScreen(screenType);
        if (screen == null) return null;
        
        var screenObj = Instantiate(screen, _popupParent);
        screenObj.Appear();
        
        return screenObj;
    }
}
