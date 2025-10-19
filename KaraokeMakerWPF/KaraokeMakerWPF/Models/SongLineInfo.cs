namespace KaraokeMakerWPF.Models;

public sealed class SongLineInfo
{
    public int Index { get; init; }
    public long StartTime { get; private set; }
    public long EndTime { get; private set; }
    public string Text { get; init; }

    public SongLineInfo(int index, string text)
    {
        Index = index;
        Text = text;
    }

    public void SetTime(long startTime, long endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}