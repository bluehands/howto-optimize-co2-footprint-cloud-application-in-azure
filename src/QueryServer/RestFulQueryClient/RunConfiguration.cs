using System.Text.Json;

namespace RestFulQueryClient;

public class RunConfiguration
{
    public string OutputFile { get; init; }
    public int FileBufferSize { get; init; }
    public int RequestCount { get; init; }
    public int PerSeconds { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public string RequestUri { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
};