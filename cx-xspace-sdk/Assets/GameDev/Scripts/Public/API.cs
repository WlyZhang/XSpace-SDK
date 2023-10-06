//#define ANDROID_NATIVE
#pragma warning disable 0618
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Networking;


public static class API
{

    //1.���ļ�
    public static string ReadFileContent(string path)
    {
        TextAsset file = Resources.Load(path) as TextAsset;
        return file == null ? null : file.text;
    }

    //2.����vector3
    public static Vector3 CopyVector3(Vector3 ori)
    {
        Vector3 des = new Vector3(ori.x, ori.y, ori.z);
        return des;
    }

    //3.�ж������Ƿ���ͬ
    public static bool EqualVector3(Vector3 v1, Vector3 v2)
    {
        return Vector3.SqrMagnitude(v1 - v2) <= 0.0000001f;
    }

    //4.Sign
    public static float GetSign(Vector3 A, Vector3 B, Vector3 M)
    {
        return Mathf.Sign((B.x - A.x) * (M.y - A.y) - (B.y - A.y) * (M.x - A.x));
    }

    //5.Χ�����ĵ���ת��
    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    //6.ϴ��
    public static void Shuffle<T>(params T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var temp = array[i];
            var randomIndex = UnityEngine.Random.Range(0, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    //7.ת���ɵ�����
    public static string[] SeparateLines(string lines)
    {
        return lines.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n"[0]);
    }

    //8.�ݹ�ı�SortingLayer
    public static void ChangeSortingLayerRecursively(Transform root, string sortingLayerName, int offsetOrder = 0)
    {
        SpriteRenderer renderer = root.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder += offsetOrder;
        }

        foreach (Transform child in root)
        {
            ChangeSortingLayerRecursively(child, sortingLayerName, offsetOrder);
        }
    }

    //9.�ݹ�ı���Ⱦ����ɫ
    public static void ChangeRendererColorRecursively(Transform root, Color color)
    {
        SpriteRenderer renderer = root.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = color;
        }

        foreach (Transform child in root)
        {
            ChangeRendererColorRecursively(child, color);
        }
    }

    //10.�ݹ�ı�ͼƬ��ɫ
    public static void ChangeImageColorRecursively(Transform root, Color color)
    {
        Image image = root.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
        }

        foreach (Transform child in root)
        {
            ChangeImageColorRecursively(child, color);
        }
    }

    /// <summary>
    /// ��ʱ������ʱ��תʱ���
    /// </summary>
    /// <param name="datetime"></param>
    /// <returns>long=Int64</returns>
    public static long ToTimestamp(DateTime datetime)
    {
        DateTime dd = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        DateTime timeUTC = DateTime.SpecifyKind(datetime, DateTimeKind.Utc);//����ʱ��ת��UTCʱ��
        TimeSpan ts = (timeUTC - dd);
        return (Int64)(ts.TotalMilliseconds);//��ȷ������
    }
    /// <summary>
    /// ʱ���ת��ʱ������ʱ��
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static string TimestampToDateTime(long timeStamp)
    {
        DateTime dd = DateTime.SpecifyKind(new DateTime(1970, 1, 1, 0, 0, 0, 0), DateTimeKind.Local);
        long longTimeStamp = long.Parse(timeStamp.ToString() + "0000");
        TimeSpan ts = new TimeSpan(longTimeStamp);
        DateTime dateTime = dd.Add(ts);
        string dateString = dateTime.ToString("yyyy-MM-dd HH:mm:ss"); // ת��Ϊ�����ַ���

        return dateString;
    }

    //11.��ȡ��ǰ��

    public static double GetCurrentTime()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalSeconds;
    }

    //12.��ȡ��ǰ��
    public static double GetCurrentTimeInDays()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalDays;
    }

    //13.��ȡ��ǰ����ʱ��
    public static double GetCurrentTimeInMills()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalMilliseconds;
    }

    //14.�༭ģʽ��ȡSortingLayer
#if UNITY_EDITOR
    public static string[] GetSortingLayerNames()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }

    public static int[] GetSortingLayerUniqueIDs()
    {
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
        return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
    }
