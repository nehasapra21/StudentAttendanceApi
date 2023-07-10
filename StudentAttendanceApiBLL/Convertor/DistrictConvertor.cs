using StudentAttendanceApiDAL.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAttendanceApiBLL
{
    public static class DistrictConvertor
    {
        public static District ConvertDistrictDtoToDistrict(DistrictDto districtDto)
        {
            District district = new District();
            district.Id = districtDto.Id;
            district.DistrictGuidId = districtDto.DistrictGuidId;
            district.Name = districtDto.Name;
            district.Status = districtDto.Status;
            district.CreatedBy = districtDto.CreatedBy;
            district.CreatedOn = districtDto.CreatedOn;

            return district;

        }
    }
}
