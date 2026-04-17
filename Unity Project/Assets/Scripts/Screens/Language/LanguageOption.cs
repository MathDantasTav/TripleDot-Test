using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class LanguageOption : MonoBehaviour
{
    [HideInInspector] public LanguageTranslations.Languages Language;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;
    private ScreenLanguage _screenLanguage;
    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        
        var button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnClick);
    }

    public void Populate(ScreenLanguage screenLanguage, LanguageTranslations.Languages language)
    {
        _text.text = language.ToString();
        _screenLanguage = screenLanguage;
        Language = language;

        if (LanguageTranslations.Translations != null)
        {
            Toggle(LanguageTranslations.Translations.CurrentLanguage == language, true);
            _image.sprite = LanguageTranslations.Translations.GetLanguageOption(Language).FlagIcon;
        }
    }

    public void Toggle(bool value, bool instant = false)
    {
        if (_toggle == null) return;
        
        if (value != _toggle.CurrentState)
            _toggle.SetState(value, instant);
    }

    public void OnClick()
    {
        if (_screenLanguage == null) return;

        _screenLanguage.SelectLanguage(Language);
    }
}
