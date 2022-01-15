﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using MusicStreaming.Core;
//
//    var newsData = NewsData.FromJson(jsonString);

namespace News.Core
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class NewsData
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("news")]
        public News[] News { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }
    }

    public partial class News
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("image")]
        public Uri Image { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("category")]
        public string[] Category { get; set; }

        [JsonProperty("published")]
        public string Published { get; set; }
    }

    public partial class NewsData
    {
        public static NewsData FromJson(string json) => JsonConvert.DeserializeObject<NewsData>(json, MusicStreaming.Core.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this NewsData self) => JsonConvert.SerializeObject(self, MusicStreaming.Core.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
