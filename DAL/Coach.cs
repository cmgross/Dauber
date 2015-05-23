using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace DAL
{
    [TableName("AspNetUsers")]
    [PrimaryKey("Id", autoIncrement = false)]
    public class Coach
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
        public bool Admin { get; set; }
        public int CoachId { get; set; }

        [Ignore]
        public List<Client> Clients { get; set; }

        public Coach()
        {
            Clients = new List<Client>();
        }

        public static bool IsAdmin(string userName)
        {
            userName = UserNameHelper(userName);
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("SELECT Admin FROM AspNetUsers WHERE UserName='{0}'", userName);
                return db.SingleOrDefault<bool>(query);
            }
        }

        public static List<Coach> GetAllCoaches()
        {
            using (var db = new Database("DauberDB"))
            {
                return db.Fetch<Coach>("SELECT * FROM AspNetUsers");
            }
        }

        public static Coach Get(string userName)
        {
            userName = UserNameHelper(userName);
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("SELECT * FROM AspNetUsers WHERE UserName='{0}'", userName);
                var coach = db.SingleOrDefault<Coach>(query);
                var clientsQuery = String.Format("SELECT * FROM Clients WHERE UserId='{0}'", coach.Id);
                coach.Clients = db.Fetch<Client>(clientsQuery);
                return coach;
            }
        }

        public static Coach Get(int coachId)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("SELECT * FROM AspNetUsers WHERE CoachId={0}", coachId);
                var coach = db.SingleOrDefault<Coach>(query);
                var clientsQuery = String.Format("SELECT * FROM Clients WHERE UserId='{0}'", coach.Id);
                coach.Clients = db.Fetch<Client>(clientsQuery);
                return coach;
            }
        }

        public static Coach GetCoachById(string guid)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("SELECT * FROM AspNetUsers WHERE Id='{0}'", guid);
                var coach = db.SingleOrDefault<Coach>(query);
                var clientsQuery = String.Format("SELECT * FROM Clients WHERE UserId='{0}'", coach.Id);
                coach.Clients = db.Fetch<Client>(clientsQuery);
                return coach;
            }
        }

//        public static Coach GetCoachAndClients(string userName)
//        {
//            using (var db = new Database("DauberDB"))
//            {
//                return db.Query<Coach, Client, Coach>(
//                    new CoachToClientRelator().MapIt,
//                    @"SELECT AspNetUsers.*, Clients.*
//                        FROM Clients 
//                        INNER JOIN AspNetUsers ON Clients.UserId = AspNetUsers.Id 
//                        WHERE AspNetUsers.UserName=@0", userName).FirstOrDefault();
//            }
//        }

        public static void ChangeActive(string id, bool activeStatus)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("UPDATE AspNetUsers SET Active = '{0}' WHERE Id='{1}'", activeStatus, id);
                db.Execute(query);
            }
        }

        public static void Delete(string id)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("DELETE FROM AspNetUsers WHERE Id='{0}'", id);
                db.Execute(query);
            }
        }

        private static string UserNameHelper(string userName)
        {
            int index = userName.IndexOf("@");
            userName = userName.Insert(index + "@".Length, "@");
            return userName;
        }
    }

    public class CoachToClientRelator
    {
        public Coach Current;

        public Coach MapIt(Coach a, Client p)
        {
            if (a == null)
                return Current;

            if (a != null && Current != null && a.Id == Current.Id)
            {
                Current.Clients.Add(p);
                return null;
            }

            var prev = Current;
            Current = a;
            Current.Clients = new List<Client>() { p };

            return prev;
        }
    }
}
