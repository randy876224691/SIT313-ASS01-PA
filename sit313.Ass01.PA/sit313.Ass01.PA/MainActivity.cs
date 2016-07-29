using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Database;
using Java.Text;
using Java.Util;
using Android.Views;
using Android.Systems;
using System;
using Android.Database.Sqlite;
using System.Collections.Generic;


namespace sit313.Ass01.PA
{
	[Activity(Label = "sit313-Ass01-PA", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, View.IOnClickListener
	{
		ImageButton btnList;
		ImageButton btnLeft;
		ImageButton btnRight;
		ImageButton btnInsert;
		ImageButton btnDetail;
		TextView lblAmount;
		TextView dateText;
		public DateTime date;
		public string now;
		public string srtDate;
		public string srtMonth;
		public string srtYear;
		public double amount;
		CustomOpenHelper myOpenHelper;

		//This two value is for the store all the data from sql query
		List<double> value;
		List<string> item;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_main);

			//built the SQL:
			myOpenHelper = new CustomOpenHelper(this, "SQL.db", null, 1);
			//get the current date from other activity
			now = Intent.GetStringExtra("nowDate");

			//format the string now to date type
			try
			{
				date = Parse(now);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			//if no extra from other it will using the system date
			if (now == null)
			{
				date = DateTime.Now;  //using system date
				now = setDate(date);
			}


			// Get our button from the layout resource,
			// and attach an event to it

			btnList = FindViewById<ImageButton>(Resource.Id.btnList);
			btnLeft = FindViewById<ImageButton>(Resource.Id.btnLeft);
			btnRight = FindViewById<ImageButton>(Resource.Id.btnRight);
			btnDetail = FindViewById<ImageButton>(Resource.Id.btnDetails);
			btnInsert = FindViewById<ImageButton>(Resource.Id.btnInsert);
			dateText = FindViewById<TextView>(Resource.Id.lblDate);
			lblAmount = FindViewById<TextView>(Resource.Id.lblAmount);

			//query:
			databaseReader();
			//button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };

			btnList.SetOnClickListener(this);
			btnLeft.SetOnClickListener(this);
			btnRight.SetOnClickListener(this);
			btnInsert.SetOnClickListener(this);
			btnDetail.SetOnClickListener(this);
			dateText.SetOnClickListener(this);

			dateText.Text = now;




		}

		//parsing the String to Date

		public static DateTime Parse(string strDate)
		{
			return DateTime.ParseExact(strDate, "dd-MM-yyyy", System.Globalization.CultureInfo.CurrentCulture);
		}



		//formatting the Date to string
		public static string setDate(DateTime date)
		{
			return date.ToString("dd-MM-yyyy");
		}

		//Split String to date month year for query using:
		public void SplitDate(string str)
		{
			Console.WriteLine(str);
			string[] a = str.Split('-');
			srtDate = a[0];
			srtMonth = a[1];
			srtYear = a[2];
		}

		//SQL query
		public void databaseReader()
		{
			SplitDate(now);
			//set all the data to null
			value = new List<double>();
			item = new List<string>();
			amount = 0.0;
			SQLiteDatabase db = myOpenHelper.ReadableDatabase;
			//query the sql for all the data at specific date
			ICursor cusor = db.RawQuery("SELECT * FROM outcome WHERE insertDate = ? and insertMonth = ? and insertYear = ?",new string[] { srtDate, srtMonth, srtYear });
			//get all the select result:
			while (cusor.MoveToNext())
			{
				//add result col into list
				value.Add(cusor.GetDouble(cusor.GetColumnIndex("amount")));
				item.Add(cusor.GetString(cusor.GetColumnIndex("comment")));
				//get total for each day
				amount = amount + cusor.GetDouble(cusor.GetColumnIndex("amount"));
			}
			lblAmount.Text = "$" + amount.ToString(); //set the text for label
		}

		public void moveTolist()
		{
			Intent intent = new Intent(this, typeof(Listbalance));
			intent.SetFlags(ActivityFlags.ClearTop);
			StartActivity(intent);
		}

		public void moveToInsert()
		{
			Intent intent = new Intent(this, typeof(Insert));
			intent.SetFlags(ActivityFlags.ClearTop);
			intent.PutExtra("nowDate", now);
			StartActivity(intent);
		}


		public void showDialog()
		{
			Dialog dialog = new Dialog(this, Android.Resource.Style.ThemeLight);
			dialog.SetCancelable(true);     //could be canceled
			dialog.SetContentView(Resource.Layout.detail);     //set the view layout for this dialog
														//set the widgets


			ImageButton cancel = dialog.FindViewById<ImageButton>(Resource.Id.btnCancel);
			ListView lv = dialog.FindViewById<ListView>(Resource.Id.lstDetail);
			//using the custom list view layout

			lv.Adapter = new detailListAdapter(this, value, item);

			cancel.Click +=delegate {
				dialog.Dismiss();
			};

			dialog.Show();
		}

		class detailListAdapter: BaseAdapter
		{
			//set the items in custom list
			List<double> title;
			List<string> detail;
			Activity activity;

			//set the default constructor
			detailListAdapter()
			{
					title = null;
					detail = null;
			}

			//set the default constructor
			public detailListAdapter(Activity activity, List<double> text1, List<string> text2): base()
			{
				title = text1;
				detail = text2;
				this.activity = activity;
			}

			public override int Count
			{
				get
				{
					//return Convert.ToInt32(title);
					return title.Count;
				}
			}

			public override Java.Lang.Object GetItem(int position)
			{
				// TODO Auto-generated method stub
				return null;
			}

			public override long GetItemId(int position)
			{
				// TODO Auto-generated method stub
				return position;
			}

			public override View GetView(int position, View convertView, ViewGroup parent)
			{

				var row = convertView ?? activity.LayoutInflater.Inflate(
		Resource.Layout.detail_adapter, parent, false);
				TextView ctitle, cdetail;
				ctitle = row.FindViewById<TextView>(Resource.Id.value);
				cdetail = row.FindViewById<TextView>(Resource.Id.item);
				ctitle.Text = "$" + title[position].ToString();
				cdetail.Text = detail[position].ToString();
				return row;
			}
		}


		public void OnClick(View v)
		{
			int buttonID = v.Id;
			switch (buttonID)
			{
				case Resource.Id.btnLeft:
					//date change when clicked
					date = date.AddDays(1);
					now = setDate(date);    //get string date
					//set string date for textView
					dateText.Text = now;
					databaseReader();   //query this new current date
					break;
				case Resource.Id.btnRight:
					//date change when clicked
					date = date.AddDays(-1);
					now = setDate(date);    //get string date
					//set string date for textView
					dateText.Text = now;
					databaseReader();   //query this new current date
					break;
				case Resource.Id.lblDate:
					break;

					/*
				case Resource.Id.btnList:
					moveTolist();
					break;
					*/

				case Resource.Id.btnInsert:
					moveToInsert();
					break;
				case Resource.Id.btnDetails:
					showDialog();
					break;
				default:
					break;
			}
		}
	}
}



