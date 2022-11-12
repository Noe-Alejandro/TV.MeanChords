using System;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.LoginHandler
{
    public interface ILoginService : IDisposable
    {
        ResponseBase<PostLoginResponse> PostLogin(PostLoginRequest request);
    }
}
