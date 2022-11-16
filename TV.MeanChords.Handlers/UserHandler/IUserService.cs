using System;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.UserHandler
{
    public interface IUserService : IDisposable
    {
        ResponseBase<GetUserResponse> GetUser(GetUserRequest request);
        ResponseBase<PostUserResponse> PostUser(PostUserRequest request);
        ResponseBase<PutUserResponse> PutUser(PutUserRequest request);
    }
}
