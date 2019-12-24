using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenChatBot.CognitiveModels
{
    public class StockLUIS
    {
        public string query { get; set; }
        public topScoringIntent[] topScoringIntent { get; set; }
        public Intent[] intents { get; set; }
        public Entity[] entities { get; set; }


    }
    public class topScoringIntent {
        public string intent { get; set; }
        public float score { get; set; }
    }
    public class Prediction
    {
        public string topIntent { get; set; }
        public Intent intents { get; set; }
        //public Entity[] entities { get; set; }
    }
        public class Intent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }
    public class score {

      public string intent { get; set; }
        public float value { get; set; }
    }
    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public float score { get; set; }
    }
    public class topScoringIntent1
    {
        public string intent { get; set; }
        public string score { get; set; }
    }

    public class entities
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public string score { get; set; }
    }
    public class LuisModel
    {
        public LuisModel()
        {
            topScoringIntent = new topScoringIntent1();
            entities = new List<entities>();
        }
        public string query { get; set; }
        public topScoringIntent1 topScoringIntent { get; set; }
        public List<entities> entities { get; set; }


    }
}
