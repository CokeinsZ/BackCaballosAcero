// Infrastructure/Data/Queries/QueryHelper.cs

using System.Reflection;

namespace Infrastructure.Data.Queries;

public static class QueryHelper
{
    public static async Task<string> LoadQueryAsync(string queryName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"Infrastructure.Data.Queries.Statistics.{queryName}.sql";
        
        await using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new FileNotFoundException($"No se encontró el archivo SQL: {resourceName}");
        
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}