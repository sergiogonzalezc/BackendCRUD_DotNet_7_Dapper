using System.Text.Json;
using System.Text.Json.Nodes;
using BackendCRUD.Application.ConfiguracionApi;
using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using BackendCRUD.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static BackendCRUD.Common.Enum;


namespace BackendCRUD.Application.Services
{
    public class MembersApplication : IMemberApplication
    {
        private IMemberRepository _MembersRepository;
        private IConfiguration _configuration;

        public MembersApplication(IMemberRepository MembersRepository, IConfiguration configuration)
        {
            _MembersRepository = MembersRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Validate if exists te same Member for the employee
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> GetValidateMember(InputValidateMember input)
        {
            bool result = await ExistsMemberByName(input.name);

            if (result)
                return result;
            else
                throw new Exception(string.Format("Member does not exists for employee name [{0}]!", input.name));
        }

        /// <summary>
        /// Insert a un record of Request Member in BD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResultRequestDTO> InsertMember(InputCreateMember input)
        {
            string nameMethod = nameof(InsertMember);
            bool result = false;

            try
            {
                // validate and get de currencie code
                Member dataMember = await ValidateInput(ActionType.ADD, input.name, input.salary_per_year, input.type, input.role, input.country);

                if (dataMember == null)
                    throw new Exception("Error: try again");
                //Member dataMember = new Member();
                //dataMember.name = input.name;
                //dataMember.salary_per_year = input.salary_per_year;
                //dataMember.type = input.type;
                //dataMember.role = input.role;
                //dataMember.country = input.country;
                //dataMember.currencie_name = "CL";

                result = await _MembersRepository.InsertMember(dataMember);

                // generate output
                if (!result)
                {
                    return new ResultRequestDTO
                    {
                        Success = false,
                        ErrorMessage = "Error: try again"
                    };
                }
                else
                {
                    return new ResultRequestDTO
                    {
                        Success = true,
                        ErrorMessage = null,
                    };
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Update specific Member
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResultRequestDTO> UpdateMember(InputUpdateMember input)
        {
            string nameMethod = nameof(UpdateMember);
            bool result = false;

            try
            {
                var item = await _MembersRepository.GetMemberById(input.Id);

                if (item == null)
                {
                    throw new Exception(string.Format("Record cannot modify! Id member [{0}] was not found!", input.Id));
                }
                else
                {
                    // validate and get de currencie code
                    Member dataMember = await ValidateInput(ActionType.UPDATE, input.name, input.salary_per_year, input.type, input.role, input.country);

                    if (dataMember == null)
                        throw new Exception("Error: try again");

                    dataMember.Id = input.Id;

                    result = await _MembersRepository.UpdateMember(dataMember);
                }

                // generate output
                if (!result)
                {
                    return new ResultRequestDTO
                    {
                        Success = true,
                        ErrorMessage = "Error: try again"
                    };
                }
                else
                {
                    return new ResultRequestDTO
                    {
                        Success = true,
                        ErrorMessage = null,
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Delete a member by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResultRequestDTO> DeleteMember(int Id)
        {
            try
            {
                bool result = await ExistsMemberById(Id);

                if (!result)
                    throw new Exception(string.Format("Member does not exists for employee Id [{0}]!", Id));


                bool resultDelete = await _MembersRepository.DeleteMember(Id);

                // generate output
                if (!resultDelete)
                {
                    return new ResultRequestDTO
                    {
                        Success = false,
                        ErrorMessage = "Error: try again"
                    };
                }
                else
                {
                    return new ResultRequestDTO
                    {
                        Success = true,
                        ErrorMessage = null,
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Validate input for creation record, or update record. Also get de currencie code
        /// </summary>
        /// <param name="action">ADD for create, UPDATE for update</param>
        /// <param name="name"></param>
        /// <param name="salary_per_year"></param>
        /// <param name="type"></param>
        /// <param name="role"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        private async Task<Member> ValidateInput(ActionType action, string name, int salary_per_year, string type, int? role, string? country)
        {
            string nameMethod = nameof(ValidateInput);
            Member dataMember = new Member();
            string currencie_name = null;
            string baseUrl = string.Empty;
            int? finalRole = null;

            // validate name
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Error: missing input name");
            }

            if (name.Trim().Length > 50)
                throw new Exception($"Error: the length of [name] field is invalid!");

            // validate type
            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("Error: missing input type");
            }
            // validate if member type exists

            // We validate the Member Type Id
            var MemberTypeObject = await _MembersRepository.GetMemberTypeById(type);
            if (MemberTypeObject == null)
                throw new Exception($"Error: type [{type}] is not valid");

            // validate salary
            if (salary_per_year < 0)
            {
                throw new Exception("Error: invalid input salary");
            }

            // for creation record, validate if the name already exists
            if (action == ActionType.ADD && ExistsMemberByName(name).Result)
                throw new Exception($"Error:: The name [{name}] already exists in the database!");

            // For Employee
            if (type == BackendCRUD.Common.Enum.MemberType.E.ToString())
            {
                // if the type is "E" (employe), must validate the role
                if (!role.HasValue)
                {
                    throw new Exception($"Error:: role is not valid, is a required field");
                }
                else if (role.Value <= 0)
                {
                    throw new Exception($"Error:: role value [{role.Value}] is not valid");
                }
                else
                {
                    // We validate the Member Type Id
                    var roleTypeObject = await _MembersRepository.GetRoleTypeById(role.Value);
                    if (roleTypeObject == null)
                        throw new Exception($"Error:: role [{role.Value}] does not exist");

                    // role is OK!
                    finalRole = role;
                }

                // if the type is "E" (employe), must validate the Country Field and call API, example: https://restcountries.com/v3.1/name/chile

                // validate if the country exists, and must have a lenght > 1 
                if (string.IsNullOrEmpty(country))
                {
                    throw new Exception("Error:: country value is required for an Employee member");
                }

                if (country.Trim().Length > 40)
                    throw new Exception($"Error: the length of [country] field is invalid!");

                if (country.Length > 1)
                {
                    try
                    {
                        baseUrl = _configuration.GetSection("countryConfig").GetSection("endpoint").Value;

                        if (string.IsNullOrEmpty(baseUrl))
                            throw new Exception("Error reading endpoint Country API");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error reading endpoint Country API");
                    }

                    Api apiCountry = new Api(baseUrl, ConfiguracionApi.CallType.EnumCallType.Get);

                    //Concatenate the country and call the api

                    HttpResponseMessage response = await apiCountry.CallApi(country.ToLower());

                    if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
                    {
                        ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Error, nameMethod, $"Readed OK! - Code [" + response.StatusCode + "] Content [" + (response.Content == null ? "NULL" : "documento correcto" + "]"));

                        if (response.Content != null)
                        {
                            // Try to extract de "currency data" by the Country Field
                            try
                            {
                                string jsonObjectDataResult = await response.Content.ReadAsStringAsync();

                                var jsonDocument = JsonDocument.Parse(jsonObjectDataResult);

                                // parse from stream, string, utf8JsonReader
                                JsonArray? jsonArray = JsonNode.Parse(jsonObjectDataResult)?.AsArray();

                                //var parsed = JObject.Parse(jsonArray[0].ToString());
                                //var vv = parsed.SelectToken("[0].currencies[0].name").Value<string>();

                                JArray obj = JArray.Parse(jsonObjectDataResult);


                                var jsonObject = JObject.Parse(jsonArray[0].ToString());
                                var currencieObject = (JObject)jsonObject["currencies"];
                                string str = currencieObject.ToString().Replace("\r\n", "").Replace("{", "").Replace("}", "").Replace("\"", "").Trim();

                                if (!string.IsNullOrEmpty(str))
                                {
                                    string[] wordSeparatesList = str.Split(":");
                                    for (int t = 0; t < wordSeparatesList.Length; t++)
                                    {
                                        if (wordSeparatesList[t].Trim() == "name")
                                        {
                                            string[] wordSeparatesListComa = wordSeparatesList[t + 1].Split(",");

                                            currencie_name = wordSeparatesListComa[0].Trim();
                                            break;
                                        }
                                    }
                                }


                                //var elementValue = jsonDocument.RootElement.GetProperty("currencies").GetString();

                                //var jsonElement = jsonDocument.RootElement.TryGetProperty("currencies", out var targetElement);


                                //foreach (JObject obj2 in obj.Children<JObject>())
                                //{
                                //    foreach (JProperty singleProp in obj2.Properties())
                                //    {
                                //        if (singleProp.Name == "currencies")
                                //        {
                                //            string value = singleProp.Value.ToString();                                               
                                //        }                                           
                                //    }
                                //}

                            }
                            catch (Exception ex)
                            {
                                currencie_name = null;
                            }
                        }
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            //GestorLog.Write(MedicalWebScraping.Common.Enum.LogType.WebSite, TraceLevel.Error, nameMethod, $"Respuesta Error (bad request)! Registro No Insertado! - Sucursal Id [{sucursalItemData.Codigo}] - " + doctorName + "/" + specialityName + "/" + firsthourData + " - Code[" + response.StatusCode + "] Content[" + (response.Content == null ? "NULL" : response.Content.ReadAsStringAsync().Result + "]"));
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Error, nameMethod, $"Respuesta Error (no autorizado)!: Registro No Insertado! - Code[" + response.StatusCode + "] Content[" + (response.Content == null ? "NULL" : response.Content.ReadAsStringAsync().Result + "]"));
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Error, nameMethod, $"Respuesta Error (no autorizado)!: Registro No Insertado! - Code[" + response.StatusCode + "] Content[" + (response.Content == null ? "NULL" : response.Content.ReadAsStringAsync().Result + "]"));

                        }
                        else
                            ServiceLog.Write(BackendCRUD.Common.Enum.LogType.WebSite, System.Diagnostics.TraceLevel.Error, nameMethod, "Respuesta Error!: Content [" + (response.Content == null ? "NULL" : response.Content.ReadAsStringAsync().Result + "]"));

                        //return new ResultRequestMemberDTO
                        //{
                        //    Success = true,
                        //    ErrorMessage = $"Could not connect calling country API: please check country input [{input.country}]"
                        //};
                        //throw new Exception($"Could not connect calling country API: please check country input [{country}]");
                    }
                }
            }

            dataMember.name = name;
            dataMember.salary_per_year = salary_per_year;
            dataMember.type = type;
            dataMember.role = finalRole;
            dataMember.country = country;
            dataMember.currencie_name = currencie_name;

            return dataMember;
        }

        /// <summary>
        /// Get the full list of members
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberDTO>> GetMembers()
        {

            List<MemberDTO> resultado = new List<MemberDTO>();
            resultado = await _MembersRepository.GetMembers();
            //if (resultado.Count == 0)
            //    throw new Exception("No existen datos.");

            return resultado;
        }

        /// <summary>
        /// Validate if exists te same Member for the employee. Return TRUE if exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistsMemberByName(string name)
        {
            return await _MembersRepository.ExistsMemberByName(name);
        }


        /// <summary>
        /// Validate if exists a Member by Id. Return TRUE if exists.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> ExistsMemberById(int Id)
        {
            return await _MembersRepository.ExistsMemberById(Id);
        }


        /// <summary>
        /// Get a member list by name of member
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lastName"></param>
        /// <param name="MemberType"></param>
        /// <returns></returns>
        public Task<MemberDTO> GetMembersByName(string name)
        {
            return _MembersRepository.GetMembersByName(name);
        }

        /// <summary>
        /// Get the Member type List filter by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MemberDTO> GetMembersById(int id)
        {
            if (id < 0)
                throw new Exception("Error: invalid code");
                
            MemberDTO outPut = await _MembersRepository.GetMemberById(id);

            if (outPut == null)
                throw new Exception("Error: member not found");

            return outPut;

        }

        /// <summary>
        /// Get the full Member type List
        /// </summary>
        /// <returns></returns>
        public async Task<List<Model.MemberType>> GetMemberTypes()
        {
            return await _MembersRepository.GetMemberTypes();
        }


        /// <summary>
        /// Get the Member type List by Id
        /// </summary>
        /// <returns></returns>
        public async Task<Model.MemberType> GetMemberTypeById(string id)
        {
            return await _MembersRepository.GetMemberTypeById(id);
        }


        #region RoleType

        /// <summary>
        /// Insert a un record of Role Type in BD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResultRequestDTO> InsertRoleType(InputCreateRoleType input)
        {
            try
            {
                if (input.Id <= 0)
                    throw new Exception($"Can'tinsert Role Type {input.Id}: Role Type value is invalid!");

                if (string.IsNullOrEmpty(input.Description))
                    throw new Exception($"Can'tinsert Role Type {input.Id}: [Description] field is required!");

                if (input.Description.Trim().Length > 50)
                    throw new Exception($"Can'tinsert Role Type {input.Id}: the length of [Description] field is invalid!");

                // validate if exists the same Id
                RoleType result = await _MembersRepository.GetRoleTypeById(input.Id);

                if (result != null)
                    throw new Exception($"Can'tinsert Role Type {input.Id}: Role Type duplicated!");
                else
                {
                    RoleType newObject = new RoleType
                    {
                        id = input.Id,
                        description = input.Description,
                    };

                    var resultInsert = await _MembersRepository.InsertRoleType(newObject);

                    // generate output
                    if (!resultInsert)
                    {
                        return new ResultRequestDTO
                        {
                            Success = false,
                            ErrorMessage = "Error: try again"
                        };
                    }
                    else
                    {
                        return new ResultRequestDTO
                        {
                            Success = true,
                            ErrorMessage = null,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// GetRoleTypes
        /// </summary>
        /// <returns></returns>
        public async Task<List<Model.RoleType>> GetRoleTypes()
        {
            return await _MembersRepository.GetRoleTypes();
        }

        /// <summary>
        /// Get the Member Type By Id
        /// </summary>
        /// <returns></returns>
        public async Task<RoleType> GetRoleTypeById(int id)
        {
            return await _MembersRepository.GetRoleTypeById(id);
        }

        #endregion


        #region Tags
        /// <summary>
        /// Get the full Tags List
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tag>> GetTags()
        {
            return await _MembersRepository.GetTags();
        }


        /// <summary>
        /// Get the full Tags List
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberTagDTO>> GetMemberTags()
        {
            return await _MembersRepository.GetMemberTags();
        }


        /// <summary>
        /// Get the full list of tags for a specific member id
        /// </summary>
        /// <returns></returns>
        public async Task<List<MemberTag>> GetMemberTags(int member_id)
        {
            return await _MembersRepository.GetMemberTags(member_id);
        }

        /// <summary>
        /// Get list tags by label
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tag>> GetTagsByLabel(string label)
        {
            return await _MembersRepository.GetTagsByLabel(label);
        }

        /// <summary>
        /// Get a specific tag by label
        /// </summary>
        /// <returns></returns>
        public async Task<Tag> GetTagByLabel(string label)
        {
            return await _MembersRepository.GetTagByLabel(label);
        }



        /// <summary>
        /// Insert a un record of member tag in BD
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ResultRequestDTO> InsertMemberTag(InputCreateMemberTag input)
        {
            try
            {
                if (input.member_id <= 0)
                    throw new Exception($"Can't insert Member Tag: [member Id] field is invalid!");

                if (string.IsNullOrEmpty(input.label_tag))
                    throw new Exception($"Can't insert Member Tag: [label_tag] field is invalid!");

                if (input.label_tag.Trim().Length > 50)
                    throw new Exception($"Error: the length of [label_tag] field is invalid!");

                // search the member Id if exists
                MemberDTO result = await _MembersRepository.GetMemberById(input.member_id);

                if (result == null)
                    throw new Exception($"Can't insert Role Type: [member Id] doesn't exists!");

                // search id then tag exists searching by label (exact match)
                Tag resultTag = await _MembersRepository.GetTagByLabel(input.label_tag);

                // if not exist the exact tag, insert the bew tag, and then asociate to the member
                if (resultTag == null)
                {
                    Tag newObjectTag = new Tag()
                    {
                        Label = input.label_tag,
                    };

                    resultTag = await _MembersRepository.InsertTag(newObjectTag);
                }

                // validate if the member id has the tag id asociated
                bool hasAsociateTheTag = await _MembersRepository.GetMemberTags(input.member_id, resultTag.Id);
                if (hasAsociateTheTag)
                {
                    return new ResultRequestDTO
                    {
                        Success = true,
                        ErrorMessage = null,
                    };
                }


                MemberTag newObject = new MemberTag
                {
                    Member_id = input.member_id,
                    Tag_id = resultTag.Id,
                };

                var resultInsert = await _MembersRepository.InsertMemberTag(newObject);

                // generate output
                if (!resultInsert)
                {
                    return new ResultRequestDTO
                    {
                        Success = false,
                        ErrorMessage = "Error: try again"
                    };
                }
                else
                {
                    return new ResultRequestDTO
                    {
                        Success = true,
                        ErrorMessage = null,
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        // recursively yield all children of json
        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }

        public static Dictionary<string, JsonElement> GetFlat(string json)
        {
            IEnumerable<(string Path, JsonProperty P)> GetLeaves(string path, JsonProperty p)
                => p.Value.ValueKind != JsonValueKind.Object
                    ? new[] { (Path: path == null ? p.Name : path + "." + p.Name, p) }
                    : p.Value.EnumerateObject().SelectMany(child => GetLeaves(path == null ? p.Name : path + "." + p.Name, child));

            using (JsonDocument document = JsonDocument.Parse(json)) // Optional JsonDocumentOptions options
                return document.RootElement.EnumerateObject()
                    .SelectMany(p => GetLeaves(null, p))
                    .ToDictionary(k => k.Path, v => v.P.Value.Clone()); //Clone so that we can use the values outside of using
        }
    }
}
