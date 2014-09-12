﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Xamarin.Ecclesia.DataObjects;
using Xamarin.Ecclesia.Parse;
using Xamarin.Ecclesia.Settings;
using Xamarin.Ecclesia.XML;
using Xamarin.Forms;


namespace Xamarin.Ecclesia.ViewModels
{
    public class QuizViewModel:ParentViewModel
    {
        #region Constructor
        
        /// <summary>
        /// Quiz from parse data
        /// </summary>
        /// <param name="data"></param>
        public QuizViewModel(Quiz quiz)
            : base()
        {
            Title = "Quiz";
            BackgroundColor = AppSettings.PageBackgroundColor;
            _quiz = quiz;
            _leaderboard = AppSettings.CurrentAccount.GetLeaderboardForQuiz(quiz.Name);
        }
        #endregion

        #region Fields
        Quiz _quiz;
        LeaderboardEntry _leaderboard;
        #endregion

        #region Properties
        public string ID {
            get
            { return _quiz.ID; }
        }
        public string Name {
            get { return _quiz.Name; }
        }
        public string Description {
            get { return _quiz.Description; }
        }

		public string Score
		{
			get
			{ 
                return AppSettings.CurrentAccount.GetQuizScore(Name).ToString ();
			}
		}

        #endregion

        #region Methods
        
        public async void LoadQuestionsFromParse()
        {
            if (Children != null && Children.Any())
                ClearChildren();
            var quizData = await ParseHelper.ParseData.GetQuestionsAsync(Name);

            foreach (var question in quizData)
            {
                AddChild(new QuestionViewModel(question));
            }
        }

        public override void ClearChildren()
        {
            foreach (QuestionViewModel vm in Children)
                vm.ClearChildren();
            base.ClearChildren();
        }

        public void SaveLeaderboard()
        {
            //this recalculates score
            var t =Score;
            ParseHelper.ParseData.SaveLeaderboard(_leaderboard);
        }
        #endregion
    }
}
