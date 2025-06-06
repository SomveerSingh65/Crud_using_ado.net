
using C03_06_2025.model;
using C03_06_2025.Repo.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace C03_06_2025.Repo.Concate
{
    public class Repositoryy : IRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public Repositoryy(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<CommonModel> create(modelRequest model)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("SaveDetails", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                //cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@name", model.Name);
                cmd.Parameters.AddWithValue("@mobileNumber", model.mobileNumber);
               

                return await Task.Run(() =>
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return new CommonModel
                    {
                        status = 1,
                        message = "Model created successfully"
                    };
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the model: " + ex.Message);
            }
        }

        public async Task<IEnumerable<modelRequest>> GetAllDetails()
        {
            CommonModel commonModel = new CommonModel();
            try
            {
                var modelList = new List<modelRequest>();
                using (var conn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("GetAllDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        await conn.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var model = new modelRequest
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    mobileNumber = reader.GetString(reader.GetOrdinal("mobileNumber"))
                                };
                                modelList.Add(model);
                            }
                        }
                    }
                }
                return modelList;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all details: " + ex.Message);
            }
        }

        public async Task<modelRequest> GetSingleDetails(int id)
        {
            try
            {
                CommonModel commonModel = new CommonModel();
                using (var conn = new SqlConnection(_connectionString))
                {
                    using (var cmd = new SqlCommand("GetSingleDetails", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        await conn.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new modelRequest
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    mobileNumber = reader.GetString(reader.GetOrdinal("mobileNumber"))
                                };
                            }
                            else
                            {
                                commonModel.status = 0;
                                commonModel.message = "No record found with the provided ID.";
                                return null; ;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the single detail: " + ex.Message);
            }
        }

        public async Task<CommonModel> Update(modelRequest model)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("UpdateModel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@mobileNumber", model.mobileNumber);
                return await Task.Run(() =>
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return new CommonModel
                    {
                        status = 1,
                        message = "Model updated successfully"
                    };
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the model: " + ex.Message);
            }
        }

        public Task<CommonModel> Delete(int id)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("DeleteModel", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                return Task.Run(() =>
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return new CommonModel
                    {
                        status = 1,
                        message = "Model deleted successfully"
                    };
                });
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the model: " + ex.Message);
            }
        }
    }
}
