using UnityEngine;
using System.Collections;
using Assets.Scripts.Tools;
using Assets.Scripts.UI;
public class UIRender :
    MonoBehaviour,
    IUIRender, 
    ITipRender
{

    // Use this for initialization
    void Start()
    {
        //when start
		tweenPosition.Play(false);
		isHide = true;
    }

    // Update is called once per frame
    void Update()
    {
        Assets.Scripts.UI.UITipManager.Singleton.OnUpdate();
        Assets.Scripts.UI.UIManager.Singleton.OnUpdate();
    }

    void LateUpdate()
    {

        Assets.Scripts.UI.UITipManager.Singleton.OnLateUpdate();
        Assets.Scripts.UI.UIManager.Singleton.OnLateUpdate();

        if (delayTime > 0)
        {
            if (Time.time > delayTime)
            {
				delayTime = -1;
				isHide = true;
				this.tweenPosition.Play (false);
            }
        }
    }

    void Awake()
    {
        Assets.Scripts.UI.UIManager.Singleton.Init(this);
        Assets.Scripts.UI.UITipManager.Singleton.Init(this);
        DontDestroyOnLoad(this.gameObject);
    }

    [SerializeField]
    public Camera UICamera;
    [SerializeField]
    public Transform UIRoot;
    [SerializeField]
    public UILabel MessageLable;


    [SerializeField]
    public Transform MessagePanel;
	public UIPanel MessageViewPanle;

    [SerializeField]
    public Transform UITipPanel;
    public void Render(GameObject uiRoot)
    {
        uiRoot.transform.parent = UIRoot;
        uiRoot.transform.localRotation = Quaternion.identity;
        uiRoot.transform.localScale = Vector3.one;
        uiRoot.transform.localPosition = Vector3.zero;
    }

    public void RenderTip(GameObject tip)
    {
        //Render(tip);
        tip.transform.parent = UITipPanel;
        tip.transform.localRotation = Quaternion.identity;
        tip.transform.localScale = Vector3.one;
        tip.transform.localPosition = Vector3.zero;
    }


    private float delayTime = 0f;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    public void ShowMessage(string text)
    {
        if (MessageLable == null) return;
        MessageLable.text = text;
		NGUITools.AddWidgetCollider (MessageLable.gameObject);
    }


    public void ShowOrHideMessage(bool show)
    {
        if (MessagePanel == null) return;
        MessagePanel.ActiveSelfObject(show);
    }

	public TweenPosition tweenPosition;
	private bool isHide =true;
	public UILabel lableInfo;

	public void ShowInfo(string message,float delay)
	{
		lableInfo.text = message;
		delayTime = Time.time + delay;
		if (isHide) {
			isHide = false;
			tweenPosition.Play (true);
		}
	}
}
