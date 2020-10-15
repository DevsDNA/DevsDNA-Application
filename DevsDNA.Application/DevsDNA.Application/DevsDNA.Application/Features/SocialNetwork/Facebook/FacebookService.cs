[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Features.SocialNetwork.Facebook.FacebookService))]
namespace DevsDNA.Application.Features.SocialNetwork.Facebook
{
    using DevsDNA.Application.Helpers;
    using DevsDNA.Application.Common;
    using Refit;
    using System.Collections.Generic;
    using System;
    using System.Reactive.Linq;

    public class FacebookService : IFacebookService
    {
        private readonly IFacebookAPIService facebookAPIService;
      
        public FacebookService() : this(RestService.For<IFacebookAPIService>(SettingsKeyValues.FacebookEndPoint))
        {
        }

        public FacebookService(IFacebookAPIService facebookAPIService)
        {
            this.facebookAPIService = facebookAPIService;
        }

        public IObservable<IList<PostModel>> GetPosts()
        {
            return facebookAPIService.GetPageFeed(SettingsKeyValues.FacebookPageId, Secrets.FacebookAccessToken).Select(p => p?.ToPosts());
        }
    }
}
