package md51e5a67ed1fcf13cff747b9ea3148811f;


public class MadMinute
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPause:()V:GetOnPauseHandler\n" +
			"n_onDestroy:()V:GetOnDestroyHandler\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("WritePadXamarinSample.MadMinute, WritePad SDK Xamarin Sample, Version=1.0.0.27, Culture=neutral, PublicKeyToken=null", MadMinute.class, __md_methods);
	}


	public MadMinute () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MadMinute.class)
			mono.android.TypeManager.Activate ("WritePadXamarinSample.MadMinute, WritePad SDK Xamarin Sample, Version=1.0.0.27, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();


	public void onDestroy ()
	{
		n_onDestroy ();
	}

	private native void n_onDestroy ();


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
