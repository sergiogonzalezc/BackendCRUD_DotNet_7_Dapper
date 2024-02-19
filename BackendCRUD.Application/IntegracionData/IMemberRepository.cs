using BackendCRUD.Application.Model;

namespace BackendCRUD.Application.Interface
{
    public interface IMemberRepository
    {
        public Task<bool> InsertMember(Member input);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<bool> UpdateMember(Member input);

        /// <summary>
        /// Delete Member by Id.
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteMember(int Id);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<List<MemberDTO>> GetMembers();

        /// <summary>
        /// Validate if exists te same Member for the employee
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<bool> ExistsMemberByName(string name);


        /// <summary>
        /// Validate if exists a Member by Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<bool> ExistsMemberById(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<MemberDTO> GetMembersByName(string name);

        /// <summary>
        /// Get the Member type List filter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<MemberDTO> GetMemberById(int id);


        /// <summary>
        /// Get the full Member type List
        /// </summary>
        /// <returns></returns>
        public Task<List<MemberType>> GetMemberTypes();

        /// <summary>
        /// Get the Member Type By Id
        /// </summary>
        /// <returns></returns>
        public Task<MemberType> GetMemberTypeById(string id);


        /// <summary>
        /// Insert a RoleType record
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<bool> InsertRoleType(RoleType input);

        /// <summary>
        /// Get the full Role types List
        /// </summary>
        /// <returns></returns>
        public Task<List<RoleType>> GetRoleTypes();


        /// <summary>
        /// Get the Member Type By Id
        /// </summary>
        /// <returns></returns>
        public Task<RoleType> GetRoleTypeById(int id);


        /// <summary>
        /// Get the full Tags List
        /// </summary>
        /// <returns></returns>
        public Task<List<Tag>> GetTags();

        /// <summary>
        /// Get the full list of member and tags
        /// </summary>
        /// <returns></returns>
        public Task<List<MemberTagDTO>> GetMemberTags();

        /// <summary>
        /// Get the full list of tags for a specific member id
        /// </summary>
        /// <returns></returns>
        public Task<List<MemberTag>> GetMemberTags(int member_id);

        /// <summary>
        /// Get the list if tag by label
        /// </summary>
        /// <returns></returns>
        public Task<List<Tag>> GetTagsByLabel(string label);


        /// <summary>
        /// Get a specific tag by label
        /// </summary>
        /// <returns></returns>
        public Task<Tag> GetTagByLabel(string label);


        /// <summary>
        /// Insert a new Tag
        /// </summary>
        /// <returns></returns>
        public Task<Tag> InsertTag(Tag input);


        /// <summary>
        /// Insert a member tag record
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<bool> InsertMemberTag(MemberTag input);


        /// <summary>
        /// Return TRUE if the member id has the tag id asociated
        /// </summary>
        /// <param name="member_id"></param>
        /// <param name="tag_id"></param>
        /// <returns></returns>
        public Task<bool> GetMemberTags(int member_id, int tag_id);
    }
}