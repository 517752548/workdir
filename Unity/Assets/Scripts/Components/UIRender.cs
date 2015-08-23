using UnityEngine;
using System.Collections;
using Assets.Scripts.Tools;
public class UIRender : MonoBehaviour, Assets.Scripts.UI.IUIRender, Assets.Scripts.UI.ITipRender
{

    // Use this for initialization
    void Start()
    {
        
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
                if (MessageLable != null)
                    MessageLable.text = string.Empty;
                delayTime = -1f;
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
    /// <param name="time">-1 always show</param>
    public void ShowMessage(string text,float time = 10f)
    {
        if (MessageLable == null) return;
        MessageLable.text = text;
        if (time > 0)
        {
            delayTime = Time.time + time;
        }
        else
        {
            delayTime = -1f;
        }
    }


    public void ShowOrHideMessage(bool show)
    {
        if (MessagePanel == null) return;
        MessagePanel.ActiveSelfObject(show);
    }
}