#endif

    //15.��ȡsprite

    public static Sprite GetSprite(string textureName, string spriteName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(textureName);
        foreach (Sprite sprite in sprites)
        {
            if (sprite.name == spriteName)
            {
                return sprite;
            }
        }
        return null;
    }

    //16.��ȡ��ʾ�е�������
    public static List<Transform> GetActiveChildren(Transform parent)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf) list.Add(child);
        }
        return list;
    }

    //17.��ȡ����������
    public static List<Transform> GetChildren(Transform parent)
    {
        List<Transform> list = new List<Transform>();
        foreach (Transform child in parent)
        {
            list.Add(child);
        }
        return list;
    }


    //18.�����������

    public static void CheckConnection(MonoBehaviour behaviour, Action<int> connectionListener)
    {
        behaviour.StartCoroutine(ConnectUrl("http://www.baidu.com", connectionListener));
    }

    private static IEnumerator ConnectUrl(string url, Action<int> connectionListener)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
            connectionListener(1);
        else if (string.IsNullOrEmpty(www.text))
            connectionListener(2);
        else
            connectionListener(0);
    }

    //19.�����������

    public static void CheckDisconnection(MonoBehaviour behaviour, Action onDisconnected)
    {
        behaviour.StartCoroutine(ConnectUrl("http://www.baidu.com", onDisconnected));
    }

    private static IEnumerator ConnectUrl(string url, Action onDisconnected)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www.error != null)
        {
            yield return new WaitForSeconds(2f);
            www = new WWW(url);
            yield return www;

            if (www.error != null)
                onDisconnected();
        }
    }

    public static IEnumerator LoadPicture(string url, string folder, string fileName, Action<Texture2D> callback, Vector2Int? textureSize = null, bool forceUpdate = false, bool cached = true)
    {
        string localPath = GetLocalPath(folder, fileName);
        bool loaded = false;
        if (!forceUpdate && cached) loaded = LoadFromLocal(callback, localPath);

        if (!loaded)
        {
            WWW www = new WWW(url);
            yield return www;
            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                if (textureSize == null || textureSize.Value.x == www.texture.width && textureSize.Value.y == www.texture.height)
                {
                    if (cached)
                    {
                        string folderPath = Path.Combine(Application.persistentDataPath, folder);
                        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                        File.WriteAllBytes(localPath, www.bytes);
                    }
                }

                var texture = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                www.LoadImageIntoTexture(texture);
                if (callback != null)
                {
                    callback(texture);
                }
            }
            else
            {
                //                if (callback != null) callback(null);
                //Toast.instance.ShowMessage("image is not ready,please try later!", 1.5f);
            }
        }
    }

    //20.��ȡ����·��
    public static string GetLocalPath(string folder, string fileName)
    {
        string pathFolder = Path.Combine(Application.persistentDataPath, folder);
        return Path.Combine(pathFolder, fileName);
    }

    //21.�ӱ����ļ���ȡ
    public static bool LoadFromLocal(Action<Texture2D> callback, string localPath)
    {
        if (File.Exists(localPath))
        {
            var bytes = File.ReadAllBytes(localPath);
            var tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            tex.LoadImage(bytes);
            if (tex != null)
            {
                if (callback != null) callback(tex);
                return true;
            }
        }
        return false;
    }

    //22.����Sprite
    public static Sprite CreateSprite(Texture2D texture, int width, int height)
    {
        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);
    }

    //Camera�и������Ƿ�ɼ�
    public static bool IsObjectSeenByCamera(Camera camera, GameObject gameObj, float delta = 0)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(gameObj.transform.position);
        return (screenPoint.z > 0 && screenPoint.x > -delta && screenPoint.x < 1 + delta && screenPoint.y > -delta && screenPoint.y < 1 + delta);
    }

    //23.����·��Ƿ�������
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    //24.����Ƿ��������Ϸ�
    public static bool IsPointerOverUIObject(GameObject uiObject)
    {
        if (uiObject == null) return false;
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (var go in results)
        {
            if (go.gameObject == uiObject) return true;
        }
        return false;
    }

    //25.����֮�����ĵ�
    public static Vector3 GetMiddlePoint(Vector3 begin, Vector3 end, float delta = 0)
    {
        Vector3 center = Vector3.Lerp(begin, end, 0.5f);
        Vector3 beginEnd = end - begin;
        Vector3 perpendicular = new Vector3(-beginEnd.y, beginEnd.x, 0).normalized;
        Vector3 middle = center + perpendicular * delta;
        return middle;
    }

    //26.AnimationClip
    public static AnimationClip GetAnimationClip(Animator anim, string name)
    {
        var ac = anim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == name) return ac.animationClips[i];
        }
        return null;
    }

    //27.����
    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp = lhs;
        lhs = rhs;
        rhs = temp;
    }

    //28.base64����

    public static string Base64Encode(string message)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(message);
        return Convert.ToBase64String(bytes);
    }

    //29. md5 32λ����

    public static string Md5Sum(string str)
    {
        Debug.LogError("���ַ�:" + str_new2(str));
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str_new2(str));
        byte[] hash = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("x2"));//��  "X2",С"x2"  
        }
        return sb.ToString();
    }

    /// 30. ���� ȥ���ַ����е�˫����

    public static string str_new(string str)
    {
        return System.Text.RegularExpressions.Regex.Replace(str, @"[\""]+", "");
    }

    //31.�滻"Ϊ\"

    public static string str_new2(string str)
    {
        return System.Text.RegularExpressions.Regex.Replace(str, "\"", "\\\"");
    }


    /// 32.ȥ��Json Key��˫����

    public static string JsonRegex(string jsonInput)
    {
        string result = string.Empty;
        try
        {
            string pattern = "\"(\\w+)\"(\\s*:\\s*)";
            string replacement = "$1$2";
            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(pattern);
            result = rgx.Replace(jsonInput, replacement);
        }
        catch (Exception ex)
        {
            result = jsonInput;
            Debug.LogError(ex.Message);
        }
        return result;
    }


    /// 33.Json ת��Ϊ��ά����

    public static string[,] JsonToArray(string json)
    {
        string s = json.Substring(1, json.Length - 2);
        string[] b = s.Split(',');

        for (int j = 0; j < b.Length; j++)
        {
        }
        string[,] bb = new string[1, 8];
        for (int i = 0; i < b.Length; i++)
        {
            bb[0, i] = b[i].ToString();
            //Debug.LogError("bb[0]["+i+"]="+b[i]);
        }
        return bb;
    }
}