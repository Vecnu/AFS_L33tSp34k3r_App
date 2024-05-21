namespace AFS_L33tSp34k3r_App.Interfaces
{
    public interface ITranslatorService
    {
        //Normally I'd use a generic but better sticking with string considering nature of the app.
        Task<string> TranslateAsync(string input);
    }
}

