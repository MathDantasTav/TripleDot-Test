using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LanguageManager : MonoBehaviour
{
    [SerializeField] private string _translationsSheetLink;
    
    public async void UpdateCachedTranslations()
    {
        string[,] sheet = await LoadSheetAsync();
        if (sheet == null)
        {
            Debug.LogError("Translation sheet not found");
            return;
        }
        
        LanguageTranslations.Translations.CachedTranslations.Clear();
        
        // This is done to keep the order of the translations in the translationData always in sync with the enum, no matter what order the excel sheet is in
        var languageRowIndexes = new List<LanguageTranslations.Languages>();
        for (int x = 1; x < sheet.GetLength(0); x++)
        {
            if (!Enum.TryParse(sheet[x, 0], out LanguageTranslations.Languages lang))
            {
                Debug.LogError($"Invalid language: {sheet[x, 0]}");
                continue;
            }
            languageRowIndexes.Add(lang);
        }
        
        // Loop through each row of the sheet
        // each row should contain one key and its translations
        // first row is ignored since it contains only the language names
        for (int y = 1; y < sheet.GetLength(1); y++)
        {
            var data = new LanguageTranslations.Data(sheet[0, y]);

            for (int x = 1; x < sheet.GetLength(0); x++)
                data.Translations.Add(sheet[(int)languageRowIndexes[x - 1] + 1, y]);
            
            LanguageTranslations.Translations.CachedTranslations.Add(data);
        }
        
        LanguageTranslations.Translations.CreateTranslationLookup();
        
#if UNITY_EDITOR
        EditorUtility.SetDirty(LanguageTranslations.Translations);
        AssetDatabase.SaveAssets();
#endif
    }
    
    private async Task<string[,]> LoadSheetAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(_translationsSheetLink))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                return null;
            }

            string csv = request.downloadHandler.text;

            string[] rows = csv.Split('\n');

            string[] firstRowCols = rows[0].Split(',');

            int width = firstRowCols.Length;
            int height = rows.Length;

            string[,] grid = new string[width, height];

            for (int y = 0; y < height; y++)
            {
                string[] cols = rows[y].Split(',');

                for (int x = 0; x < width; x++)
                {
                    if (x < cols.Length)
                        grid[x, y] = cols[x];
                    else
                        grid[x, y] = "";
                }
            }

            return grid;
        }
    }
}
