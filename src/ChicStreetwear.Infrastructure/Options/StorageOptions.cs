namespace ChicStreetwear.Infrastructure.Options;

public sealed class CloudStorageOptions
{
    public const string SECTION_NAME = "CloudStorage:Azure";
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
