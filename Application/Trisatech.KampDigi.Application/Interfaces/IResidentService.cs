using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Application.Models.Resident;

namespace Trisatech.KampDigi.Application.Interfaces
{
    public interface IResidentService
    {
        Task<List<ResidentListModel>> GetList();
        Task<ResidentAddModel> ResidentAdd(ResidentAddModel model);
        Task<ResidentDetailModel> ResidentDetail(Guid idResident);
        Task<ResidentEditModel> ResidentEdit(Guid idResident);
        Task<bool> ResidentDelete(Guid id);
    }
}
