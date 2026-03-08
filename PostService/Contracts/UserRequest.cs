namespace PostService.API.Contracts
{
    public record UserRequest(Guid id, string name, string lastname, string nickname, string email);
}
