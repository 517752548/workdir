using UnityEngine;
using System.Collections;

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
    public Transform BackgroundTexutre;

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
}
