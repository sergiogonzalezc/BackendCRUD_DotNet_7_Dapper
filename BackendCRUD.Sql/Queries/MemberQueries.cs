using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Sql.Queries
{
    [ExcludeFromCodeCoverage]
    public static class MemberQueries
    {
        public static string AllMember => @"SELECT [ID], [name]
												  ,[salary_per_year]
												  ,[type]
												  ,[role]
												  ,[country]
												  ,[currencie_name]
												 FROM [Member] (NOLOCK)";

        public static string AllMemberDTO => @"SELECT 
												 m.[ID], 
												 m.[name]
												,m.[salary_per_year]
												,m.[type]
												,t.[description] as [type_description]
												,m.[role]
												,r.[description] as [role_description]
												,m.[country]
												,m.[currencie_name]
											FROM [Member] (NOLOCK) m
											inner join [MemberType] t (NOLOCK) on t.id = m.type
											left join [RoleType] r (NOLOCK) on r.id = m.role
											order by m.id asc 
											OFFSET @PageSize * (@PageNumber-1) ROWS FETCH NEXT @PageSize ROWS ONLY";

        public static string MemberByIdDTO => @"SELECT 
												 m.[ID], 
												 m.[name]
												,m.[salary_per_year]
												,m.[type]
												,t.[description] as [type_description]
												,m.[role]
												,r.[description] as [role_description]
												,m.[country]
												,m.[currencie_name]
											FROM [Member] (NOLOCK) m
											inner join [MemberType] t (NOLOCK) on t.id = m.type
											left join [RoleType] r (NOLOCK) on r.id = m.role
											where m.Id = @MemberId
											order by m.id asc";

        public static string MemberById => @" SELECT [ID], [name]
									  ,[salary_per_year]
									  ,[type]
									  ,[role]
									  ,[country]
									  ,[currencie_name]
										FROM [Member] (NOLOCK) WHERE Id = @Id";

        public static string AddMember =>
            @"INSERT INTO [Member] ([name]
					,[salary_per_year]
					,[type]
					,[role]
					,[country]
					,[currencie_name])
			VALUES (@name, @salary_per_year, @[type], @role, @country, @currencie_name)";

        public static string UpdateMember =>
            @"UPDATE [Member]
            SET [name] = @name, 
				[salary_per_year] = @salary_per_year, 
				[type] = @type, 
				[country] = @country,
				[currencie_name] = @currencie_name
            WHERE [Id] = @Id";

        public static string DeleteMember => "DELETE FROM [Member] WHERE [Id] = @Id";
    }
}