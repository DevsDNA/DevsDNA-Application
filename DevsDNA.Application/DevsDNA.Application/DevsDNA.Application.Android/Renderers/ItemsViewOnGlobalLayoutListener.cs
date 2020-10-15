namespace DevsDNA.Application.Droid.Renderers
{
    using Android.Views;
    using System;

    class ItemsViewOnGlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
    {
        public EventHandler<EventArgs> LayoutReady;
        public void OnGlobalLayout()
        {
            LayoutReady?.Invoke(this, new EventArgs());
        }
    }   
}