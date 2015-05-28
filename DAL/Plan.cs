using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace DAL
{
    [TableName("Plans")]
    [PrimaryKey("Id", autoIncrement = false)]
    public class Plan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MaxClients { get; set; }
        public int Cost { get; set; }

        //public static int GetPlanMax(string planId)
        //{
        //    using (var db = new Database("DauberDB"))
        //    {
        //        var query = String.Format("SELECT MaxClients FROM Plans WHERE Id='{0}'", planId);
        //        return db.ExecuteScalar<int>(query);
        //    }
        //}

        public static Plan Get(string planId)
        {
            using (var db = new Database("DauberDB"))
            {
                var query = String.Format("SELECT * FROM Plans WHERE Id='{0}'", planId);
                return db.SingleOrDefault<Plan>(query);
            }
        }
    }
}
