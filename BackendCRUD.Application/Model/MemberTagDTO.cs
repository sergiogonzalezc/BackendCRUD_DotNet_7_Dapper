using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Model
{
    public class MemberTagDTO
    {
        public int Member_id { get; set; }

        public int Tag_id { get; set; }

        public string Tag_label { get; set; }

        public string member_name{ get; set; }
    }
}
