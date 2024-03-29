﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyMap.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyMap.Controls
{
    public partial class BottomToolBarView : ContentView, IDisposable
    {
        public delegate void TapDelegate();

        public BottomToolBarView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IList),
            typeof(BottomToolBarView),
            propertyChanged: OnItemsSourceModified);

        public IList ItemsSource
        {
            get
            {
                return (IList)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public static readonly BindableProperty ItemSelectedProperty =
            BindableProperty.Create(nameof(ItemSelected), typeof(ICommand), typeof(BottomToolBarView), null);

        public ICommand ItemSelected
        {
            get
            {
                return (ICommand)GetValue(ItemSelectedProperty);
            }
            set
            {
                SetValue(ItemSelectedProperty, value);
            }
        }

        public static readonly BindableProperty BarBackgroundColorProperty =
            BindableProperty.Create(nameof(BarBackgroundColor), typeof(Color), typeof(BottomToolBarView), default(Color));

        public Color BarBackgroundColor
        {
            get
            {
                return (Color)GetValue(BarBackgroundColorProperty);
            }
            set
            {
                SetValue(BarBackgroundColorProperty, value);
            }
        }

        public static readonly BindableProperty BarDetailBackgroundColorProperty =
            BindableProperty.Create(nameof(BarDetailBackgroundColor), typeof(Color), typeof(BottomToolBarView), default(Color));

        public Color BarDetailBackgroundColor
        {
            get
            {
                return (Color)GetValue(BarDetailBackgroundColorProperty);
            }
            set
            {
                SetValue(BarDetailBackgroundColorProperty, value);
            }
        }

        private static void OnItemsSourceModified(object sender, object oldValue, object newValue)
        {
            if (!(sender is BottomToolBarView initialsView))
                return;

            var source = (IList)newValue;
            if (source != null)
            {
                var list = source.Cast<ViewItem>().ToList();
                initialsView.SetValue(list);
            }

        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            //if (propertyName.Contains(nameof(BarDetailBackgroundColor)))
            //{
            //   DetailToolbar.BackgroundColor = BarDetailBackgroundColor;
            //}
            //if (propertyName.Contains(nameof(BarBackgroundColor)))
            //{
            //    this.BackgroundColor = BarBackgroundColor;
            //}
        }

        private void SetValue(List<ViewItem> items)
        {
            //items.Insert(0, new ViewItem { ImageSource = "outline_arrow.png", Title = string.Empty });
            //DetailToolbar.BackgroundColor = BarDetailBackgroundColor;
            SToolbar.Children.Clear();
            var index = 0;
            var toolbar = new Grid
            {
                BackgroundColor = BarBackgroundColor,
                HeightRequest = 70,
                RowSpacing = 0,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            toolbar.ColumnDefinitions = new ColumnDefinitionCollection();

            foreach (var item in items)
            {
                toolbar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(items.Count + 1, GridUnitType.Star) });
                var stack = new StackLayout { Orientation = StackOrientation.Vertical,  Margin = new Thickness(0, 10, 0, 10), };
                var image = new ExtendedImageButton
                {
                    Source = item.ImageSource,
                    HeightRequest = 27,
                    WidthRequest = 27,
                   
                    TintColor = (Color)App.Current.Resources["TextDarkColor"],
                    BackgroundColor = Color.Transparent,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                stack.Children.Add(image);
                var label = new Label { Text = item.Title,
                    TextColor = (Color)App.Current.Resources["TextDarkColor"],
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand
                };
                stack.Children.Add(label);
                //if (index == 0)
                //{
                //    image.Clicked += async (s, e) =>
                //    {
                //        if (!DetailToolbar.IsVisible)
                //        {
                //            await image.RotateTo(180, 150);
                //            await this.TranslateTo(0.0f, -15f, 150);
                //            DetailToolbar.IsVisible = true;
                //        }
                //        else
                //        {
                //            await image.RotateTo(0, 150);
                //            await this.TranslateTo(0.0f, (items.Count - 1) * 44, 150);
                //            DetailToolbar.IsVisible = false;
                //        }
                //    };
                //}
                //else
                //{
                var _expandGestureRecognizer = new TapGestureRecognizer();
                _expandGestureRecognizer.Command = new Command(() => ItemSelected.Execute(item));

                stack.GestureRecognizers.Add(_expandGestureRecognizer);
                image.Command = new Command(() => ItemSelected.Execute(item));
                //}
                toolbar.Children.Add(stack, index++, 0);
            }

            SToolbar.Children.Add(toolbar, 0, 0);
            //SToolbar.HeightRequest = (items.Count - 1) * 44 + 50;
            //TranslationY = (items.Count - 1) * 44;
        }

        public void Dispose()
        {
            this?.Dispose();
        }
    }

    public class ViewItem : INotifyPropertyChanged
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        public string ImageSource { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

}
