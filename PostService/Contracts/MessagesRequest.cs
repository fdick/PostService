namespace PostService.API.Contracts
{
    public record MessagesRequest(
    Guid threadId,
    string msg,
    int likesQuantity,
    int dislikeQuantity,
    DateTime createTime
    );
}
