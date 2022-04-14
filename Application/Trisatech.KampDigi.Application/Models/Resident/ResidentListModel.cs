using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trisatech.KampDigi.Domain.Entities;

namespace Trisatech.KampDigi.Application.Models.Resident
{
    public class ResidentListModel
    {
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string IdentityPhoto { get; set; }
        public Gender Gender { get; set; }

        public Guid HouseId { get; set; }
        public string Username { get; set; }
    }
}
