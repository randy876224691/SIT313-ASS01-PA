package md523ae9eb38c3f20d6e8579f4f90eb9584;


public class Insert
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("sit313.Ass01.PA.Insert, sit313.Ass01.PA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Insert.class, __md_methods);
	}


	public Insert () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Insert.class)
			mono.android.TypeManager.Activate ("sit313.Ass01.PA.Insert, sit313.Ass01.PA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
