using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader
{
    /// <summary>
    /// ���ؽű��ӿ�
    /// </summary>
    /// <param name="loaderType">����ģʽ</param>
    //void LoadScript(LoaderType loaderType);

    /// <summary>
    /// ����ѭ������
    /// </summary>
    void FixedUpdate();

    /// <summary>
    /// ѭ������
    /// </summary>
    void Update();

    /// <summary>
    /// �ӳ�ѭ������
    /// </summary>
    void LateUpdate();

    /// <summary>
    /// ���ٺ���
    /// </summary>
    void Destroy();

    /// <summary>
    /// Android / iOS �ص�
    /// </summary>
    void Callback(string args);

    /// <summary>
    /// ʶ������ָ�� �ص�
    /// </summary>
    /// <param name="key"></param>
    void Speech(string key);

    /// <summary>
    /// ���ƻ�������
    /// </summary>
    /// <param name="fingerIndex">��ָ�±�</param>
    /// <param name="startPos">��ʼλ��</param>
    /// <param name="direction">����ö��</param>
    /// <param name="velocity">�ٶ�</param>
    void FingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity);

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="fingerIndex">��ָ�±�</param>
    /// <param name="fingerPos">��ָ���λ��</param>
    /// <param name="tapCount">�������</param>
    void FingerTap(int fingerIndex, Vector2 fingerPos, int tapCount);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fingerIndex">��ָ�±�</param>
    /// <param name="fingerPos">��ָ���λ��</param>
    void FingerLongPress(int fingerIndex, Vector2 fingerPos);


    /// <summary>
    /// ��ʼ��������ҡ��
    /// </summary>
    /// <param name="vec">����</param>
    void OnJoyStickBegin(Vector2 vec);

    /// <summary>
    /// �����ƶ�����ҡ��
    /// </summary>
    /// <param name="vec">����</param>
    void OnJoyStickMove(Vector2 vec);

    /// <summary>
    /// �����ƶ�ҡ�˽���
    /// </summary>
    void OnJoyStickEnd();
}