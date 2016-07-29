using System;
using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Widget;

namespace sit313.Ass01.PA
{
	[Activity(Label = "Insert")]
	public class Insert : Activity
	{

		ImageButton btnBack;
		Button btnDone;
		EditText cash;
		EditText comment;
		TextView lblDate;
		string now;
		public string srtDate;
		public string srtMonth;
		public string srtYear;
		Double amount;
		string commentStr;
		CustomOpenHelper myOpenHelper;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_insert);



			now = Intent.GetStringExtra("nowDate");
			
			//set widgets
			btnBack = FindViewById<ImageButton>(Resource.Id.btnBack);
			btnDone = FindViewById<Button>(Resource.Id.btnDone);
			cash = FindViewById<EditText>(Resource.Id.cash);
			comment = FindViewById<EditText>(Resource.Id.comment);
			lblDate = FindViewById<TextView>(Resource.Id.lblDateInsert);

			//set database
			myOpenHelper = new CustomOpenHelper(this, "SQL.db", null, 1);
			//set on click listener
			btnBack.Click += delegate
			{
				moveToMain();
			};

			btnDone.Click += delegate
			{
				insert();
			};

			lblDate.Text = now;

		}

		//Split String to date month year for query using:
		public void SplitDate(String str)
		{
			String[] a = str.Split('-');
			srtDate = a[0];
			srtMonth = a[1];
			srtYear = a[2];
		}

		//insert data into database
		public void insert()
		{
			SplitDate(now);
			//get data from edit text
			amount = double.Parse(cash.Text);
			commentStr = comment.Text;
			if (commentStr.Equals("") || commentStr.Equals(null))
				commentStr = "Unnamed";
			//set database
			SQLiteDatabase db = myOpenHelper.WritableDatabase;
			//set content for each value:
			ContentValues values = new ContentValues();
			values.Put("amount", amount);
			values.Put("comment", commentStr);
			values.Put("insertDate", srtDate);
			values.Put("insertMonth", srtMonth);
			values.Put("insertYear", srtYear);
			//insert all the value into database
			db.Insert("outcome", null, values);
			moveToMain();
		}

		//navigation method
		public void moveToMain()
		{
			Intent intent = new Intent(this, typeof(MainActivity));
			intent.SetFlags(ActivityFlags.ClearTop);
			intent.PutExtra("nowDate", now);
			StartActivity(intent);
		}
	}
}