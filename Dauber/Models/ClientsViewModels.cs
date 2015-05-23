using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using MFP;

namespace Dauber.Models
{
    public class NewClientDiaryViewModel
    {
        public bool IsAvailable { get; set; }
        public bool IsPublic { get; set; }

        public NewClientDiaryViewModel() { }

        public NewClientDiaryViewModel(string clientUserName)
        {
            IsAvailable = !Client.AnyClientMatches(clientUserName);
            IsPublic = Scrape.IsPublic(clientUserName);
        }
    }

    public class ClientDiaryViewModel
    {
        public bool IsAvailable { get; set; }
        public bool IsPublic { get; set; }

        public ClientDiaryViewModel() { }

        public ClientDiaryViewModel(string clientUserName, string userId)
        {
            IsAvailable = !Client.AnyClientMatchesForOtherCoaches(clientUserName, userId);
            IsPublic = Scrape.IsPublic(clientUserName);
        }
    }
}