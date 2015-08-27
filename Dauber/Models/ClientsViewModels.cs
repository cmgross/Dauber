using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using DAL;
using MFP;

namespace Dauber.Models
{
    public class NewClientDiaryViewModel
    {
        public bool IsAdmin { get; set; }
        public bool IsPartner { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPublic { get; set; }
        public int MaxClientsForPlan { get; set; }
        public int CurrentClientsCount { get; set; }
        public bool IsValidFitocracyUser { get; set; }
        public int FitocracyUserId { get; set; }

        public NewClientDiaryViewModel() { }

        public NewClientDiaryViewModel(string clientUserName, string userId, string fitocracyUserName)
        {
            IsAvailable = !Client.AnyClientMatches(clientUserName);
            IsPublic = Scrape.IsPublic(clientUserName);
            var coach = Coach.GetCoachById(userId);
            MaxClientsForPlan = coach.Plan.MaxClients;
            CurrentClientsCount = coach.Clients.Count;
            IsAdmin = coach.Admin;
            IsPartner = coach.Partner;
            var dauberUserId = ConfigurationManager.AppSettings["FitocracyUserId"];
            var fitocracyUserId = Fitocracy.Scrape.GetUserId(fitocracyUserName);
            IsValidFitocracyUser = fitocracyUserName == string.Empty || fitocracyUserId != int.Parse(dauberUserId);
            FitocracyUserId = fitocracyUserId;
        }
    }

    public class ClientDiaryViewModel
    {
        public bool IsAvailable { get; set; }
        public bool IsPublic { get; set; }
        public bool IsValidFitocracyUser { get; set; }
        public int FitocracyUserId { get; set; }

        public ClientDiaryViewModel() { }

        public ClientDiaryViewModel(string clientUserName, string userId, string fitocracyUserName)
        {
            IsAvailable = !Client.AnyClientMatchesForOtherCoaches(clientUserName, userId);
            IsPublic = Scrape.IsPublic(clientUserName);
            var dauberUserId = ConfigurationManager.AppSettings["FitocracyUserId"];
            var fitocracyUserId = Fitocracy.Scrape.GetUserId(fitocracyUserName);
            IsValidFitocracyUser = fitocracyUserName == string.Empty || fitocracyUserId != int.Parse(dauberUserId);
            FitocracyUserId = fitocracyUserId;
        }
    }
}