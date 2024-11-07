//数据文件处理
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    //文件路径
    private string dataDirPath = "";
    //数据名
    private string dataFileName = "";

    public FileDataHandler(string _dataDirPath, string _dataFileName)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
    }

    //保存文件
    public void Save(GameData _data)
    {
        //相对路径
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            //创建文件夹
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //文件流
            string dataToStore = JsonUtility.ToJson(_data, true);

            //创建一个文件
            using (FileStream stream = new FileStream(fullPath,FileMode.Create))
            {
                //讲文件流写入到文件
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("试图保存文件的时候出错！" + fullPath + "\n" + e);
            throw;
        }
    }
    
    //加载数据
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;

        //如果文件存在
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                //打开文件
                using (FileStream  stream =new FileStream(fullPath,FileMode.Open))
                {
                    //读取文件
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        //读取到结束
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return loadData;
    }

    //删除数据
    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
