package md51e5a67ed1fcf13cff747b9ea3148811f;


public class LeaderboardActivity
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
		mono.android.Runtime.register ("WritePadXamarinSample.LeaderboardActivity, WritePad SDK Xamarin Sample, Version=1.0.0.27, Culture=neutral, PublicKeyToken=null", LeaderboardActivity.class, __md_methods);
	}


	public LeaderboardActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == LeaderboardActivity.class)
			mono.android.TypeManager.Activate ("WritePadXamarinSample.LeaderboardActivity, WritePad SDK Xamarin Sample, Version=1.0.0.27, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
