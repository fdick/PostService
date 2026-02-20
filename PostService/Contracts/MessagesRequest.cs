namespace PostService.API.Contracts
{
    public record MessagesRequest(
        Guid threadId,
        Guid userId,
        string msg,
        int likesQuantity,
        int dislikeQuantity,
        Guid? parentMsgId
    );
}
