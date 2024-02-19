using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Sql.Queries
{
    [ExcludeFromCodeCoverage]
    public static class TagQueries
    {
        public static string AllTag => @"SELECT [id],[label] FROM [Tag] (NOLOCK)";
        
        public static string TagById => @"SELECT [id],[label] FROM [Tag] (NOLOCK) WHERE [Id] = @Id";

        public static string AllTagByMember => @"SELECT m.[id], m.[label] FROM [MemberTag] mt (NOLOCK) 
                                                inner join [Tag] m (NOLOCK) on m.id = mt.tag_id
                                                where mt.member_id = @MemberId";

    }
}