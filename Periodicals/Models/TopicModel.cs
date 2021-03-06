﻿using Periodicals.Core.Entities;
using System.Collections.Generic;

namespace Periodicals.Models
{
    public class TopicModel
    {
        public int Id { get; private set; }
        
        public string TopicName { get; private set; }

        public static TopicModel FromTopic(Topic item) => new TopicModel()
        {
            Id = item.Id,
            TopicName = item.TopicName
        };

        public Topic ToTopic() => new Topic()
        {
            Id = this.Id,
            TopicName = this.TopicName ?? "No topic",
        };

        public static void SetTopicsList(List<Topic> topics)
        {
            List<TopicModel> topicsModel = new List<TopicModel>();
            foreach (var item in topics)
            {
                topicsModel.Add(new TopicModel()
                {
                    Id = item.Id,
                    TopicName = item.TopicName
                });
            }
            Topics = topicsModel;
        }

        public static List<TopicModel> Topics { get; private set; }
    }

}