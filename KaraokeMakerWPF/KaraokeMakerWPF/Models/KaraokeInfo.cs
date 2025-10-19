namespace KaraokeMakerWPF.Models;

public sealed class KaraokeInfo
{
    public string ImageFilePath { get; set; } = string.Empty;
    public string FontFilePath { get; set; } = string.Empty;
    public string MusicFilePath { get; set; } = string.Empty;
    public SongLineInfo[] SongLines { get; private set; } = [];

    public void SetSongLines(string[] lines)
    {
        SongLines = lines
            .Select((line, index) => new SongLineInfo(index, line))
            .ToArray();
    }
}