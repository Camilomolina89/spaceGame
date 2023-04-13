using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
using System.Runtime.Serialization.Formatters.Binary;
// using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Text;
public class SaveGrid : MonoBehaviour
{
    /* public static T Load<T>(string filename) where T: class
    {
      string path = PathForFilename(filename);
      if(JSONSerializer.PathExists(path))
      {
        return JsonUtility.FromJson<T>(File.ReadAllText(path));
      }
      return default(T);
    } */
    public static void Save(string filename, PlanetData data)
    {
        Debug.Log("saving");
        FileStream file = new FileStream(PathForFilename("bin"), FileMode.OpenOrCreate);

        var d = JsonUtility.ToJson(data);
        byte[] bytes = Encoding.ASCII.GetBytes(d);

        file.SetLength(0);
        file.Write(bytes);

        file.Close();
        Debug.Log("close");
    
    }
    public static void SaveBin(string filename, PlanetData data)
    {
        FileStream file = new FileStream(PathForFilename("bin"), FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, data);
        file.Close();
    }
   
    public static PlanetData Load() {
        if (!File.Exists(PathForFilename("bin"))) {
            return null;
        }

        
        return  JsonUtility.FromJson<PlanetData>(File.ReadAllText(PathForFilename("bin")));
    }
    public static PlanetData LoadBin() {
        FileStream file = new FileStream(PathForFilename("bin"), FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        PlanetData data = (PlanetData) formatter.Deserialize(file);
        file.Close();
        return data;

    }

    static string PathForFilename(string filename)
    {
        return "Assets/ICO/" + filename + ".json";
    }
}
