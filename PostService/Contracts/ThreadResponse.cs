namespace PostService.API.Contracts
{
    public record ThreadResponse(Guid id, string name, Guid authorId);
}
