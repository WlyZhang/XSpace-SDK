using UnityEngine;

public class Game : MonoBehaviour
{
    /// <summary>
    /// ��Ҫʹ�����е� Module ģ�飬�л���Ҫ���Ĵ��ֶ�
    /// </summary>
    public static string ModuleName = "Game";


    /// <summary>
    /// ��˾���������
    /// </summary>
    public static string CompanyName => Application.companyName;



    /// <summary>
    /// ������ģʽSDK��ʶ
    /// </summary>
    public static string DevelopApp => Application.productName;
}
