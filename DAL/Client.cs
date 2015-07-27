using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DAL
{
    [TableName("Clients")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class Client
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "MyFitnessPal UserName")]
        [StringLength(30)]
        public string ClientUserName { get; set; }
        [Required]
        [Display(Name = "Name")]
        [StringLength(50)]
        public string ClientName { get; set; }
        [Display(Name = "Fitocracy UserName")]
        [StringLength(15)]
        public string FitocracyUserName { get; set; }
        public int FitocracyId { get; set; }

        public Client() { }

        public Client(string coachId)
        {
            UserId = coachId;
        }

        #region InstanceMethods
        public void Save()
        {
            using (var db = new Database("DauberDB"))
            {
                db.Save(this);
            }
        }
        #endregion

        #region StaticMethods
        public static Client GetClient(string clientId)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("SELECT * FROM Clients WHERE Id='{0}'", clientId);
                return db.SingleOrDefault<Client>(query);
            }
        }

        public static void Delete(string clientId)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("DELETE FROM Clients WHERE Id='{0}'", clientId);
                db.Execute(query);
            }
        }
        public static bool AnyClientMatches(string clientUserName)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("FROM Clients WHERE ClientUserName='{0}'", clientUserName);
                var results = db.Fetch<Client>(query);
                return results.Any();
            }
        }

        public static bool AnyClientMatchesForOtherCoaches(string clientUserName, string userId)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("FROM Clients WHERE ClientUserName='{0}' and UserId <> '{1}' ", clientUserName, userId);
                var results = db.Fetch<Client>(query);
                return results.Any();
            }
        }
        #endregion
    }
}


