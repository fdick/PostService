namespace PostService.API.Contracts
{
    public record ThreadRequest(Guid Id, string name, Guid authorId);
}
