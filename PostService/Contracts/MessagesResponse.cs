namespace PostService.API.Contracts
{
    public record MessagesResponse(
        Guid id,
        Guid threadId,
        Guid userId,
        string msg,
        int likesQuantity,
        int dislikeQuantity,
        DateTime createTime,
        Guid? parentMessageId
        );
}
