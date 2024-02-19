using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Application.Model
{
    public class MemberTag
    {
        public int Member_id { get; set; }

        public int Tag_id { get; set; }
    }
}
