using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RakTDAApi.Models.Payloads
{
    public class Login
    {
        [Required]
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class SiteVisit
    {
        public string date { get; set; }
        public int unitId { get; set; }
        public string time { get; set; }
    }


    public class LoadCriteria
    {
       public int UnitId { get; set; }
        public int UCId { get; set; }
    }


    public class SiteCompletion
    {
        public int UnitId { get; set; }
        public int UCId { get; set; }

        public string Comments { get; set; }
    }

    public class CheckListResults
    {
        public int UnitId { get; set; }
        public int PropertyTypeId { get; set; }
        public int ClassificationTypeId { get; set; } = 0;
        public string IndexCode { get; set; }
        public int Result { get; set; }
        public string MasterId { get; set; }
        public int UserId { get; set; }
        public int UcId { get; set; }
        public string Remarks { get; set; }
        public string Image { get; set; }
    }
}
