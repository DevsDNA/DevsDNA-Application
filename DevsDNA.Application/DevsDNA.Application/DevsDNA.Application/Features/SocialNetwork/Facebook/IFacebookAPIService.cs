namespace DevsDNA.Application.Features.SocialNetwork.Facebook
{
    using DevsDNA.Application.Features.SocialNetwork.Facebook.APIModels;
    using Refit;
    using System;

    public interface IFacebookAPIService
    {
        [Get("/v7.0/{page-id}/feed?fields=id,created_time,message,attachments&limit=25&access_token={access-token}")]
        IObservable<PageFeed> GetPageFeed([AliasAs("page-id")] string pageId, [AliasAs("access-token")] string accessToken);
    }
}
