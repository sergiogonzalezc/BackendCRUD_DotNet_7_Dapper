using BackendCRUD.Application.Common;
using BackendCRUD.Application.Model;

namespace BackendCRUD.Application.Interface
{
    public interface IMemberApplication
    {
        Task<bool> GetValidateMember(InputValidateMember input);

        public Task<ResultRequestDTO> InsertMember(InputCreateMember input);


        /// <summary>
        /// Update specific Member
        /// </summary>
        /// <returns></returns>
        Task<ResultRequestDTO> UpdateMember(InputUpdateMember input);


        /// <summary>
        /// Delete a member by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<ResultRequestDTO> DeleteMember(int Id);

        /// <summary>
        /// Get the full list of members
        /// </summary>
        /// <returns></returns>
        Task<List<MemberDTO>> GetMembers(int pageNumber, int pageSize);

        /// <summary>
        /// Validate if exists te same Member for the employee
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        Task<bool> ExistsMemberByName(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="MemberType"></param>
        /// <returns></returns>
        Task<MemberDTO> GetMembersByName(string name);

        /// <summary>
        /// Get the Member type List filter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MemberDTO> GetMembersById(int id);

        /// <summary>
        /// Get the full Member type List
        /// </summary>
        /// <returns></returns>
        Task<List<MemberType>> GetMemberTypes();

        /// <summary>
        /// Get the Member type List by Id
        /// </summary>
        /// <returns></returns>
        Task<MemberType> GetMemberTypeById(string id);


        /// <summary>
        /// Insert a un record of Role Type in BD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Task<ResultRequestDTO> InsertRoleType(InputCreateRoleType input);

        /// <summary>
        /// Get the full Role types List
        /// </summary>
        /// <returns></returns>
        Task<List<RoleType>> GetRoleTypes();


        /// <summary>
        /// Get the Member Type By Id
        /// </summary>
        /// <returns></returns>
        Task<RoleType> GetRoleTypeById(int id);


        /// <summary>
        /// Get the full Tags List
        /// </summary>
        /// <returns></returns>
        Task<List<Tag>> GetTags();

        /// <summary>
        /// Get the full list of member and tags
        /// </summary>
        /// <returns></returns>
        Task<List<MemberTagDTO>> GetMemberTags();

        /// <summary>
        /// Get the full list of tags for a specific member id
        /// </summary>
        /// <returns></returns>
        Task<List<MemberTag>> GetMemberTags(int member_id);

        /// <summary>
        /// Get list tags by label
        /// </summary>
        /// <returns></returns>
        Task<List<Tag>> GetTagsByLabel(string label);

        /// <summary>
        /// Get a specific tag by label
        /// </summary>
        /// <returns></returns>
        Task<Tag> GetTagByLabel(string label);


        /// <summary>
        /// Insert a new tag for a member
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResultRequestDTO> InsertMemberTag(InputCreateMemberTag input);

    }
}