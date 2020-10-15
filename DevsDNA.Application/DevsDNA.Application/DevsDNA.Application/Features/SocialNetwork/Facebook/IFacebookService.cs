namespace DevsDNA.Application.Features.SocialNetwork
{
    using System;
    using System.Collections.Generic;

    public interface IFacebookService
    {
        IObservable<IList<PostModel>> GetPosts();
    }
}
