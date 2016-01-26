using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake()
	{
		_cur = this;
	}

	private static SoundManager _cur;

	public static SoundManager Singleton{ get { return _cur;} }


	public void PlaySound(string name)
	{
		if (!Assets.Scripts.DataManagers.GamePlayerManager.Singleton.IsMusicOn)
			return;
		var res = Assets.Scripts.Tools.ResourcesManager.Singleton.LoadResources<AudioClip> ("sounds/" + name);
		//res.length
		//Source.clip = res;
		//Source.Play ();
		NGUITools.PlaySound(res);
	}

	public void SetSourceValue(float value)
	{
		if (value <= 0.001f) {
			var listener = this.GetComponent<AudioListener> ();
			listener.enabled = false;
		} else {
			var listener = this.GetComponent<AudioListener> ();
			listener.enabled = true;
		}
		var sources = this.GetComponent<AudioSource> ();
		if (sources != null)
			sources.volume = value;
		
	}
		


	//public AudioSource Source;


}
