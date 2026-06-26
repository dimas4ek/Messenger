using System.Globalization;

namespace Client.Utils;

public static class LanguageManager
{
    public static event Action? LanguageChanged;
    public static string CurrentLanguage => Thread.CurrentThread.CurrentCulture.Name;
    
    public static void SetLanguage(string cultureName) 
    {
        var culture = new CultureInfo(cultureName);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        Properties.Settings.Default.Language = cultureName;
        Properties.Settings.Default.Save();

        LanguageChanged?.Invoke();
    }

    public static void LoadSavedLanguage()
    {
        var lang = Properties.Settings.Default.Language;
        if (string.IsNullOrEmpty(lang)) return;
        
        var culture = new CultureInfo(lang);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}