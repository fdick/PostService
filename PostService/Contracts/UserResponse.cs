namespace PostService.API.Contracts
{
    public record UserResponse(Guid id, string name, string lastname, string nickname, string email);
}
