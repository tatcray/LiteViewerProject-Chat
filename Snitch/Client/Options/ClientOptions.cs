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
    public class ClientOptions : ISaveToFileAble, IReadFromFileAble, ICloneable
    {
        public ClientOptions()
        {

        }
        public bool NormVersion { get; set; }
        public bool NormAudioVersion { get; set; }

        public bool ShareBufferBool { get; set; }
        public bool ShareTextBufferBool { get; set; }
        public bool ShareAudioBool { get; set; }

        public bool ConnectPermission { get; set; }
        public string ConnectPass { get; set; }

        public bool NoAccConnectBool { get; set; }
        public string NoAccConnectPass { get; set; }

        public bool WhiteListBool { get; set; }
        public int[] WhiteList { get; set; }

        public bool GroupConnectBool { get; set; }

        [JsonIgnore]
        public const string FileNameToSave = @"\ClientOptions.json";

        [JsonIgnore]
        public string FileName { get; set; } = FileNameToSave; // Рептиль зачем?
        

        public object Clone()
        {
            return MemberwiseClone();
        }

        public static ClientOptions GetDefault(bool normVersion, bool normAudioVersion)
        {
            ClientOptions options = new ClientOptions()
            {
                NormVersion = normVersion,
                NormAudioVersion = normAudioVersion,
                ShareBufferBool = normVersion,
                ShareAudioBool = normAudioVersion,
                ShareTextBufferBool = true,
                ConnectPermission = true,
                NoAccConnectBool = false,
                WhiteListBool = false,
                GroupConnectBool = true
            };



            return options;
        }

        public static ClientOptions CheckDefault(ClientOptions Options)
        {
            // для новых настроек
            ClientOptions Default = GetDefault(Options.NormVersion, Options.NormAudioVersion);
            

            return Options;
        }

    }

    public class OptionsBool
    {
        public bool Value
        {
            get => Value; 
            set
            {
                Value = value;
                Changed = true;
            }
        }
        public bool Changed { get; set; }
    }

    public class OptionsInt
    {
        public int Value
        {
            get => Value;
            set
            {
                Value = value;
                Changed = true;
            }
        }
        public bool Changed { get; set; }
    }

    public class OptionsString
    {
        public string Value
        {
            get => Value;
            set
            {
                Value = value;
                Changed = true;
            }
        }
        public bool Changed { get; set; }
    }

    public class OptionsIntM
    {
        public int[] Value
        {
            get => Value;
            set
            {
                Value = value;
                Changed = true;
            }
        }
        public bool Changed { get; set; }
    }

}
