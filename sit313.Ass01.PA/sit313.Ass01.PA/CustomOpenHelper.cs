using System;
using Android.Content;
using Android.Database.Sqlite;

namespace sit313.Ass01.PA
{
	public class CustomOpenHelper : SQLiteOpenHelper
	{
		//set query
		private static String DATABASE_CREATE = "CREATE TABLE outcome (_id INTEGER PRIMARY KEY AUTOINCREMENT, amount DOUBLE not null,comment text ," +
			"insertDate text not null,insertMonth text not null,insertYear text not null);";
		//set the default public function
		public CustomOpenHelper(Context context, string name, SQLiteDatabase.ICursorFactory factory, int version):base (context, name, factory, version)
		{
		}

		public override void OnCreate(SQLiteDatabase db)
		{
			db.ExecSQL(DATABASE_CREATE);
		}

		public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
		{
			db.ExecSQL("DROP TABLE IF EXISTS USER");
			OnCreate(db);
		}
	}
}

