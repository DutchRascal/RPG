using System.IO;
using System.Text;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);

            FileStream stream = File.Open(path, FileMode.Create);
            // stream.WriteByte(0xc2);
            // stream.WriteByte(0xa1);
            // stream.WriteByte(0x48);
            // stream.WriteByte(0x6f);
            // stream.WriteByte(0x6c);
            // stream.WriteByte(0x61);
            // stream.WriteByte(0x20);
            // stream.WriteByte(0x4d);
            // stream.WriteByte(0x75);
            // stream.WriteByte(0x6e);
            // stream.WriteByte(0x64);
            // stream.WriteByte(0x6f);
            // stream.WriteByte(0x21);
            byte[] bytes = Encoding.UTF8.GetBytes("¡Hola Mundo! Bytes");
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
        }

        public void Load(string saveFile)
        {
            saveFile = Path.Combine(Application.persistentDataPath, saveFile);
            print("Loading from " + GetPathFromSaveFile(saveFile));
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}