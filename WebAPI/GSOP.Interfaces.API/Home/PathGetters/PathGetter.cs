namespace GSOP.Interfaces.API.Home.PathGetters;

/// <inheritdoc/>
public class PathGetter : IPathGetter
{
    private const string _uiStaticFilesFolderInternalPath = "wwwroot/UI";
    private const string _reactOutputFileName = "index.html";

    private string _uiStaticFilesFolderPath;
    private string _uiFilePath;

    /// <inheritdoc/>
    public string UiStaticFilesFolderPath { get { return _uiStaticFilesFolderPath ??= UiStaticFilesFolderFullPath; } }

    /// <inheritdoc/>
    public string UiFilePath { get { return _uiFilePath ??= GetUiFileFullPath; } }

    /// <summary>
    /// Get full path to UI static folder.
    /// </summary>
    private string UiStaticFilesFolderFullPath => Path.Combine(Directory.GetCurrentDirectory(), _uiStaticFilesFolderInternalPath);

    /// <summary>
    /// Get full path to main UI file.
    /// </summary>
    private string GetUiFileFullPath => Path.Combine(UiStaticFilesFolderPath, _reactOutputFileName);
}
