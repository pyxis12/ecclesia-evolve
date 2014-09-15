﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Xamarin.Ecclesia.Settings;
using Xamarin.Forms;
using Xamarin.Ecclesia.XML;
using Xamarin.Ecclesia.Parse;

namespace Xamarin.Ecclesia.ViewModels
{
    public class QuizGroupViewModel :QuizDataBaseViewModel
    {
        #region Constructor
        public QuizGroupViewModel():base()
        {
            Title = "Quizzes";
            BackgroundColor = AppSettings.PageBackgroundColor;
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        #endregion

        #region Methods
        
        #endregion
    }
}
