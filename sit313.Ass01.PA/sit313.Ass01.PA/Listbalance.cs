using System;
using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using Java.Text;
using Android.Database;

namespace sit313.Ass01.PA
{
	[Activity(Label = "Listbalance")]
	public class Listbalance : Activity
	{
		ImageButton btnBack;
		CalendarView calendarView;
		TextView lblAmount;
		DateTime date;
		string str;
		public double amount;
		public string srtDate;
		public string srtMonth;
		public string srtYear;
		CustomOpenHelper myOpenHelper;

		protected override void OnCreate(Bundle savedInstanceState)
		{

			base.OnCreate(savedInstanceState);

			//set widgets
			btnBack = FindViewById<ImageButton>(Resource.Id.btnBackList);
			calendarView = FindViewById<CalendarView>(Resource.Id.calendar);
			lblAmount = FindViewById<TextView>(Resource.Id.lblAmount);

			myOpenHelper = new CustomOpenHelper(this, "SQL.db", null, 1);
			//get the current system time:
			date = new System.DateTime();
			str = setDate(date);
			databaseReader();

			calendarView.SetOnDateChangeListener(calendarView.SetOnDateChangeListener, listener);


			btnBack.Click +=delegate {
				moveToMain();
			};
		}

		//format Date type to String
		public static string setDate(DateTime date)
		{
			return date.ToString("dd-MM-yyyy");
		}

		//parse String to Date
		public static DateTime Parse(string strDate)
		{
			return DateTime.ParseExact(strDate, "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
		}

		//Split String to date month year for query using:
		public void SplitDate(string str)
		{
			string[] a = str.Split('-');
			srtDate = a[0];
			srtMonth = a[1];
			srtYear = a[2];
		}
		//query database
		public void databaseReader()
		{
			//get the current time:
			str = setDate(date);
			SplitDate(str);
			amount = 0.0;

			SQLiteDatabase db = myOpenHelper.ReadableDatabase;
			//query the sql for all the data at specific date
			ICursor cusor = db.RawQuery("SELECT * FROM outcome WHERE insertDate = ? and insertMonth = ? and insertYear = ?", new string[] { srtDate, srtMonth, srtYear });
			//get all the select result:
			while (cusor.MoveToNext())
			{
				amount = amount + cusor.GetDouble(cusor.GetColumnIndex("amount"));    //get total spend for each day
			}
			lblAmount.Text = "$" + amount; //set the text for label
		}

		//navigation method
		public void moveToMain()
		{
			Intent intent = new Intent(this, typeof(MainActivity));
			intent.SetFlags(ActivityFlags.ClearTop);
			intent.PutExtra("nowDate", str);
			StartActivity(intent);
		}

		public void onDateSet(DatePicker view, int year, int month, int dayOfMonth)
		{
			//get date from calendar view
			str = dayOfMonth + "-" + (month + 1) + "-" + year;
			//format the string now to date type
			try
			{
				date = Parse(str);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}

