using UnityEngine;
using System.Collections;
using Assets.Scripts.UI;

[AddComponentMenu("GAME/UIShowEffect/ScaleShow")]
public class ScaleShowEffect : MonoBehaviour , IEffect{
    // Use this for initialization
    void Start()
    {
        Reset();
        _ready = true;
    }

    public TweenScale GetTween()
    {
        var tween = this.GetComponent<TweenScale>();
        if (tween == null)
        {
            tween = this.gameObject.AddComponent<TweenScale>();
        }
        tween.enabled = true;
        tween.from = this.From;
        tween.to = this.OrScale.Value;
        tween.eventReceiver = this.gameObject;
        tween.callWhenFinished = "Completed";
        tween.duration = durtion;
        return tween;

    }

    public void Reset()
    {
        //Debug.Log("UIShowEffect Reset!");
        if (OrScale == null)
        {
            OrScale = this.gameObject.transform.localScale;
        }
        this.gameObject.transform.localScale = From;
    }

    [SerializeField]
    public Vector3 From = Vector3.one;

    [HideInInspector]
    private Vector3? OrScale;

    public bool ShowEffect()
    {
        if (!IsReady()) return false;
        //运行中未完成
        if (isRuning)
        {
            if (!isCompleted) return false;
            isRuning = false;
            return true;
        }
        else
        {
            isRuning = true;
            isCompleted = false;
            GetTween().Play(true);
            return false;
        }
    }

    public bool HideEffect()
    {
        if (!IsReady()) return false;
        //运行中未完成
        if (isRuning)
        {
            if (!isCompleted) return false;
            isRuning = false;
            return true;
        }
        else
        {
            isRuning = true;
            isCompleted = false;
            GetTween().Play(false);
            return false;
        }
    }
    [SerializeField]
    public float durtion = 0.5f;
    private bool isRuning = false;
    private bool isCompleted = false;
    private void Completed()
    {
        isCompleted = true;
    }

    private bool _ready = false;

    public bool IsReady()
    {
        return _ready;
    }
}
