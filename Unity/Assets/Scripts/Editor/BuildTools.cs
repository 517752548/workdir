using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEditor.iOS.Xcode;


public class BuildTools
{
	public BuildTools ()
	{
	}

	[PostProcessBuildAttribute(999)]
	public static void OnPostBuild(BuildTarget targetF,string path)
	{
		if (targetF != BuildTarget.iOS) {
			return;
		}

		var proPath = PBXProject.GetPBXProjectPath (path);
		var obj = new UnityEditor.iOS.Xcode.PBXProject ();
		obj.ReadFromFile (proPath);
		string target = obj.TargetGuidByName("Unity-iPhone");
		obj.AddFrameworkToProject (target, "StoreKit.framework", false);
		obj.WriteToFile (proPath);
	}

	[MenuItem("GAME/INCREASE_VERSION")]
	public static void IncreaseVerson()
	{

		if (EditorUtility.DisplayDialog ("Increase Version", 
			string.Format("Decide to increase version? Current is {0}",PlayerSettings.bundleVersion),
			"Yes", "Cancle"))
		{

			var buildVersion = PlayerSettings.bundleVersion;
			var versions = buildVersion.Split ('.');

			int major = int.Parse (versions [0]);
			int build = int.Parse (versions [1]);
			var develop = int.Parse (versions [2]);

			develop += 1;
			if (develop >= 10) {
				build += 1;
				develop = 0;
				if (build >= 10) {
					major += 1;
					build = 0;
				}
			}

			PlayerSettings.bundleVersion = string.Format ("{0}.{1}.{2}", major, build, develop);
			//PlayerSettings. = PlayerSettings.bundleVersion;
			EditorUtility.DisplayDialog ("Version Increased", 
				string.Format ("Version From {0} To {1}", buildVersion, PlayerSettings.bundleVersion),
				"Close");
		}
	}

}


