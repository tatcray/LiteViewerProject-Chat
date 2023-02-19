using Snitch.Api.User.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Snitch.Client.Options
{
    [Serializable]
    public class LocalConnectOptions : ISaveToFileAble, IReadFromFileAble, ICloneable
    {
        public LocalConnectOptions()
        {

        }
        public int WorkerLocalServerPort { get; set; }
        //public int WorkerLocalServicePort { get; set; }


        [JsonIgnore]
        public const string FileNameToSave = @"\LocalConnectOptions.json";

        [JsonIgnore]
        public string FileName { get; set; } = FileNameToSave; // Рептиль зачем?

        public object Clone()
        {
            return MemberwiseClone();
        }

        public static LocalConnectOptions GetDefault(int workerLocalServerPort)
        {
            LocalConnectOptions options = new LocalConnectOptions()
            {
                WorkerLocalServerPort = workerLocalServerPort
            };



            return options;
        }
    }
}
