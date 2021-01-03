﻿using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace MyMap.CustomViews
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
        public CustomMap()
        {
            CustomPins = new List<CustomPin>();
        }
    }
}