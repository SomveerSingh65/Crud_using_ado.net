using C03_06_2025.model;
using Microsoft.AspNetCore.Mvc;

namespace C03_06_2025.Repo.Abstract
{
    public interface IRepository
    {
        Task<CommonModel> create(modelRequest model);
        Task<IEnumerable<modelRequest>> GetAllDetails();
        Task<modelRequest> GetSingleDetails(int id);
        Task<CommonModel> Delete(int id);
        Task<CommonModel> Update(modelRequest model);
    }
}
