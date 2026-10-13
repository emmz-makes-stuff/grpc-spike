namespace GrpcSpike.Benchmarks;

public static class RepositoryRoot
{
    private const string Marker = "GrpcSpike.slnx";

    /// <summary>The nearest directory above the working directory that holds the solution file.</summary>
    public static string Find()
    {
        for (var directory = new DirectoryInfo(Environment.CurrentDirectory); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, Marker)))
            {
                return directory.FullName;
            }
        }

        throw new InvalidOperationException($"No {Marker} found above '{Environment.CurrentDirectory}'. Run from inside the repository.");
    }
}
