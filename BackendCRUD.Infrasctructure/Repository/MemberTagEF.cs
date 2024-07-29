using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Infraestructure.Repository
{
    [Table("MemberTag", Schema = "dbo")]
    public class MemberTagEF
    {
        [Key]        
        [Column("member_id")]
        public int Member_id { get; set; }

        [Column("tag_id")]
        public int Tag_id { get; set; }
    }
}
