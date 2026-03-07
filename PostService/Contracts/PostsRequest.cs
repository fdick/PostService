namespace PostService.API.Contracts
{
    public record PostsRequest(
        Guid threadId,
        Guid userId,
        string msg,
        int likesQuantity,
        int dislikeQuantity,
        Guid? parentMsgId
    );
}
