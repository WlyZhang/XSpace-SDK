using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

public class BuildAssets
{
    /// <summary>
    /// �ļ������б�
    /// </summary>

    /// <summary>
    /// Resources �ļ����е� ��.lua.txt���ļ����б�
    /// </summary>
    private static List<string> resList = new List<string>();

    /// <summary>
    /// ��Ҫ���Lua�ű��б�
    /// </summary>
    private static List<Object> fileList = new List<Object>();

    /// <summary>
    /// ����ģʽ�ű�·��
    /// </summary>
    private static string developPath;

    private static List<string> fileNameList = new List<string>();

    /// <summary>
    /// ���ڰ汾����Ϣ
    /// </summary>
    private static List<Object> abFileList = new List<Object>();

    /// <summary>
    /// �汾��Ϣ�ֵ�
    /// </summary>
    private static Dictionary<string, object> versionDic = new Dictionary<string, object>();

    /// <summary>
    /// �汾��Ϣ
    /// </summary>
    private static string versionInfo;

    /// <summary>
    /// �汾��Ϣ����·��
    /// </summary>
    private static string versionPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles/files.txt";

    /// <summary>
    /// Win���԰汾·��
    /// </summary>
    private static string winBuildPath = $"{Application.dataPath}/../Debug/{Game.DevelopApp}_Data/StreamingAssets";

    /// <summary>
    /// �ȸ���ģʽ�ű�·��
    /// </summary>
    private static string hotUpdatePath;

    /// <summary>
    /// ���Bundle·��
    /// </summary>
    private static string buildPath;

    /// <summary>
    /// ���Bundleƽ̨
    /// </summary>
    private static BuildTarget buildTarget;

    /// <summary>
    /// ��ǰ����ƽ̨
    /// </summary>
    private static string platform = string.Empty;



    //================================= ���������� ================================================




    [MenuItem("HotUpdate/Builder/Windows Platform")]
    public static void BuildWindows()
    {
        platform = "Windows";
        buildTarget = BuildTarget.StandaloneWindows64;
        BuildToAssetBundle();
    }

    [MenuItem("HotUpdate/Builder/Mac Platform")]
    public static void BuildMac()
    {
        platform = "Mac";
        buildTarget = BuildTarget.StandaloneOSX;
        BuildToAssetBundle();
    }

    [MenuItem("HotUpdate/Builder/iOS Platform")]
    public static void BuildIOS()
    {
        platform = "iOS";
        buildTarget = BuildTarget.iOS;
        BuildToAssetBundle();
    }

    [MenuItem("HotUpdate/Builder/Android Platform")]
    public static void BuildAndroid()
    {
        platform = "Android";
        buildTarget = BuildTarget.Android;
        BuildToAssetBundle();
    }

    [MenuItem("HotUpdate/Clean Bundle")]
    public static void DeleteCache()
    {
        PlayerPrefs.DeleteAll();

        ClearList();

        string cachePath = $"{Application.persistentDataPath}/{Game.ModuleName}/{platform}/";
        string luaPath = $"{Application.dataPath}/LuaProject/{Game.ModuleName}/{platform}/";
        string streamingPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/";

        ClearCache(cachePath);
        ClearCache(luaPath);
        ClearCache(streamingPath);


        //winBuild Ŀ¼����
        ClearCache($"{winBuildPath}/{Game.ModuleName}");

        AssetDatabase.Refresh();
    }

