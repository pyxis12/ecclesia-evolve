﻿using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Ecclesia.Models.Utils;
using Xamarin.Ecclesia.XML;
using Xamarin.Ecclesia.Auth;
using Xamarin.Ecclesia.Settings;
using Parse;
using Xamarin.Ecclesia.Parse;


namespace Xamarin.Ecclesia.Droid
{
    [Activity(Label = "Xamarin.Ecclesia", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        OAuthCommunicator _oAuth;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Initialize Xamarin.Forms
            Xamarin.Forms.Forms.Init(this, bundle);

            //XML loader instance
            XMLHelper.XMLLoader = new XMLLoader(this);

            //Initialize settings
            AppSettings.Init(new LocalSettings());

            // Initialize the parse client with your Application ID and .NET Key found on
            // your Parse dashboard
            ParseClient.Initialize(Ecclesia.Settings.Constants.ParseID, Ecclesia.Settings.Constants.ParseNETKey);
            ParseHelper.ParseData = new ParseData();

            //set main page
            SetPage(App.GetMainPage());

            _oAuth = new OAuthCommunicator();
            _oAuth.AuthUIRequest += oAuth_AuthUIRequest;
            AuthHelper.OAuthCommunicator = _oAuth;

        }

        void oAuth_AuthUIRequest()
        {
            var intent = _oAuth.Authenticator.GetUI(this);
            StartActivity(intent);
        }
    }
}

