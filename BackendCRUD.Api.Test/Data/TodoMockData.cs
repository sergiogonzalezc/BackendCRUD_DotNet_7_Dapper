using BackendCRUD.Api.Model;
using BackendCRUD.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.Api.UnitTest.Data
{
    public class TodoMockData
    {
        public static List<MemberModel> GetTodo()
        {
            return new List<MemberModel>
            {
                new MemberModel
                {
                    Code = "1",
                    DataList = new List<MemberDTO>
                    {
                        new MemberDTO
                        {
                            Id = 1,
                            name = "1",
                            salary_per_year = 100000,
                            type = "C"
                        }
                    }
                }
            };
        }
    }
}
