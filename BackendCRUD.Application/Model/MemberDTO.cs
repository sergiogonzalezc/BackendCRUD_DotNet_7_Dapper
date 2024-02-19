using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BackendCRUD.Application.Model
{
    public class MemberDTO
    {
        public int Id { get; set; }

        public string name { get; set; }

        public int salary_per_year { get; set; }
        public string type { get; set; }

        public string type_description { get; set; }

        public int? role { get; set; }

        public string? role_description { get; set; }

        public string? country { get; set; }

        public string? currencie_name { get; set; }

        public List<TagDTO> tag_list { get; set; }
    }
}
