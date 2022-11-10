using System;
using System.Collections.Generic;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.UserHandler
{
    public interface IUserService : IDisposable
    {
        ResponseBase<PostUserResponse> PostUser(PostUserRequest request);
        ResponseBase<PutUserResponse> PutUser(PutUserRequest request);
    }
}
