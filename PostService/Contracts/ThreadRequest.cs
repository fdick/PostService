namespace PostService.API.Contracts
{
    public record ThreadRequest(string name, Guid authorId);
}
