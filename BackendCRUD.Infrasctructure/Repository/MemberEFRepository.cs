using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Interface;
using AutoMapper;
using BackendCRUD.Infraestructure.Repository;
using BackendCRUD.Infraestructure.Data;
using static BackendCRUD.Common.Enum;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using BackendCRUD.Sql.Queries;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace BackendCRUD.Infraestructure.Repository
{
    public class MemberEFRepository : IMemberRepository
    {
        private readonly string _cadenaConexion;
        private readonly IConfiguration _configuracion;
        private DBContextBackendCRUD _dataBaseDBContext;
        private Mapper _mapper;
        private readonly IConnectionFactory _connectionFactory;

        public MemberEFRepository(IConfiguration configuracion)
        {
            _configuracion = configuracion;
            _cadenaConexion = _configuracion.GetConnectionString("stringConnection");

            var opcionesDBContext = new DbContextOptionsBuilder<DBContextBackendCRUD>();
            //opcionesDBContext.UseMySQL(_cadenaConexion);
            opcionesDBContext.UseSqlServer(_cadenaConexion);

            _dataBaseDBContext = new DBContextBackendCRUD(opcionesDBContext.Options);

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MemberEF, Member>().ReverseMap();
                cfg.CreateMap<MemberTypesEF, Application.Model.MemberType>().ReverseMap();
                cfg.CreateMap<RoleTypeEF, Application.Model.RoleType>().ReverseMap();
                cfg.CreateMap<TagEF, Application.Model.Tag>().ReverseMap();
                cfg.CreateMap<MemberTagEF, Application.Model.MemberTag>().ReverseMap();
            }
            );

            _mapper = new Mapper(config);

        }

        #region Member

        /// <summary>
        /// Insert a un record of Request Member in BD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> InsertMember(Member input)
        {
            try
            {
                MemberEF MemberBD = _mapper.Map<MemberEF>(input);

                _dataBaseDBContext.Member.Add(MemberBD);
                bool result = await _dataBaseDBContext.SaveChangesAsync() > 0;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateMember(Member input)
        {
            try
            {
                MemberEF? MemberBD = await _dataBaseDBContext.Member.FirstOrDefaultAsync(m => m.Id == input.Id);
                {
                    MemberBD.name = input.name;
                    MemberBD.salary_per_year = input.salary_per_year;
                    MemberBD.type = input.type;

                    MemberBD.role = input.role;

                    MemberBD.country = input.country;
                    MemberBD.currencie_name = input.currencie_name;
                }

                _dataBaseDBContext.Member.Update(MemberBD);
                bool result = await _dataBaseDBContext.SaveChangesAsync() > 0;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the list of member using Dapper
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberDTO>> GetMembers()
        {
            List<MemberDTO> outPutList = new List<MemberDTO>();

            using (IDbConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                var result = await connection.QueryAsync<MemberDTO>(MemberQueries.AllMemberDTO);

                //       var result = await connection.QueryAsync<MemberDTO, List<TagDTO>, MemberDTO>(MemberQueries.AllMemberDTO, (member, tag) =>
                //       {
                //           member.tag_list = tag;
                //           return member;
                //       },
                //splitOn: "");


                foreach (var itemMember in result)
                {
                    var tagListDTO = await connection.QueryAsync<TagDTO>(TagQueries.AllTagByMember, new { MemberId = itemMember.Id });
                    itemMember.tag_list = tagListDTO.ToList();
                }

                return result.ToList();
            }
        }

        /// <summary>
        /// Asynchronous reading of member list is executed
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberDTO>> GetMembersOld()
        {
            List<MemberEF>? dataBD = await _dataBaseDBContext.Member.ToListAsync();
            List<MemberDTO> outPutList = new List<MemberDTO>();

            // map the output tho model type
            List<Member> result = _mapper.Map<List<Member>>(dataBD);

            foreach (var Member in result)
            {
                MemberDTO item = new MemberDTO();
                item.Id = Member.Id;
                item.name = Member.name;
                item.salary_per_year = Member.salary_per_year;
                item.type = Member.type;
                item.type_description = _dataBaseDBContext.MemberType.Where(x => x.Id.Equals(Member.type)).Select(x => x.Description).SingleOrDefault();

                item.role = Member.role;
                item.role_description = _dataBaseDBContext.RoleType.Where(x => x.id.Equals(Member.role)).Select(x => x.description).SingleOrDefault();

                item.country = Member.country;
                item.currencie_name = Member.currencie_name;

                // search for tags that belong to the member
                var tagListDTO = (from mt in _dataBaseDBContext.MemberTag
                                  join t in _dataBaseDBContext.Tag on mt.Tag_id equals t.Id
                                  where
                                  mt.Member_id.Equals(Member.Id)
                                  select new TagDTO
                                  {
                                      Id = mt.Tag_id,
                                      Label = t.Label
                                  }).ToList();

                item.tag_list = tagListDTO;

                outPutList.Add(item);
            }

            return outPutList;
        }

        /// <summary>
        /// Validate if exists te same Member for the employee. Return TRUE if exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistsMemberByName(string name)
        {
            List<MemberEF>? dataBD = await _dataBaseDBContext.Member.Where(x => x.name.ToUpper().Equals(name.ToUpper())).ToListAsync();
            if (dataBD == null || dataBD.Count == 0)
                return false;
            else
                return true;
        }


        /// <summary>
        /// Validate if exists a Member by Id. Return TRUE if exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistsMemberById(int id)
        {
            //MemberEF? dataBD = await _dataBaseDBContext.Member.Where(x => x.Id.Equals(Id)).SingleOrDefaultAsync();

            //List<MemberDTO> outPutList = new List<MemberDTO>();

            using (IDbConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<MemberDTO>(MemberQueries.MemberByIdDTO, new { MemberId = id });

                return (result != null);
            }
        }

        /// <summary>
        /// Delete Member by Id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteMember(int id)
        {
            //using (IDbConnection connection = new SqlConnection(_cadenaConexion))
            //{
            //    connection.Open();
            //    var result = await connection.QuerySingleOrDefaultAsync<MemberDTO>(MemberQueries.MemberByIdDTO, new { MemberId = id });

            //    if (result == null)
            //        return false;
            //}

            MemberEF? dataBD = await _dataBaseDBContext.Member.Where(x => x.Id.Equals(id)).SingleOrDefaultAsync();
            if (dataBD == null)
                return false;
            else
            {
                List<MemberTagEF>? tagList = await _dataBaseDBContext.MemberTag.Where(x => x.Member_id.Equals(id)).ToListAsync();

                if (tagList.Any())
                {
                    foreach (var tag in tagList)
                        _dataBaseDBContext.MemberTag.Remove(tag);
                }

                _dataBaseDBContext.Member.Remove(dataBD);
                bool result = await _dataBaseDBContext.SaveChangesAsync() > 0;

                return result;
            }
        }

        /// <summary>
        /// Get the list member by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<MemberDTO> GetMembersByName(string name)
        {
            // Take the last record 
            MemberEF? dataBD = await _dataBaseDBContext.Member.Where(x => x.name.ToUpper().Equals(name.ToUpper())
                                                                                    ).OrderByDescending(x => x.Id).Take(1).SingleOrDefaultAsync();

            if (dataBD == null)
                return null;

            // map the output tho model type
            Member result = _mapper.Map<Member>(dataBD);

            MemberDTO item = new MemberDTO();
            item.Id = result.Id;
            item.name = result.name;
            item.salary_per_year = result.salary_per_year;
            item.type = result.type;
            item.type_description = _dataBaseDBContext.MemberType.Where(x => x.Id.Equals(result.type)).Select(x => x.Description).SingleOrDefault();

            item.role = result.role;
            item.role_description = _dataBaseDBContext.MemberType.Where(x => x.Id.Equals(result.type)).Select(x => x.Description).SingleOrDefault();

            item.country = result.country;
            item.currencie_name = result.currencie_name;

            //item.tag_list = Member.currencie_name;
            return item;
        }

        /// <summary>
        /// Get the Member type List filter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public async Task<MemberDTO> GetMemberById(int id)
        //{
        //    // Take the last record 
        //    MemberEF? dataBD = await _dataBaseDBContext.Member.Where(x => x.Id == id).SingleOrDefaultAsync();

        //    if (dataBD == null)
        //        return null;

        //    // map the output tho model type
        //    Member result = _mapper.Map<Member>(dataBD);

        //    MemberDTO item = new MemberDTO();
        //    item.Id = result.Id;
        //    item.name = result.name;
        //    item.salary_per_year = result.salary_per_year;
        //    item.type = result.type;
        //    item.type_description = _dataBaseDBContext.MemberType.Where(x => x.Id.Equals(result.type)).Select(x => x.Description).SingleOrDefault();

        //    item.role = result.role;
        //    item.role_description = _dataBaseDBContext.MemberType.Where(x => x.Id.Equals(result.type)).Select(x => x.Description).SingleOrDefault();

        //    item.country = result.country;
        //    item.currencie_name = result.currencie_name;

        //    //item.tag_list = Member.currencie_name;
        //    return item;
        //}

        /// <summary>
        /// Get the object member using Dapper
        /// </summary>
        /// <returns></returns>
        public async Task<MemberDTO> GetMemberById(int id)
        {
            List<MemberDTO> outPutList = new List<MemberDTO>();

            using (IDbConnection connection = new SqlConnection(_cadenaConexion))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<MemberDTO>(MemberQueries.MemberByIdDTO, new { MemberId = id });

                if (result != null)
                {
                    var tagListDTO = await connection.QueryAsync<TagDTO>(TagQueries.AllTagByMember, new { MemberId = id });
                    result.tag_list = tagListDTO.ToList();
                }

                return result;
            }
        }


        #endregion


        #region MemberType

        /// <summary>
        /// Get the full Member types List
        /// </summary>
        /// <returns></returns>
        public async Task<List<Application.Model.MemberType>> GetMemberTypes()
        {
            List<MemberTypesEF>? dataBD = await _dataBaseDBContext.MemberType.ToListAsync();

            if (dataBD == null)
                return null;

            // map the output tho model type
            List<Application.Model.MemberType> resultModel = _mapper.Map<List<Application.Model.MemberType>>(dataBD);

            return resultModel;
        }


        /// <summary>
        /// Get the Member Type By Id
        /// </summary>
        /// <returns></returns>
        public async Task<Application.Model.MemberType> GetMemberTypeById(string id)
        {
            MemberTypesEF? dataBD = await _dataBaseDBContext.MemberType.Where(x => x.Id == id).SingleOrDefaultAsync();

            if (dataBD == null)
                return null;

            // map the output tho model type
            Application.Model.MemberType result = _mapper.Map<Application.Model.MemberType>(dataBD);

            return result;
        }

        #endregion

        #region RoleType


        /// <summary>
        /// Insert a RoleType record
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> InsertRoleType(RoleType input)
        {
            try
            {
                RoleTypeEF dataBD = _mapper.Map<RoleTypeEF>(input);

                _dataBaseDBContext.RoleType.Add(dataBD);
                bool result = await _dataBaseDBContext.SaveChangesAsync() > 0;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get the full Role types List
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleType>> GetRoleTypes()
        {
            List<RoleTypeEF>? dataBD = await _dataBaseDBContext.RoleType.ToListAsync();

            if (dataBD == null)
                return null;

            // map the output tho model type
            List<RoleType> resultModel = _mapper.Map<List<RoleType>>(dataBD);

            return resultModel;
        }


        /// <summary>
        /// Get the Member Type By Id
        /// </summary>
        /// <returns></returns>
        public async Task<RoleType> GetRoleTypeById(int id)
        {
            RoleTypeEF? dataBD = await _dataBaseDBContext.RoleType.Where(x => x.id == id).SingleOrDefaultAsync();

            if (dataBD == null)
                return null;

            // map the output tho model type
            Application.Model.RoleType result = _mapper.Map<Application.Model.RoleType>(dataBD);

            return result;
        }


        #endregion


        #region Tag

        /// <summary>
        /// Get the full Tags List
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tag>> GetTags()
        {
            List<TagEF>? dataBD = await _dataBaseDBContext.Tag.ToListAsync();

            if (dataBD == null)
                return null;

            // map the output tho model type
            List<Tag> resultModel = _mapper.Map<List<Tag>>(dataBD);

            return resultModel;
        }

        /// <summary>
        /// Get the list if tag by label
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tag>> GetTagsByLabel(string label)
        {
            List<TagEF>? dataBD = await _dataBaseDBContext.Tag.Where(x => x.Label.ToLower().Contains(label.ToLower())).ToListAsync();

            if (dataBD == null)
                return null;

            // map the output tho model type
            List<Tag> resultModel = _mapper.Map<List<Tag>>(dataBD);

            return resultModel;
        }


        /// <summary>
        /// Get a specific tag by label
        /// </summary>
        /// <returns></returns>
        public async Task<Tag> GetTagByLabel(string label)
        {

            List<TagEF>? dataBD = await _dataBaseDBContext.Tag.Where(x => x.Label.Trim().ToLower().Equals(label.Trim().ToLower())).ToListAsync();

            if (dataBD == null)
                return null;

            if (dataBD.Count == 0)
                return null;

            // map the output tho model type
            List<Tag> resultModel = _mapper.Map<List<Tag>>(dataBD);

            return resultModel[0];  // return the first record
        }


        /// <summary>
        /// Insert a new Tag
        /// </summary>
        /// <returns></returns>
        public async Task<Tag> InsertTag(Tag input)
        {
            try
            {
                TagEF dataBD = _mapper.Map<TagEF>(input);

                _dataBaseDBContext.Tag.Add(dataBD);
                bool result = await _dataBaseDBContext.SaveChangesAsync() > 0;

                if (result)
                {
                    Tag resultModel = _mapper.Map<Tag>(dataBD);
                    return resultModel;
                }

                throw new Exception("Error inserting Tag");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        #region MemberTag

        /// <summary>
        /// Get the full list of member and tags
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberTagDTO>> GetMemberTags()
        {
            List<MemberTagDTO>? dataBD = await (from mt in _dataBaseDBContext.MemberTag
                                                join mem in _dataBaseDBContext.Member on mt.Member_id equals mem.Id
                                                join tag in _dataBaseDBContext.Tag on mt.Tag_id equals tag.Id
                                                orderby mt.Member_id ascending, tag.Label ascending
                                                select new MemberTagDTO
                                                {
                                                    Member_id = mt.Member_id,
                                                    Tag_id = mt.Tag_id,
                                                    member_name = mem.name,
                                                    Tag_label = tag.Label
                                                }
                                                ).ToListAsync();

            if (dataBD == null)
                return null;

            return dataBD;
        }


        /// <summary>
        /// Get the full list of tags for a specific member id
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberTag>> GetMemberTags(int member_id)
        {
            List<MemberTagEF>? dataBD = await _dataBaseDBContext.MemberTag.Where(x => x.Member_id == member_id).ToListAsync();

            if (dataBD == null)
                return null;

            // map the output tho model type
            List<MemberTag> resultModel = _mapper.Map<List<MemberTag>>(dataBD);

            return resultModel;
        }


        /// <summary>
        /// Insert a member tag record
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> InsertMemberTag(MemberTag input)
        {
            try
            {
                MemberTagEF dataBD = _mapper.Map<MemberTagEF>(input);

                _dataBaseDBContext.MemberTag.Add(dataBD);
                bool result = await _dataBaseDBContext.SaveChangesAsync() > 0;

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Return TRUE if the member id has the tag id asociated
        /// </summary>
        /// <param name="member_id"></param>
        /// <param name="tag_id"></param>
        /// <returns></returns>
        public async Task<bool> GetMemberTags(int member_id, int tag_id)
        {
            List<MemberTagEF>? dataBD = await _dataBaseDBContext.MemberTag.Where(x => x.Member_id == member_id && x.Tag_id == tag_id).ToListAsync();

            if (dataBD == null)
                return false;

            return dataBD.Count > 0 ? true : false;
        }

        #endregion

    }
}