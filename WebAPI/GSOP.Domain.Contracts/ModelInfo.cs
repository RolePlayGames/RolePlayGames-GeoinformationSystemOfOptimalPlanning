namespace GSOP.Domain.Contracts
{
    public record ModelInfo
    {
        public required long ID { get; init; }

        public required string Name { get; init; }
    }
}
