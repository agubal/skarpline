using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Skarpline.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void DeserializerTest()
        {
            //Arrange
            const string inputString =
                "{'has_title':true,'title':'GoodLuck','entries':" +
                "[['/gettingstarted.pdf',{'thumb_exists':false,'path':'GettingStarted.pdf','client_mtime':'Wed, 08 Jan 2014 18:00:54+ 0000','bytes':249159}]," +
                "['/task.jpg',{'thumb_exists':true,'path':'/Task.jpg','client_mtime':'Tue, 14 Jan 2014 05:53:57+ 0000','bytes':207696}]]}";

            //Act
            Folder folder = JsonConvert.DeserializeObject<Folder>(inputString, new FileJsonConverter());

            //Assert
            Assert.IsNotNull(folder);
            Assert.IsTrue(folder.HasTitle);
            Assert.AreEqual(folder.Title, "GoodLuck");
            CollectionAssert.AllItemsAreNotNull(folder.Files);
            CollectionAssert.AllItemsAreUnique(folder.Files);

            Assert.IsTrue(folder.Files.ContainsKey("gettingstarted.pdf"));
            Assert.IsFalse(folder.Files["gettingstarted.pdf"].ThumbExists);
            Assert.IsNotNull(folder.Files["gettingstarted.pdf"].Path);
            Assert.IsTrue(folder.Files["gettingstarted.pdf"].ClientMTime.Date == new DateTime(2014, 1, 8));
            Assert.IsTrue(folder.Files["gettingstarted.pdf"].Bytes > default(int));

            Assert.IsTrue(folder.Files.ContainsKey("task.jpg"));
            Assert.IsTrue(folder.Files["task.jpg"].ThumbExists);
            Assert.IsNotNull(folder.Files["task.jpg"].Path);
            Assert.IsTrue(folder.Files["task.jpg"].ClientMTime.Date == new DateTime(2014, 1, 14));
            Assert.IsTrue(folder.Files["task.jpg"].Bytes > default(int));
        }

    }

    /// <summary>
    /// Book object
    /// </summary>
    public class Folder
    {
        [JsonProperty("has_title")]
        public bool HasTitle { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("entries")]
        public Dictionary<string, File> Files { get; set; }
    }

    /// <summary>
    /// File object
    /// </summary>
    public class File
    {
        [JsonProperty("thumb_exists")]
        public bool ThumbExists { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("client_mtime")]
        public DateTime ClientMTime { get; set; }

        [JsonProperty("bytes")]
        public int Bytes { get; set; }
    }

    /// <summary>
    /// Custom converter for Dictionary with images
    /// </summary>
    public class FileJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var imageArray = JArray.Load(reader);
            var dictionary = new Dictionary<string, File>();
            foreach (var imageJson in imageArray.Children())
            {
                if (imageJson == null) continue;
                object[] valueParts = JsonConvert.DeserializeObject<object[]>(imageJson.ToString());
                if (valueParts == null || valueParts.Length != 2) continue;
                string imageName = valueParts[0].ToString();
                imageName = imageName.Replace("/", string.Empty);
                File image = JsonConvert.DeserializeObject<File>(valueParts[1].ToString(), new DateTimeJsonConverter());
                dictionary.Add(imageName, image);
            }
            return dictionary;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, File>);
        }
    }

    /// <summary>
    /// Custom converter for Date
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader?.Value == null) return new DateTime();
            DateTime parseDateTime;
            //Tue, 14 Jan 2014 05:53:57+ 0000
            return DateTime.TryParseExact(reader.Value.ToString(), "ddd, dd MMM yyyy HH:mm:ss+ 0000", new CultureInfo("en-US"),
                DateTimeStyles.None, out parseDateTime) ? parseDateTime : new DateTime();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
}
