using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "LanguageTranslations", menuName = "Scriptable Objects/LanguageTranslations")]
public class LanguageTranslations : ScriptableObject
{
    private const string TranslationsSOName = "Translations";
    private const string LanguageKey = "Language";
    
    public enum Languages
    {
        English,
        Português
    }
    
    [System.Serializable]
    public class LanguageInfo
    {
        public Languages Language;
        public Sprite FlagIcon;
    }
    
    [System.Serializable]
    public class Data
    {
        public string Key;
        public List<string> Translations;

        public Data(string key)
        {
            Key = key;
            Translations = new List<string>();
        }
    }
    
    public static LanguageTranslations Translations
    {
        get
        {
            if (_translations == null)
            {
                _translations = Resources.Load<LanguageTranslations>(TranslationsSOName);
            }
            return _translations;
        }
    }
    private static LanguageTranslations _translations;
    
    public List<LanguageInfo> LanguagesInfo = new();
    public List<Data> CachedTranslations = new List<Data>();
    
    /// <summary>
    /// Cached translations by key and language.
    /// String key   = translation ID  (e.g. "PLAY_BUTTON")
    /// Language key = target language (e.g. "English")
    /// Value        = translated text (e.g. "Play!")
    /// </summary>
    private Dictionary<string, Dictionary<Languages, string>> _translationLookup = new Dictionary<string, Dictionary<Languages, string>>();
    
    [HideInInspector] public UnityEvent<Languages> OnLanguageChanged;
    
    public Languages CurrentLanguage
    {
        get
        {
            if (_currentLanguage == null)
            {
                Languages language = (Languages)PlayerPrefs.GetInt(LanguageKey);
                if (Enum.IsDefined(typeof(Languages), language) == false)
                    language = Languages.English;

                _currentLanguage = language;
            }
            
            return _currentLanguage.Value;
        }
        set
        {
            _currentLanguage = value;
            OnLanguageChanged?.Invoke(_currentLanguage.Value);
            PlayerPrefs.SetInt(LanguageKey, (int)value);
        }
    }
    private Languages? _currentLanguage = null;

    public LanguageInfo GetLanguageOption(Languages language)
    {
        foreach (var option in LanguagesInfo)
        {
            if (option.Language == language)
                return option;
        }

        return null;
    }
    
    public string GetTranslation(string key, Languages? language = null)
    {
        if (string.IsNullOrEmpty(key)) return null;
        if (language == null) language = _currentLanguage;

        if (_translationLookup == null || _translationLookup.Count <= 0)
            CreateTranslationLookup();
        
        if (_translationLookup.TryGetValue(key, out var translations) == false)
        {
            Debug.LogError("Couldn't find translation key: " + key);
            return null;
        }

        if (translations.TryGetValue(language.Value, out var translation) == false)
        {
            // If translation was not available, try getting translation for English
            if (language.Value != Languages.English)
                return GetTranslation(key, Languages.English);
            
            // If English wasn't available, fail
            Debug.LogError("Couldn't find language: " + language.Value);
            return null;
        }

        return translation;
    }
    
    public void CreateTranslationLookup()
    {
        _translationLookup.Clear();
        
        foreach (var data in CachedTranslations)
        {
            var translationsDictionary = new Dictionary<Languages, string>();
            for (int i = 0; i < data.Translations.Count; i++)
            {
                translationsDictionary.Add((Languages)i, data.Translations[i]);
            }
            
            _translationLookup.Add(data.Key, translationsDictionary);
        }
    }
}
