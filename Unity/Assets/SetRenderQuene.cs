using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SetRenderQuene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
		panelOfRoot= NGUITools.FindInParents<UIPanel> (this.gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lastQuene != panelOfRoot.startingRenderQueue) {
			lastQuene = panelOfRoot.sortingOrder;
			render.sortingOrder = lastQuene +1;
		}
	}

	private SpriteRenderer render;

	private int lastQuene;

	public UIPanel panelOfRoot;

    
}
