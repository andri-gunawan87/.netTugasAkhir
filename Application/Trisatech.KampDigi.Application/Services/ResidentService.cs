using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Application.Interfaces;
using Trisatech.KampDigi.Application.Models.Resident;
using Trisatech.KampDigi.Domain;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Services
{
    public class ResidentService : BaseDbService, IResidentService
    {
        public ResidentService(KampDigiContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<ResidentListModel>> GetList()
        {
            var ListResident = await (from a in Db.Residents
                                      join b in Db.Users on a.Id equals b.ResidentId
                                      select new ResidentListModel
                                      {
                                          Name = a.Name,
                                          ContactNumber = a.ContactNumber,
                                          IdentityPhoto = a.IdentityPhoto,
                                          Gender = a.Gender,
                                          HouseId = a.HouseId,
                                          Username = b.Username,
                                      }
                                      ).Take(10).ToListAsync();
            return ListResident;
        }

        public async Task<ResidentAddModel> ResidentAdd(ResidentAddModel model)
        {
            if (await Db.Residents.AnyAsync(x => x.IdentityNumber == model.IdentityNumber))
            {
                throw new InvalidOperationException($"Username {model.IdentityNumber} sudah terdaftar");
            }
            
            var newResident = new Resident
            {
                Name = model.Name,
                IdentityNumber = model.IdentityNumber,
                ContactNumber = model.ContactNumber,
                OccupantType = model.OccupantType,
                TotalOccupant = model.TotalOccupant,
                IdentityPhoto = model.IdentityPhoto,
                IdentityFamilyPhoto = model.IdentityFamilyPhoto,
                Gender = model.Gender,
                EmergencyCallName = model.EmergencyCallName,
                EmergencyCallNumber = model.EmergencyCallNumber,
                EmergencyCallRelation = model.EmergencyCallRelation,
                IsOccupant = model.IsOccupant,

            };
            await Db.Residents.AddAsync(newResident);
            await Db.SaveChangesAsync();

            var newUser = new User
            {
                Name = model.Name,
                Username = model.Username,
                Role = model.Role,
                Password = model.Password,
                ResidentId = newResident.Id
            };

            await Db.Users.AddAsync(newUser);
            await Db.SaveChangesAsync();

            return model;
        }

        public async Task<bool> ResidentDelete(Guid idResident)
        {
            var user = Db.Residents.Find(idResident);
            if (user == null)
            {
                throw new InvalidOperationException($"User dengan ID {idResident} tidak dapat ditemukan");
            }
            Db.Remove(user);
            await Db.SaveChangesAsync();
            return true;
        }

        public async Task<ResidentDetailModel> ResidentDetail(Guid idResident)
        {
            var dataResident = Db.Residents.First(x => x.Id == idResident);
            if (dataResident == null)
            {
                throw new InvalidOperationException($"User dengan ID {idResident} tidak dapat ditemukan");
            }
            var residentModel = new ResidentDetailModel
            {
                Name = dataResident.Name,
                IdentityNumber = dataResident.IdentityNumber,
                ContactNumber = dataResident.ContactNumber,
                OccupantType = dataResident.OccupantType,
                TotalOccupant = dataResident.TotalOccupant,
                IdentityPhoto = dataResident.IdentityPhoto,
                IdentityFamilyPhoto = dataResident.IdentityFamilyPhoto,
                Gender = dataResident.Gender,
                EmergencyCallName = dataResident.EmergencyCallName,
                EmergencyCallNumber = dataResident.EmergencyCallNumber,
                EmergencyCallRelation = dataResident.EmergencyCallRelation,
                HouseId = dataResident.HouseId,
                IsOccupant = dataResident.IsOccupant,
            };

            return residentModel;

        }

        public async Task<ResidentEditModel> ResidentEdit(Guid idResident)
        {
            var dataResident = Db.Residents.First(x => x.Id == idResident);
            if (dataResident == null)
            {
                throw new InvalidOperationException($"User dengan ID {idResident} tidak dapat ditemukan");
            }
            var residentModel = new ResidentEditModel
            {
                Name = dataResident.Name,
                IdentityNumber = dataResident.IdentityNumber,
                ContactNumber = dataResident.ContactNumber,
                OccupantType = dataResident.OccupantType,
                TotalOccupant = dataResident.TotalOccupant,
                IdentityPhoto = dataResident.IdentityPhoto,
                IdentityFamilyPhoto = dataResident.IdentityFamilyPhoto,
                EmergencyCallName = dataResident.EmergencyCallName,
                EmergencyCallNumber = dataResident.EmergencyCallNumber,
                EmergencyCallRelation = dataResident.EmergencyCallRelation,
                HouseId = dataResident.HouseId,
                IsOccupant = dataResident.IsOccupant,
            };

            return residentModel;
        }
    }
}
