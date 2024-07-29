using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Infraestructure.Repository
{
    [Table("RoleType", Schema = "dbo")]
    public class RoleTypeEF
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
