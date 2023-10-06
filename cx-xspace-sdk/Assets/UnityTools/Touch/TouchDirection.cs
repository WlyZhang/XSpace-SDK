using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public class TouchDirection : MonoBehaviour
{
    enum slideVector { nullVector, up, down, left, right };
    private Vector2 touchFirst = Vector2.zero; //��ָ��ʼ���µ�λ��
    private Vector2 touchSecond = Vector2.zero; //��ָ�϶���λ��
    private slideVector currentVector = slideVector.nullVector;//��ǰ��������
    private float timer;//ʱ�������  
    public float offsetTime = 0.1f;//�жϵ�ʱ���� 
    public float SlidingDistance = 80f;
    void OnGUI()   // ��������02
    {
        if (Event.current.type == EventType.MouseDown)
        //�жϵ�ǰ��ָ�ǰ����¼� 
        {
            touchFirst = Event.current.mousePosition;//��¼��ʼ���µ�λ��
        }
        if (Event.current.type == EventType.MouseDrag)
        //�жϵ�ǰ��ָ���϶��¼�
        {
            touchSecond = Event.current.mousePosition;

            timer += Time.deltaTime;  //��ʱ��

            if (timer > offsetTime)
            {
                touchSecond = Event.current.mousePosition; //��¼�����µ�λ��
                Vector2 slideDirection = touchFirst - touchSecond;
                float x = slideDirection.x;
                float y = slideDirection.y;

                if (y + SlidingDistance < x && y > -x - SlidingDistance)
                {

                    if (currentVector == slideVector.left)
                    {
                        return;
                    }

                    Debug.Log("left");

                    currentVector = slideVector.left;
                }
                else if (y > x + SlidingDistance && y < -x - SlidingDistance)
                {
                    if (currentVector == slideVector.right)
                    {
                        return;
                    }

                    Debug.Log("right");

                    currentVector = slideVector.right;
                }
                else if (y > x + SlidingDistance && y - SlidingDistance > -x)
                {
                    if (currentVector == slideVector.up)
                    {
                        return;
                    }

                    Debug.Log("up");

                    currentVector = slideVector.up;
                }
                else if (y + SlidingDistance < x && y < -x - SlidingDistance)
                {
                    if (currentVector == slideVector.down)
                    {
                        return;
                    }

                    Debug.Log("Down");

                    currentVector = slideVector.down;
                }

                timer = 0;
                touchFirst = touchSecond;
            }
            if (Event.current.type == EventType.MouseUp)
            {//��������  
                currentVector = slideVector.nullVector;
            }
        }   // ��������
    }
}
