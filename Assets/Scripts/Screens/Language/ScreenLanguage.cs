using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLanguage : ScreenInstance
{
    [SerializeField] private LanguageOption _languageOptionPrefab;
    [SerializeField] private Transform _languageOptionParent;
    private List<LanguageOption> _languageOptions = new();

    private void Awake()
    {
        foreach (LanguageTranslations.Languages value in Enum.GetValues(typeof(LanguageTranslations.Languages)))
        {
            var option = Instantiate(_languageOptionPrefab, _languageOptionParent);
            option.Populate(this, value);
            _languageOptions.Add(option);
        }
    }
    
    public void SelectLanguage(LanguageTranslations.Languages language)
    {
        foreach (var languageOption in _languageOptions)
        {
            languageOption.Toggle(languageOption.Language == language);
        }
        LanguageTranslations.Translations.CurrentLanguage = language;
    }
}
