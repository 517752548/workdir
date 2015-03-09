using UnityEngine;
using System.Collections;

public class UIRender : MonoBehaviour, Assets.Scripts.UI.IUIRender
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        Assets.Scripts.UI.UIManager.Singleton.Init(this);
    }

    [SerializeField]
    public Camera UICamera;
    [SerializeField]
    public Transform UIRoot;

    public void Render(GameObject uiRoot)
    {
        uiRoot.transform.parent = UIRoot;
        uiRoot.transform.localRotation = Quaternion.identity;
        uiRoot.transform.localScale = Vector3.one;
        uiRoot.transform.localPosition = Vector3.zero;
    }
}
