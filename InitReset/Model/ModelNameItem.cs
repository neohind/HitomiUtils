using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InitReset.Model
{    
    public class ModelNameItem
    {
        [JsonPropertyName("anime")]        
        public bool Anime
        {
            get;
            set;
        }

        [JsonPropertyName("artist")]
        public string Artist
        {
            get;
            set;
        }

        [JsonPropertyName("dir")]
        public string Dir
        {
            get;
            set;
        }

        [JsonPropertyName("done")]
        public bool Done
        {
            get;
            set;
        }

        [JsonPropertyName("filesize")]        
        public double FileSize
        {
            get;
            set;
        }

        [JsonPropertyName("gal_num")]
        public string GalNum
        {
            get;
            set;
        }

        [JsonPropertyName("label_color")]
        public string LabelColor
        {
            get;
            set;
        }

        [JsonPropertyName("lazy")]
        public bool Lazy
        {
            get;
            set;
        }

        [JsonPropertyName("music")]
        public bool Music
        {
            get;
            set;
        }

        [JsonPropertyName("p_lazy")]
        public int PLazy
        {
            get;
            set;
        }

        [JsonPropertyName("pbar")]
        public object[] PBar
        {
            get;
            set;
        } = new object[3];
 
        [JsonPropertyName("time")]
        public double Time
        {
            get;
            set;
        }

        [JsonPropertyName("title")]
        public string Title
        {
            get;
            set;
        }

        [JsonPropertyName("type")]
        public string Type
        {
            get;
            set;
        }

        [JsonPropertyName("url")]   
        public string Url
        {
            get;
            set;
        }

        [JsonPropertyName("uid")]
        public string Uid
        {
            get;
            set;
        }

        [JsonPropertyName("valid")]
        public bool Valid
        {
            get;
            set;
        }

        [JsonPropertyName("ver")]
        public string Ver
        {
            get;
            set;
        }

        [JsonPropertyName("version")]
        public string Version
        {
            get;
            set;
        }

    }
}
