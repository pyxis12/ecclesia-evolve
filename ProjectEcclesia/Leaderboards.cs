﻿using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

/**
 * Leaderboards namespace contains constructors and methods for all pages relating to Leaderboards
 * eg. LeaderboardOptionsPage and Top10Page
 * */
namespace Leaderboards {

	/**
	 * Contains constructor to create the leaderboard menu.
	 * User can choose to see the top 10 overall, sales, or trivia.
	 * */
	public class LeaderboardOptionsPage : ContentPage {

		static string whichBoard = "";

		/**
		 * Constructor for LeaderboardOptionsPage
		 * Allows user to choose from overall, sales, and triva
		 * */
		public LeaderboardOptionsPage () {
			NavigationPage.SetHasNavigationBar (this, false);
			BackgroundColor = Color.FromHex ("#ecf0f1");
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

			Label pageLabel = new Label() {
				Text = "Leaderboards",
				TextColor = Color.FromHex("#4e5758"),
				Font = Font.SystemFontOfSize(NamedSize.Large),
			};

			Button toOverallLeaders = new Button () {
				Text = "Overall",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toSalesLeaders = new Button () {
				Text = "Sales Playbook",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toTriviaLeaders = new Button () {
				Text = "Trivia",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toPeopleLeaders = new Button () {
				Text = "People",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toMainMenu = new Button () {
				Text = "Main Menu",
			};

			toOverallLeaders.Clicked += async (sender, e) => {
				whichBoard = "OverallPoints";
				var topUsers = await GetTopUsers(whichBoard);
				await this.Navigation.PushAsync(new Top10Page(whichBoard, topUsers));
			};

			toSalesLeaders.Clicked += async (sender, e) =>  {
				whichBoard = "SalesPoints";
				var topUsers = await GetTopUsers(whichBoard);
				await this.Navigation.PushAsync(new Top10Page(whichBoard, topUsers));
			};

			toTriviaLeaders.Clicked += async (sender, e) =>  {
				whichBoard = "TriviaPoints";
				var topUsers = await GetTopUsers(whichBoard);
				await this.Navigation.PushAsync(new Top10Page(whichBoard, topUsers));
			};

			toPeopleLeaders.Clicked += async (sender, e) => {
				whichBoard = "PeoplePoints";
				var topUsers = await GetTopUsers(whichBoard);
				await this.Navigation.PushAsync(new Top10Page(whichBoard, topUsers));
			};

			toMainMenu.Clicked += async (sender, e) => {
				await this.Navigation.PopAsync();
			};

			sl.Children.Add (pageLabel);
			sl.Children.Add (toOverallLeaders);
			sl.Children.Add (toSalesLeaders);
			sl.Children.Add (toTriviaLeaders);
			sl.Children.Add (toPeopleLeaders);
			sl.Children.Add (toMainMenu);

			Content = sl;
		}

		/**
		 * <summary>
		 * Queries for the top 10 users based on score for their category
		 * </summary>
		 * @param string whichBoard
		 * @return Task <IEnumerable<ParseObject>>
		 * */
		private async Task <IEnumerable <ParseObject>> GetTopUsers (string whichBoard) {
			var query = from user in ParseUser.Query
				.Limit (10)
				orderby user [whichBoard] descending
				select user;
			var topUsers = await query.FindAsync();
			return topUsers;
		}
	}

	/**
	 * Contains private inner Person class, defining each person for the leaderboard.
	 * Also gets the top 10 users in each category and displays their name, rank, and points
	 * in a listview.
	 * */

	public class Top10Page : ContentPage {

		string boardName;

		/**
		 * Private inner Person class.
		 * */
		class Person {

			/**
			 * <summary>
			 * Constructor for a person.
			 * </summary>
			 * @param int rank, string name, long points
			 * */
			public Person(int rank,  string name, long points) {
				this.Rank = rank.ToString() + ".";
				this.Name = name;
				this.Points = "   " + points.ToString();
			}

			public string Rank { private set; get; }
			public string Name { private set; get; }
			public string Points { private set; get; }
		}

		/**
		 * Constructor to create a leaderboard based on the IEnumerable of previously calculated topUsers 
		 * for the indicated board.
		 * */

		public Top10Page(string whichBoard, IEnumerable<ParseObject> topUsers) {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			SetBoardName (whichBoard);

			Title = string.Format("Top 10 {0}", boardName);

			int rank = 1;

			List <Person> top10 = new List <Person>();

			Console.WriteLine ("TopUsers " + topUsers.Count());

			foreach (ParseObject user in topUsers) {
				string name = (string) user ["Name"];
				long points = (long) user [whichBoard];

				top10.Add (new Person (rank, name, points));
				Console.WriteLine ("New Person " + top10.ToString ());
				rank++;
			}

			ListView listView = new ListView {
				BackgroundColor = Color.FromHex("#ecf0f1"),
				ItemsSource = top10,

				ItemTemplate = new DataTemplate(() => {
					Label rankLabel = new Label();
					rankLabel.SetBinding(Label.TextProperty, "Rank");
					rankLabel.TextColor = Color.Black;

					Label nameLabel = new Label();
					nameLabel.SetBinding(Label.TextProperty, "Name");
					nameLabel.TextColor = Color.FromHex("#b455b6");

					Label pointsLabel = new Label();
					pointsLabel.SetBinding(Label.TextProperty, "Points");
					pointsLabel.TextColor = Color.Navy;

					return new ViewCell () {
						View = new StackLayout () {
							Padding = new Thickness(0, 5),
							Orientation = StackOrientation.Horizontal,
							Children = {
								rankLabel,
								nameLabel,
								pointsLabel,
							},
						}
					};
				})
			};

			this.Padding = new Thickness (10, Device.OnPlatform (20, 0, 0), 10, 5);
			this.Content = new StackLayout {
				Children = {
					listView,
				}
			};
		}

		/**
		 * <summary>
		 * Sets the name of the leaderboard based on which board was selected.
		 * </summary>
		 * */
		private void SetBoardName(string whichBoard) {
			if (whichBoard.Equals ("OverallPoints")) {
				boardName = "Overall";
			} else if (whichBoard.Equals ("SalesPoints")) {
				boardName = "Sales";
			} else if (whichBoard.Equals ("TriviaPoints")) {
				boardName = "Trivia";
			} else if (whichBoard.Equals ("PeoplePoints")) {
				boardName = "People";
			}
		}
	}
}



