using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// ҡ�˿�����(GTAģʽ�ĵ����˳�-people+camera��ͬһ���ڵ�)
/// ҡ�˿�����(˫��ģʽ�ĵ����˳�-people��camera�ڲ�ͬ�ڵ㣬�����������׷��)
/// </summary>
public class JoystickController : MonoBehaviour {

    /// <summary>
    /// ������ڵ�
    /// </summary>
    public Transform m_Target;

    /// <summary>
    /// ҡ��
    /// </summary>
    private JoyStick js;

    /// <summary>
    /// ����ڵ�
    /// </summary>
    private Transform _peopleNode;

    /// <summary>
    /// ������ڵ�
    /// </summary>
    private Camera _cameraNode;

    /// <summary>
    /// �Ƿ���GTAģʽ�ĵ����˳�
    /// </summary>
    private bool _isGTAThird = true;

    /// <summary>
    /// ����������ٶ�
    /// </summary>
    private float _followSpeed = 5;

    /// <summary>
    /// ����ڵ��ƶ��ٶ�
    /// </summary>
    private float _moveSpeed = 10;

    /// <summary>
    /// ����ڵ���ת�ٶ�
    /// </summary>
    private float _rotationSpeed = 30 ;

    /// <summary>
    /// ������������
    /// </summary>
    private float _cameraDistance = 5;

    void Awake() {

    }

    public void OnEnable()
    {
        js = GameObject.FindObjectOfType<JoyStick>();
        js.OnJoyStickTouchBegin += On_JoystickMoveStart;
        js.OnJoyStickTouchMove += On_JoystickMove;
        js.OnJoyStickTouchEnd += On_JoystickMoveEnd;


        //�ϡ��¡����ҡ��ĸ���������ƻ���
        FingerGestures.OnFingerSwipe += OnFingerSwipe;
    }

    public void OnDisable()
    {
        js.OnJoyStickTouchBegin -= On_JoystickMoveStart;
        js.OnJoyStickTouchMove -= On_JoystickMove;
        js.OnJoyStickTouchEnd -= On_JoystickMoveEnd;


        //�ϡ��¡����ҡ��ĸ���������ƻ���
        FingerGestures.OnFingerSwipe -= OnFingerSwipe;
    }
		
	void OnDestroy()
    {
        js.OnJoyStickTouchBegin -= On_JoystickMoveStart;
        js.OnJoyStickTouchMove -= On_JoystickMove;
        js.OnJoyStickTouchEnd -= On_JoystickMoveEnd;
    }

    void On_JoystickMoveStart(Vector2 move)
    {
        Debug.Log("Start");
    }
	
	void On_JoystickMoveEnd(){
        Debug.Log("End");
    }

    /// <summary>
    /// �����������ƶ�״̬
    /// </summary>
    /// <param name="move"></param>
	void On_JoystickMove(Vector2 move)
    {
        Debug.Log(move);
    }

    private void OnFingerSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
    {

    }

}
