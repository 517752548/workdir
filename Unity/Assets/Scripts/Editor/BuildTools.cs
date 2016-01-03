using System;
using UnityEditor;
using UnityEditor.Callbacks;

public class BuildTools
{
	public BuildTools ()
	{
	}

	[PostProcessBuildAttribute(999)]
	public static void OnPostBuild(BuildTarget target)
	{
		if (target != BuildTarget.iOS) {
			return;
		}

	
	}

}


