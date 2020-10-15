namespace DevsDNA.Application.Features.Videos.VideoDetail
{
    using System;
    using Xamarin.Forms;
    
    public class WebviewYoutube : WebView
    {
        private const string VideoReadyAction = "videoReady";

        public static BindableProperty VideoModelProperty = BindableProperty.Create(nameof(VideoModel), typeof(VideoModel), typeof(WebviewYoutube), null, propertyChanged: OnVideoModelChanged);

        public VideoModel VideoModel
        {
            get { return (VideoModel)GetValue(VideoModelProperty); }
            set { SetValue(VideoModelProperty, value); }
        }

        public event EventHandler OnVideoReady;


        public void InvokeAction(string action)
        {
            if (string.IsNullOrEmpty(action))            
                return;

            if (action == VideoReadyAction)
                OnVideoReady?.Invoke(this, EventArgs.Empty);
        }

        private static void OnVideoModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WebviewYoutube webviewYoutube && newValue is VideoModel videoModel)
            {
                webviewYoutube.Source = new HtmlWebViewSource
                {                  
                    Html = "<script type='text/javascript'>" +
                           "    var player;" +
                           "    function createPlayer() {" +
                           "        var tag = document.createElement('script');" +
                           "        tag.src = 'https://www.youtube.com/iframe_api';" +
                           "        var firstScriptTag = document.getElementsByTagName('script')[0];" +
                           "        firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);" +
                           "    }" +
                           "    function onYouTubeIframeAPIReady() {" +
                           "        player = new YT.Player('youtube-video', {" +
                           "            events: {" +
                           "                'onReady': onPlayerReady" +
                           "            }" +
                           "        });" +
                           "    }" +
                           "    function onPlayerReady() {" +
                           $"        invokeCSharpAction('{VideoReadyAction}');" +
                           "    }" +
                           "</script>" +
                           "<body style='margin:0px; overflow: hidden;' onload='createPlayer()'>" +
                           $"    <iframe id ='youtube-video' width ='100%' height ='100%' src ='https://www.youtube.com/embed/{videoModel.Id}?enablejsapi=1&controls=1&fs=1&playsinline=1' frameborder ='0' allowfullscreen/>" +
                           "</body>"
                };
            }
        }
    }
}
