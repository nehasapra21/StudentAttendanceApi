using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class VillageConvertor
    {
        public static Village ConvertVillageDtoToVillage(VillageDto villageDto)
        {
            Village village = new Village();
            village.Id = villageDto.Id;
            village.VillageGuidId = villageDto.VillageGuidId;
            village.Name = villageDto.Name;
            village.Status = villageDto.Status;
            village.DistrictId = villageDto.DistrictId;
            village.VidhanSabhaId = villageDto.VidhanSabhaId;
            village.PanchayatId = villageDto.PanchayatId;
            village.CreatedBy = villageDto.CreatedBy;
            village.CreatedOn = villageDto.CreatedOn;

            return village;

        }
    }
}
