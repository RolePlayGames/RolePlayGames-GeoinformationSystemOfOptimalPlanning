namespace GSOP.Interfaces.API.Home.PathGetters;

/// <summary>
/// Contains file system paths.
/// </summary>
public interface IPathGetter
{
    string UiStaticFilesFolderPath { get; }

    string UiFilePath { get; }
}
