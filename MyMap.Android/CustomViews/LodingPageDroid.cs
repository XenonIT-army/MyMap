﻿using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using MyMap.Droid.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFPlatform = Xamarin.Forms.Platform.Android.Platform;
using MyMap.Views.DialogViews;
using MyMap.CustomViews;

[assembly: Xamarin.Forms.Dependency(typeof(LodingPageDroid))]
namespace MyMap.Droid.CustomViews
{
    public class LodingPageDroid : ILoadingPage
    {
            private Android.Views.View _nativeView;

            private Dialog _dialog;

            private bool _isInitialized;

            public void InitLoadingPage(ContentPage loadingIndicatorPage)
            {
                // check if the page parameter is available
                if (loadingIndicatorPage != null)
                {
                    // build the loading page with native base
                    loadingIndicatorPage.Parent = Xamarin.Forms.Application.Current.MainPage;

                    loadingIndicatorPage.Layout(new Rectangle(0, 0,
                        Xamarin.Forms.Application.Current.MainPage.Width,
                        Xamarin.Forms.Application.Current.MainPage.Height));

                    var renderer = loadingIndicatorPage.GetOrCreateRenderer();

                    _nativeView = renderer.View;

                    _dialog = new Dialog(CrossCurrentActivity.Current.Activity);
                    _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
                    _dialog.SetCancelable(false);
                    _dialog.SetContentView(_nativeView);
                    Window window = _dialog.Window;
                    window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                    window.ClearFlags(WindowManagerFlags.DimBehind);
                    window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));

                    _isInitialized = true;
                }
            }

            public void ShowLoadingPage()
            {
                if (!_isInitialized)
                    InitLoadingPage(new LoadingPage());

                _dialog.Show();
            }

            private void XamFormsPage_Appearing(object sender, EventArgs e)
            {
                var animation = new Animation(callback: d => ((ContentPage)sender).Content.Rotation = d,
                                              start: ((ContentPage)sender).Content.Rotation,
                                              end: ((ContentPage)sender).Content.Rotation + 360,
                                              easing: Easing.Linear);
                animation.Commit(((ContentPage)sender).Content, "RotationLoopAnimation", 16, 800, null, null, () => true);
            }

            public void HideLoadingPage()
            {
                if (_dialog != null)
                    _dialog.Hide();
            }
        }

        internal static class PlatformExtension
        {
            public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
            {
                var renderer = XFPlatform.GetRenderer(bindable);
                if (renderer == null)
                {
                    renderer = XFPlatform.CreateRendererWithContext(bindable, CrossCurrentActivity.Current.Activity);
                    XFPlatform.SetRenderer(bindable, renderer);
                }
                return renderer;
            }
        }
    }
