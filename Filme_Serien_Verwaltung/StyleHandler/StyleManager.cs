using System;
using System.Windows;
using MahApps.Metro;

namespace GUIApp.StyleHandler
{
    public static class StyleManager
    {
        public static Application Application { get; set; }

        public static void ChangeStyle(Themes theme)
        {
            ChangeStyle(null, theme);
        }

        private static Tuple<AppTheme, Accent> _oldStyle;

        public static void ChangeStyle(Accents? accent, Themes? theme = null)
        {
            var app = Application ?? Application.Current;
            _oldStyle = ThemeManager.DetectAppStyle(app);

            string newAccent = accent != null ? accent.ToString() : _oldStyle.Item2.Name;
            string newTheme = theme != null ? theme.ToString() : _oldStyle.Item1.Name;

            ThemeManager.ChangeAppStyle(app, ThemeManager.GetAccent(newAccent), ThemeManager.GetAppTheme(newTheme));
        }

        public static Themes getAppTheme()
        {
            Themes retTheme;
            var theme = ThemeManager.DetectAppStyle(Application.Current);

            try
            {
                Enum.TryParse(theme.Item1.Name, out retTheme);
                return retTheme;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        public static Accents getAppAccent()
        {
            Accents retAcc;
            var accent = ThemeManager.DetectAppStyle(Application.Current);

            try
            {
                Enum.TryParse(accent.Item2.Name, out retAcc);
                return retAcc;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

    }
}