    [MenuItem("HotUpdate/Clean Bundle All")]
    public static void DeleteCacheAll()
    {
        PlayerPrefs.DeleteAll();

        ClearList();

        string cachePath = $"{Application.persistentDataPath}/";
        string luaPath = $"{Application.dataPath}/LuaProject/";
        string streamingPath = $"{Application.streamingAssetsPath}/";

        ClearCache(cachePath);
        ClearCache(luaPath);
        ClearCache(streamingPath);


        //winBuild Ŀ¼����
        ClearCache($"{winBuildPath}/");

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// �����б�
    /// </summary>
    public static void ClearList()
    {
        resList.Clear();
        fileList.Clear();
        fileNameList.Clear();
        abFileList.Clear();
        versionDic.Clear();
    }

    //[MenuItem("Tools/�ȸ���/������Դ��StreamingAssets�ļ���")]
    public static void CopyToStreamingAssets()
    {
        string lua_ab = "lua";
        string sourcePath = $"{Application.dataPath}/../AssetBundles/StandaloneWindows64/src/";
        string copyPath = $"{Application.streamingAssetsPath}/src/";

        ClearCache(copyPath);

        if (!Directory.Exists(copyPath))
        {
            Directory.CreateDirectory(copyPath);
        }

        if (File.Exists($"{sourcePath}{lua_ab}"))
        {
            File.Copy($"{sourcePath}{lua_ab}", $"{copyPath}{lua_ab}", true);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError($"��{sourcePath}{lua_ab}��·��û��Դ�ļ�");
        }

        Debug.Log("<color=yellow>�ѿ�����Դ��StreamingAssets�ļ���!</color>");
    }


    /// <summary>
    /// ���Lua�ű���AssetBundle
    /// </summary>
    private static async void BuildToAssetBundle()
    {
        ClearList();

        versionInfo = string.Empty;

        AssetLoadManager.Create();

        string luaPath = $"{Application.dataPath}/LuaProject/{Game.ModuleName}/{platform}/src/Resources/";

        ClearCache(luaPath);

        string streamPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/";

        ClearCache(streamPath);

        string winPath = $"{winBuildPath}/{Game.ModuleName}/{platform}/";

        ClearCache(winPath);

        await CopyLuaScriptsToFolder();

        Debug.Log("<color=yellow>������Lua�ű����!</color>");

        //����BundleName
        await ReNameBundle("src/lua");

        //ˢ�±༭��
        AssetDatabase.Refresh();

        await GetAssetAll($"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles/");

        //���ɰ汾��Ϣ
        versionPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles/files.txt";
        CreateTextFile(versionPath, versionInfo.Trim());

        //����StreamingAssets��Դ��Win���԰汾·��
        CopyFolder(streamPath, winPath);

        //ˢ�±༭��
        AssetDatabase.Refresh();

        Debug.Log("<color=green>�ȸ�����Դ������!</color>");





















        //���ⲿGameX���Գ���
        System.Diagnostics.Process.Start($"{Application.dataPath}/../Debug/{Game.DevelopApp}.exe");

    }

    private static async Task GetAssetAll(string dirPath)
    {
        string[] dirList = Directory.GetDirectories(dirPath);

        if (dirList.Length > 0)
        {
            //�������ļ���
            for (int i = 0; i < dirList.Length; i++)
            {
                //��ȡ��һ���ļ���
                await GetAssetAll(dirList[i]);
            }

            //������ļ����ļ�
            await GetAsset(dirPath);
        }
        else
        {
            //��ӱ��ļ����ļ�
            await GetAsset(dirPath);
        }

    }

    /// <summary>
    /// ����ļ��нű��ļ�
    /// </summary>
    /// <param name="dirPath"></param>
    private static async Task GetAsset(string dirPath)
    {
        string[] dirlist = Directory.GetFiles(dirPath);
        if (dirlist.Length > 0)
        {
            for (int i = 0; i < dirlist.Length; i++)
            {
                dirlist[i] = dirlist[i].Replace("\\", "/");

                //�����ļ�
                if(dirlist[i].Contains(".meta")|| dirlist[i].Contains(".manifest"))
                {
                    continue;
                }



                Debug.Log(dirlist[i]);


                string tempPath = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles/";
                string name = dirlist[i].Replace(tempPath, "");
                byte[] byteArr = await AssetLoadManager.Instance.LoadAssetAsyncWithByte(dirlist[i]);

                string md5 = CreateMD5(byteArr);
                AddSingleVersion(name, md5);
            }

            versionInfo = JsonConvert.SerializeObject(versionDic);
        }
    }

    /// <summary>
    /// ����Lua�ű�������ļ���
    /// </summary>
    private static async Task CopyLuaScriptsToFolder()
    {
        ClearList();

        developPath = $"{Application.dataPath}/../LuaProject/{Game.ModuleName}/src/";
        hotUpdatePath = $"{Application.dataPath}/LuaProject/{Game.ModuleName}/{platform}/src/Resources/";
        buildPath = $"Assets/LuaProject/{Game.ModuleName}/{platform}/src/Resources/";

        if (!Directory.Exists(hotUpdatePath))
        {
            Directory.CreateDirectory(hotUpdatePath);
        }

        if (!string.IsNullOrEmpty(developPath))
        {
            await GetScriptAll(developPath);
        }

        //����Lua�ű�
        await CopyLuaScripts();

        //ˢ�±༭��
        AssetDatabase.Refresh();

    }

    /// <summary>
    /// ��ȡ�ļ���ȫ���ű��ļ�
    /// </summary>
    /// <param name="dirPath"></param>
    private static async Task GetScriptAll(string dirPath)
    {
        string[] dirList = Directory.GetDirectories(dirPath);

        if (dirList.Length > 0)
        {
            //�������ļ���
            for (int i = 0; i < dirList.Length; i++)
            {
                //��ȡ��һ���ļ���
                await GetScriptAll(dirList[i]);
            }

            //������ļ����ļ�
            AddScript(dirPath);
        }
        else
        {
            //��ӱ��ļ����ļ�
            AddScript(dirPath);
        }
    }

    /// <summary>
    /// ����ļ��нű��ļ�
    /// </summary>
    /// <param name="dirPath"></param>
    private static void AddScript(string dirPath)
    {
        string[] dirlist = Directory.GetFiles(dirPath);
        if (dirlist.Length > 0)
        {
            for (int i = 0; i < dirlist.Length; i++)
            {
                dirlist[i] = dirlist[i].Replace("\\", "/");

                fileNameList.Add(dirlist[i]);
            }
        }
    }

    /// <summary>
    /// ����Lua�ű�������ļ���
    /// </summary>
    private static async Task CopyLuaScripts()
    {
        if (fileNameList.Count > 0)
        {
            await Task.Yield();

            for (int i = 0; i < fileNameList.Count; i++)
            {
                string luaPath = fileNameList[i].Replace(developPath, hotUpdatePath);

                string[] strArr = luaPath.Split('/');
                string[] splitArr = strArr[strArr.Length - 1].Split('.');
                string key = splitArr[0];

                string dirPath = luaPath.Replace($"{key}.lua", "");
                if(!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string fileName = $"{luaPath}.txt";
                File.Copy(fileNameList[i], fileName, true);//���������ֱ���Դ�ļ�·�����洢·�������洢·������ͬ�ļ��Ƿ��滻

                string tempPath = $"{Application.dataPath}/";
                string midPath = luaPath.Replace(tempPath, "Assets/");

                Debug.Log(midPath);

                resList.Add($"{midPath}.txt");
            }
        }
    }

    /// <summary>
    /// ����Bundle����
    /// </summary>
    /// <param name="dir"></param>
    private static async Task ReNameBundle(string dir)
    {
        await Task.Yield();

        for (int i = 0; i<resList.Count; i++)
        {
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(resList[i]);

            fileList.Add(obj);

            AssetImporter imp = AssetImporter.GetAtPath(resList[i]);

            imp.assetBundleName = dir.Trim();

            imp.SaveAndReimport();
        }

        AssetDatabase.Refresh();


        //���Bundle
        BuildBundles();
    }

    /// <summary>
    /// ���Bundle
    /// </summary>
    private static void BuildBundles()
    {
        string assetBundlesLibrary = $"{Application.streamingAssetsPath}/{Game.ModuleName}/{platform}/AssetBundles";

        if(buildTarget == BuildTarget.Android)
        {
            //assetBundlesLibrary += "/Android";
            if (!Directory.Exists(assetBundlesLibrary))//�жϱ���Ŀ¼�Ƿ��Ѿ�����Ŀ¼��
            {
                Directory.CreateDirectory(assetBundlesLibrary);
            }

            BuildPipeline.BuildAssetBundles(assetBundlesLibrary, BuildAssetBundleOptions.None,
                BuildTarget.Android);//�ڴ�Ŀ¼�´���AssetBundle
        }
        else if(buildTarget == BuildTarget.iOS)
        {
            //assetBundlesLibrary += "/iOS";
            if (!Directory.Exists(assetBundlesLibrary))//�жϱ���Ŀ¼�Ƿ��Ѿ�����Ŀ¼��
            {
                Directory.CreateDirectory(assetBundlesLibrary);
            }

            BuildPipeline.BuildAssetBundles(assetBundlesLibrary, BuildAssetBundleOptions.None,
                BuildTarget.iOS);//�ڴ�Ŀ¼�´���AssetBundle
        }
        else if(buildTarget == BuildTarget.StandaloneWindows64)
        {
            //assetBundlesLibrary += "/Windows/";

            if (!Directory.Exists(assetBundlesLibrary))//�жϱ���Ŀ¼�Ƿ��Ѿ�����Ŀ¼��
            {
                Directory.CreateDirectory(assetBundlesLibrary);
            }

            BuildPipeline.BuildAssetBundles(assetBundlesLibrary, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows64);//�ڴ�Ŀ¼�´���AssetBundle
        }
        else if(buildTarget == BuildTarget.StandaloneOSX)
        {
            //assetBundlesLibrary += "/Mac/";
            ClearCache($"{assetBundlesLibrary}src/");

            if (!Directory.Exists(assetBundlesLibrary))//�жϱ���Ŀ¼�Ƿ��Ѿ�����Ŀ¼��
            {
                Directory.CreateDirectory(assetBundlesLibrary);
            }

            BuildPipeline.BuildAssetBundles(assetBundlesLibrary, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneOSX);//�ڴ�Ŀ¼�´���AssetBundle
        }

        buildTarget = BuildTarget.NoTarget;
        //������Դ��StreamingAssets�ļ���
        //CopyToStreamingAssets();
    }

    /// <summary>
    /// ������
    /// </summary>
    private static void ClearCache(string cachePath)
    {
        if (Directory.Exists(cachePath))
        {
            Directory.Delete(cachePath, true);
            Debug.Log($"<color=yellow>{cachePath} ����������</color>");
        }
        else
        {
            Debug.Log($"{cachePath} �޻���Ŀ¼");
        }
    }

    /// <summary>
    /// �����ļ��м��ļ�
    /// </summary>
    /// <param name="sourceFolder">ԭ�ļ�·��</param>
    /// <param name="destFolder">Ŀ���ļ�·��</param>
    /// <returns></returns>
    public static int CopyFolder(string sourceFolder, string destFolder)
    {
        try
        {
            //���Ŀ��·��������,�򴴽�Ŀ��·��
            if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //�õ�ԭ�ļ���Ŀ¼�µ������ļ�
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                System.IO.File.Copy(file, dest);//�����ļ�
            }
            //�õ�ԭ�ļ���Ŀ¼�µ������ļ���
            string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string dest = System.IO.Path.Combine(destFolder, name);
                CopyFolder(folder, dest);//����Ŀ��·��,�ݹ鸴���ļ�
            }
            return 1;
        }
        catch
        {
            return -1;
        }
    }

    /// <summary>
    /// ��ӵ����汾��Ϣ
    /// </summary>
    /// <param name="name"></param>
    /// <param name="md5"></param>
    private static void AddSingleVersion(string name,string md5)
    {
        if(!versionDic.ContainsKey(name))
        {
            versionDic.Add(name, md5);
        }
    }

    /// <summary>
    /// ����md5
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    private static string CreateMD5(byte[] buffer)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] md5Bytes = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
            {
                sb.Append(md5Bytes[i].ToString("x2"));//X2ʱ��������ĸ��дMD5
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// �����汾��Ϣ
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <param name="info"></param>
    private static void CreateTextFile(string path, string info)
    {
#if !UNITY_WEBPLAYER
        //�ļ�����Ϣ  
        StreamWriter sw;
        FileInfo t = new FileInfo(path);
        t.Delete();
        sw = t.CreateText();
        //���е���ʽд����Ϣ  
        sw.WriteLine(info);
        //�ر���  
        sw.Close();
        //������  
        sw.Dispose();
#endif
    }
}