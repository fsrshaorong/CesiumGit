using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ʱ��
/// <para>ZhangYu 2018-04-08</para>
/// </summary>
public class Timer : MonoBehaviour
{

    // �ӳ�ʱ��(��)
    public float delay = 0;
    // ���ʱ��(��)
    public float interval = 1;
    // �ظ�����
    public int repeatCount = 1;
    // �Զ���ʱ
    public bool autoStart = false;
    // �Զ�����
    public bool autoDestory = false;
    // ��ǰʱ��
    public float currentTime = 0;
    // ��ǰ����
    public int currentCount = 0;
    // ��ʱ���
    public UnityEvent onIntervalEvent;
    // ��ʱ���
    public UnityEvent onCompleteEvent;
    // �ص��¼�����
    public delegate void TimerCallback(Timer timer);
    // ��һ�μ��ʱ��
    private float lastTime = 0;
    // ��ʱ���
    private TimerCallback onIntervalCall;
    // ��ʱ����
    private TimerCallback onCompleteCall;

    private void Start()
    {
        enabled = autoStart;
    }

    private void FixedUpdate()
    {
        if (!enabled) return;
        addInterval(Time.deltaTime);
    }

    /// <summary> ���Ӽ��ʱ�� </summary>
    private void addInterval(float deltaTime)
    {
        currentTime += deltaTime;
        if (currentTime < delay) return;
        if (currentTime - lastTime >= interval)
        {
            currentCount++;
            lastTime = currentTime;
            if (repeatCount <= 0)
            {
                // �����ظ�
                if (currentCount == int.MaxValue) reset();
                if (onIntervalCall != null) onIntervalCall(this);
                if (onIntervalEvent != null) onIntervalEvent.Invoke();
            }
            else
            {
                if (currentCount < repeatCount)
                {
                    //��ʱ���
                    if (onIntervalCall != null) onIntervalCall(this);
                    if (onIntervalEvent != null) onIntervalEvent.Invoke();
                }
                else
                {
                    //��ʱ����
                    stop();
                    if (onCompleteCall != null) onCompleteCall(this);
                    if (onCompleteEvent != null) onCompleteEvent.Invoke();
                    if (autoDestory && !enabled) Destroy(this);
                }
            }
        }
    }

    /// <summary> ��ʼ/������ʱ </summary>
    public void start()
    {
        enabled = autoStart = true;
    }

    /// <summary> ��ʼ��ʱ </summary>
    /// <param name="time">ʱ��(��)</param>
    /// <param name="onComplete(Timer timer)">��ʱ��ɻص��¼�</param>
    public void start(float time, TimerCallback onComplete)
    {
        start(time, 1, null, onComplete);
    }

    /// <summary> ��ʼ��ʱ </summary>
    /// <param name="interval">��ʱ���</param>
    /// <param name="repeatCount">�ظ�����</param>
    /// <param name="onComplete(Timer timer)">��ʱ��ɻص��¼�</param>
    public void start(float interval, int repeatCount, TimerCallback onComplete)
    {
        start(interval, repeatCount, null, onComplete);
    }

    /// <summary> ��ʼ��ʱ </summary>
    /// <param name="interval">��ʱ���</param>
    /// <param name="repeatCount">�ظ�����</param>
    /// <param name="onInterval(Timer timer)">��ʱ����ص��¼�</param>
    /// <param name="onComplete(Timer timer)">��ʱ��ɻص��¼�</param>
    public void start(float interval, int repeatCount, TimerCallback onInterval, TimerCallback onComplete)
    {
        this.interval = interval;
        this.repeatCount = repeatCount;
        onIntervalCall = onInterval;
        onCompleteCall = onComplete;
        reset();
        enabled = autoStart = true;
    }

    /// <summary> ��ͣ��ʱ </summary>
    public void stop()
    {
        enabled = autoStart = false;
    }

    /// <summary> ֹͣTimer���������� </summary>
    public void reset()
    {
        lastTime = currentTime = currentCount = 0;
    }

    /// <summary> �������ݲ����¿�ʼ��ʱ </summary>
    public void restart()
    {
        reset();
        start();
    }

}
