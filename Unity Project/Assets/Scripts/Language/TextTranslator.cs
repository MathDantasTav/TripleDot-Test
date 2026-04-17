using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextTranslator : MonoBehaviour
{
    [SerializeField] private string _translationKey;

    private TMP_Text Target
    {
        get
        {
            if (_target == null)
                _target = GetComponent<TMP_Text>();
            return _target;
        }
    }
    private TMP_Text _target;

    private void Start()
    {
        var translations = LanguageTranslations.Translations;
        translations.OnLanguageChanged.AddListener(UpdateTargetText);

        UpdateTargetText();
    }
    
    private void OnDestroy()
    {
        var translations = LanguageTranslations.Translations;
        
        if (translations != null)
            translations.OnLanguageChanged.RemoveListener(UpdateTargetText);
    }

    public void UpdateTargetText()
    {
        var translations = LanguageTranslations.Translations;
        if (translations == null) return;
        
        UpdateTargetText(translations.CurrentLanguage);
    }

    private void UpdateTargetText(LanguageTranslations.Languages language)
    {
        Target.text = LanguageTranslations.Translations?.GetTranslation(_translationKey) ?? "";
    }
}
