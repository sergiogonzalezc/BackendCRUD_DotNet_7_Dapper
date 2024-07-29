using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCRUD.Infraestructure.Repository
{
    [Table("Member", Schema ="dbo")]
    public class MemberEF
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("salary_per_year")]
        public int salary_per_year { get; set; }

        [Column("type")]
        public string type { get; set; }

        [Column("role")]
        public int? role { get; set; }

        [Column("country")]
        public string? country { get; set; }

        [Column("currencie_name")]
        public string? currencie_name { get; set; }
    }
}
