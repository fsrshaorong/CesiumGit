using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SandMaterialContrl : MonoBehaviour
{
    MeshRenderer sandMaterial;
    Vector2 offset = new(0, 0);
    public float feedAmount;
    public float feedDuration;
    public float feedInterval;
    public float feedSpeed;
    public Transform pursher;

    // Start is called before the first frame update
    void Start()
    {
        sandMaterial = GetComponent<MeshRenderer>();
        StartCoroutine(nameof(IName));
    }

    // Update is called once per frame
    void Update()
    {
        feedDuration = feedAmount / feedSpeed;
    }

    
    void Offset()
    {
        Vector2 offset = sandMaterial.material.GetVector("_UVOffset");
        float yoffset = sandMaterial.material.GetVector("_UVOffset").y;
        DOTween.To(() => yoffset, x => { sandMaterial.material.SetVector("_UVOffset", new Vector2(0, x)); }, yoffset + feedAmount, feedDuration);
    }

    void Push()
    {
        pursher.DOMoveX(-20f, feedDuration);
    }

    void WithdrawPusher()
    {
        pursher.DOMoveX(-5f, feedInterval);
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(IName)); // 关闭协程
    }

    public IEnumerator IName()
    {
        while (true)
        {
            Offset();
            Push();
            //WithdrawPusher();
            yield return new WaitForSeconds(feedDuration);//延时1秒再继续向下执行
            WithdrawPusher();
            yield return new WaitForSeconds(feedInterval);//延时1秒再继续向下执行
        }
    }
}
