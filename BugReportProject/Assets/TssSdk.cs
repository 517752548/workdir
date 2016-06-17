using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class TssSdk  
{

	[DllImport("__Internal")]
	public static extern void tss_sdk_setuserinfo(UserInfoStrStr info);

	[DllImport("__Internal")]
	public static extern void tss_sdk_setuserinfo_ex(UserInfoEx info);	
}

public class UserInfoStrStr
{
	public uint size;
	public uint entrance_id;
}

public class UserInfoEx
{
	public int size;
	public uint entrance_id;
	public uint uin_type;
}
