using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BackendCRUD.Application.Model
{
    public class Member
    {
        public int Id { get; set; }

        public string name { get; set; }

        public int salary_per_year { get; set; }
        public string type { get; set; }

        public int? role { get; set; }

        public string? country { get; set; }

        public string? currencie_name { get; set; }

    }
}
