﻿using System;
using System.Collections.Generic;
using CoreGraphics;
using MapKit;
using MyMap.CustomViews;
using MyMap.iOS.CustomViews;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MyMap.iOS.CustomViews
{
    public class CustomMapRenderer : MapRenderer
    {
        UIView customPinView;
        List<CustomPin> customPins;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                var nativeMap = Control as MKMapView;
                if (nativeMap != null)
                {
                    nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    nativeMap.GetViewForAnnotation = null;
                    //nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
                    //nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    //nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                customPins = formsMap.CustomPins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                //nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
                //nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                //nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
            }
        }

        protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;
            if (annotation is MKUserLocation)
                return null;
            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }
            annotationView = mapView.DequeueReusableAnnotation(customPin.Name);
            if (annotationView == null)
            {
                annotationView = new MKAnnotationView(annotation, customPin.Name);
                //annotationView.Image = UIImage.FromFile("pin.png");
                annotationView.Image = UIImage.FromFile(customPin.Icon);
                annotationView.CalloutOffset = new CGPoint(0, 0);
            }
            annotationView.CanShowCallout = true;
            return annotationView;
        }
        CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
        //void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        //{
        //    //CustomMKAnnotationView customView = e.View as CustomMKAnnotationView;
        //    //customPinView = new UIView();

        //    //if (customView.Name.Equals("Xamarin"))
        //    //{
        //        customPinView.Frame = new CGRect(0, 0, 200, 84);
        //        var image = new UIImageView(new CGRect(0, 0, 200, 84));
        //        image.Image = UIImage.FromFile("xamarin.png");
        //        customPinView.AddSubview(image);
        //        customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
        //        e.View.AddSubview(customPinView);
        //    //}
        //}
        //protected override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        //{
        //    MKAnnotationView annotationView = null;

        //    if (annotation is MKUserLocation)
        //        return null;

        //    //var customPin = GetCustomPin(annotation as MKPointAnnotation);
        //    //if (customPin == null)
        //    //{
        //    //    throw new Exception("Custom pin not found");
        //    //}

        //    //annotationView = mapView.DequeueReusableAnnotation(customPin.Name);
        //    //if (annotationView == null)
        //    //{
        //    //    annotationView = new CustomMKAnnotationView(annotation, customPin.Name);
        //    //    annotationView.Image = UIImage.FromFile("pin.png");
        //    //    annotationView.CalloutOffset = new CGPoint(0, 0);
        //    //    annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("monkey.png"));
        //    //    annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
        //    //    ((CustomMKAnnotationView)annotationView).Name = customPin.Name;
        //    //    ((CustomMKAnnotationView)annotationView).Url = customPin.Url;
        //    //}
        //    annotationView.CanShowCallout = true;

        //    return annotationView;
        //}
        //void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
        //{
        //    //CustomMKAnnotationView customView = e.View as MKAnnotationView;
        //    //if (!string.IsNullOrWhiteSpace(customView.Url))
        //    //{
        //    //    UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
        //    //}
        //}
        //void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        //{
        //    if (!e.View.Selected)
        //    {
        //        customPinView.RemoveFromSuperview();
        //        customPinView.Dispose();
        //        customPinView = null;
        //    }
        //}
    }
}