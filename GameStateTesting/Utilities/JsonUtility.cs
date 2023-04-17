using GameStateTesting.Story;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GameStateTesting.Utilities
{
    public static class JsonUtility
    {
        public static List<Message> GetJsonStringMessageFromJSON(string jsonFileLocation)
        {
            string jsonMessage = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, jsonFileLocation));
            //If the return is empty it needs to throw an exception 
            if (jsonMessage == "")
            {
                throw new Exception("This file is empty");
            }
            return JsonConvert.DeserializeObject<List<Message>>(jsonMessage);
            //return JsonSerializer.DeserializeObject<List<Message>>(jsonMessage);
        }
    }
}
