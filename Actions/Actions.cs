namespace Windows.Configurations
{
    public static class Actions
    {
        public static class PainelControle
        {
            public static IWindowsAction DisableUac { get; } = new Features.PainelControle.DisableUac();

            public static IWindowsAction NoSoundScheme { get; } = new Features.PainelControle.NoSoundScheme();

            public static IWindowsAction DisableStartupSound { get; } = new Features.PainelControle.DisableStartupSound();

            public static IWindowsAction LidCloseDoNothing { get; } = new Features.PainelControle.LidCloseDoNothing();

            public static IWindowsAction NeverSleepOrTurnOffDisplay { get; } = new Features.PainelControle.NeverSleepOrTurnOffDisplay();
        }

        public static class Personalizacao
        {
            public static IWindowsAction TaskbarAlignAndSettings { get; } = new Features.Personalizacao.TaskbarAlignAndSettings();

            public static IWindowsAction DisableItemsTaskbar { get; } = new Features.Personalizacao.DisableItemsTaskbar();
        }
    }
}