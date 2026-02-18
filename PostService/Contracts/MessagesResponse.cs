namespace PostService.API.Contracts
{
    public record MessagesResponse(
        Guid id,
        Guid threadId,
        string msg,
        int likesQuantity,
        int dislikeQuantity,
        DateTime createTime
        );
}
