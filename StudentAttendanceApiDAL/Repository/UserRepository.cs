using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StudentAttendanceApiDAL.IRepository;
using StudentAttendanceApiDAL.Model;
using StudentAttendanceApiDAL.Tables;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudentAttendanceApiDAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private IConfiguration configuration;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public UserRepository(AppDbContext appDbContext, ILogger<UserRepository> logger, IConfiguration configuration)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
        }

       

        public async Task<Users> GetUserById(int userId)
        {
            logger.LogInformation($"UserRepository : GetUserById : Started");

            Users user = new Users();
            try
            {
                user = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);

                if (user != null && user.Type == (int)Constant.Type.Teacher)
                {
                    user = await appDbContext.Users.Include(x => x.District)
                                                  .Include(x => x.VidhanSabha)
                                                  .Include(x => x.Panchayat)
                                                  .Include(x => x.Village)
                                                  .Include(x => x.CenterAssignUser)
                                                  .Where(x => x.Id == userId).FirstOrDefaultAsync();

                    user.Center = appDbContext.Center.Include(x => x.CenterAssignUser).FirstOrDefaultAsync(x => x.AssignedTeachers == user.Id).Result;
                    //user = await (from u in appDbContext.Users
                    //              join d in appDbContext.District
                    //              on u.DistrictId equals d.Id
                    //              join vid in appDbContext.VidhanSabha
                    //              on u.VidhanSabhaId equals vid.Id
                    //              into emp
                    //              from Village in emp.DefaultIfEmpty()
                    //              where u.Id == userId
                    //              select new Users
                    //              {
                    //                  Id = u.Id,
                    //                  EnrolmentRollId = u.EnrolmentRollId,
                    //                  Name = u.Name,
                    //                  Age = u.Age,
                    //                  Gender = u.Gender,
                    //                  DateOfBirth = u.DateOfBirth,
                    //                  PhoneNumber = u.PhoneNumber,
                    //                  WhatsApp = u.WhatsApp,
                    //                  GuardianName = u.GuardianName,
                    //                  GuardianNumber = u.GuardianNumber,
                    //                  DistrictId = d.Id,
                    //                  DistrictName = d.Name,
                    //                  VidhanSabhaId = u.VidhanSabhaId,
                    //                  Education = u.Education,
                    //                  PanchayatId = u.PanchayatId,
                    //                  VillageId = Village.Id,
                    //                  VillageName = Village == null ? string.Empty : Village.Name,
                    //              }).AsNoTracking().FirstOrDefaultAsync();
                }
                else if (user != null && user.Type == (int)Constant.Type.RegionalAdmin)
                {
                    user = appDbContext.Users.Include(x => x.District)
                                                  .Include(x => x.VidhanSabha)
                                                  .Include(x => x.Panchayat)
                                                  .Include(x => x.Village)
                                                  .Include(x => x.RegionalAdminPanchayat)
                                                  .Include(x => x.CenterAssignUser)
                                                  .Where(x => x.Id == userId).FirstOrDefault();
                    user.Centers = appDbContext.Center.Include(x => x.CenterAssignUser).Where(x => x.AssignedRegionalAdmin == user.Id).ToList();

                }
                else
                {
                    //user.Center = appDbContext.Center.FirstOrDefaultAsync(x => x.AssignedRegionalAdmin == user.Id).Result;
                    return user;
                }

                //if (user != null)
                //{
                //    List<Panchayat> listOfPanchayat =await appDbContext.Panchayat.AsNoTracking().Where(x => user.PanchayatId.Contains(x.Id.ToString())).ToListAsync();
                //    if (listOfPanchayat != null)

                //        user.PanchayatName = new List<string>();
                //    foreach (var item in listOfPanchayat)
                //    {
                //        user.PanchayatName.Add(item.Name);
                //    }

                //  user.VidhanSabhaName=  appDbContext.VidhanSabha.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.VidhanSabhaId).Result.Name;
                //}
                logger.LogInformation($"UserRepository : GetUserById : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetUserById", ex);
                throw ex;
            }
            return user;
        }

        public async Task<Users?> LoginUser(string userName, string password)
        {
            logger.LogInformation($"UserRepository : LoginUser : Started");

            Users user = new Users();
            try
            {
                user = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.PhoneNumber == userName && x.Password == password);
                if (user != null)
                {
                    ///Generate token for user
                    #region JWT
                    user.Token = CommonUtility.GenerateToken(configuration, user.Email, user.Name);
                    #endregion
                }

                logger.LogInformation($"UserRepository : LoginUser : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : LoginUser ", ex);
                throw ex;
            }
            return user;
        }


        public async Task<Users> SaveLogin(Users user)
        {
            logger.LogInformation($"UserRepository : SaveLogin : Started");

            try
            {
                if (user.Id > 0)
                {
                    var userVal = appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.Id).Result;
                    if (userVal != null)
                    {
                        user.EnrolmentRollId = userVal.EnrolmentRollId;
                        user.Password = userVal.Password;
                        user.CreatedOn = userVal.CreatedOn;
                        user.Status = userVal.Status;
                    }
                    if (user.Type == (int)Constant.Type.Teacher && user.ListOfPanchayatId != null && user.ListOfPanchayatId.Count == 1)
                    {
                        user.PanchayatId = Convert.ToInt32(user.ListOfPanchayatId[0]);
                    }

                    appDbContext.Update(user);

                    if (user.Type == (int)Constant.Type.RegionalAdmin && user.ListOfPanchayatId != null && user.ListOfPanchayatId.Count > 0)
                    {
                        List<RegionalAdminPanchayat> list = await appDbContext.RegionalAdminPanchayat.Where(x => x.UsersId == user.Id).ToListAsync();

                        if (list != null && list.Count > 0)
                        {
                            //var listOfNew=list.Where(l => user.ListOfPanchayatId.Contains    (l.PanchayatId.Value)).ToList();

                            appDbContext.RemoveRange(list);
                            appDbContext.SaveChanges();
                        }

                        AddRegionalAdminPanchayat(user);
                    }
                    await appDbContext.SaveChangesAsync();
                }
                else
                {//
                    user.AssignedTeacherStatus = false;
                    user.AssignedRegionalAdminStatus = false;
                    user.Status = true;
                    user.CreatedOn = DateTime.Now;
                    if (user.Type == (int)Constant.Type.Teacher && user.ListOfPanchayatId != null && user.ListOfPanchayatId.Count == 1)
                    {
                        user.PanchayatId = Convert.ToInt32(user.ListOfPanchayatId[0]);
                    }
                    appDbContext.Users.Add(user);
                    await appDbContext.SaveChangesAsync();
                    if (user.Type == (int)Constant.Type.RegionalAdmin && user.ListOfPanchayatId != null && user.ListOfPanchayatId.Count > 0)
                    {
                        AddRegionalAdminPanchayat(user);

                        await appDbContext.SaveChangesAsync();
                    }
                }

                #region Add list of panchayat in table (regionalAdminPanchayat)
                //if (user.Type == (int)Constant.Type.RegionalAdmin)
                //{
                //    if (user.ListOfPanchayatId != null && user.ListOfPanchayatId.Count > 0)
                //    {
                //        if (user.Id == 0)
                //        {
                //            AddRegionalAdminPanchayat(user);
                //            await appDbContext.SaveChangesAsync();
                //        }
                //        else
                //        {
                //            //Chcek same id exists in db

                //            List<RegionalAdminPanchayat> list = await appDbContext.RegionalAdminPanchayat.Where(x => x.UsersId == user.Id).ToListAsync();

                //            if (list != null)
                //            {
                //                //var listOfNew=list.Where(l => user.ListOfPanchayatId.Contains    (l.PanchayatId.Value)).ToList();

                //                appDbContext.RemoveRange(list);
                //                appDbContext.SaveChanges();
                //            }

                //            AddRegionalAdminPanchayat(user);
                //            appDbContext.SaveChanges();
                //        }
                //    }
                //}


                #endregion
                logger.LogInformation($"UserRepository : SaveLogin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveLogin ", ex);
                throw ex;
            }

            return user;
        }

        private void AddRegionalAdminPanchayat(Users user)
        {
            List<RegionalAdminPanchayat> listOfAdminPacnchayat = new List<RegionalAdminPanchayat>();
            foreach (var item in user.ListOfPanchayatId)
            {
                RegionalAdminPanchayat regionalAdminPanchayat = new RegionalAdminPanchayat();
                regionalAdminPanchayat.UsersId = user.Id;
                regionalAdminPanchayat.PanchayatId = Convert.ToInt32(item);
                regionalAdminPanchayat.PanchayatName = appDbContext.Panchayat.FirstOrDefaultAsync(x => x.Id == Convert.ToInt32(item)).Result.Name;
                listOfAdminPacnchayat.Add(regionalAdminPanchayat);
            }
            appDbContext.AddRangeAsync(listOfAdminPacnchayat);
        }
        public async Task<Users> SaveSuperAdmin(Users user)
        {
            logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");

            try
            {
                if (user.Id > 0)
                {
                    appDbContext.Entry(user).State = EntityState.Modified;
                }
                else
                {//
                    user.CreatedOn = DateTime.Now;
                    user.AssignedTeacherStatus = false;
                    user.AssignedRegionalAdminStatus = false;
                    appDbContext.Users.Add(user);
                }
                await appDbContext.SaveChangesAsync();

                logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveSuperAdmin ", ex);
                throw ex;
            }
            return user;
        }

        public async Task<string> CheckUserMobileNumber(string mobileNumber)
        {
            logger.LogInformation($"UserRepository : CheckUserMobileNumber : Started");

            string? mobileNo = string.Empty;
            try
            {
                mobileNo = appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.PhoneNumber == mobileNumber).Result.PhoneNumber;

                logger.LogInformation($"UserRepository : GetUserById : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetUserById", ex);
                throw ex;
            }
            return mobileNo;
        }

        public async Task<List<Users>> GetAllTeachers()
        {
            logger.LogInformation($"UserRepository : GetRegisteredTeachers : Started");

            List<Users> users = new List<Users>();

            try
            {
                users = await (from u in appDbContext.Users.Where(x =>  x.Type == 3 && x.Status.Value)
                               select new Users
                               {
                                   Id = u.Id,
                                   Name = u.Name,
                                   AssignedTeacherStatus = u.AssignedTeacherStatus
                               }).OrderByDescending(x => x.Id).ToListAsync();
                logger.LogInformation($"UserRepository : GetRegisteredTeachers : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetRegisteredTeachers", ex);
            }
            return users;
        }

        public async Task<List<Users>> GetAllUnAssignedTeacher()
        {
            logger.LogInformation($"UserRepository : GetAllTeachers : Started");

            List<Users> users = new List<Users>();
            try
            {
                users = await (from u in appDbContext.Users.Where(x=>x.AssignedTeacherStatus==false && x.Type == 3 && x.Status.Value)
                               select new Users
                               {
                                   Id = u.Id,
                                   Name = u.Name,
                                   Picture = u.Picture,
                                   AssignedTeacherStatus = u.AssignedTeacherStatus
                               }).OrderByDescending(x => x.Id).ToListAsync();

                //users = await (from u in appDbContext.Users

                //               where u.Type == (int)Constant.Type.Teacher
                //               select new Users
                //               {
                //                   Id = u.Id,
                //                   EnrolmentRollId = u.EnrolmentRollId,
                //                   Name = u.Name,
                //                   Age = u.Age,
                //                   Gender = u.Gender,
                //                   DateOfBirth = u.DateOfBirth,
                //                   PhoneNumber = u.PhoneNumber,
                //                   WhatsApp = u.WhatsApp,
                //                   GuardianName = u.GuardianName,
                //                   GuardianNumber = u.GuardianNumber,
                //                   DistrictId = d.Id,
                //                   DistrictName = d.Name,
                //                   VidhanSabhaId = u.VidhanSabhaId,
                //                   Education = u.Education,
                //                   PanchayatId = u.PanchayatId,
                //                   VillageId = Village.Id,
                //                   VillageName = Village == null ? string.Empty : Village.Name,
                //                   CenterName = Center == null ? string.Empty : Center.CenterName,
                //                   //CenterName=c.CenterName,
                //                   AssignedTeacherStatus = u.AssignedTeacherStatus,
                //                   EnrollmentDate = u.EnrollmentDate
                //               }).AsNoTracking().ToListAsync();

                //if (users != null)
                //{
                //    foreach (var user in users)
                //    {
                //        List<Panchayat> listOfPanchayat = await appDbContext.Panchayat.AsNoTracking().Where(x => user.PanchayatId.Contains(x.Id.ToString())).ToListAsync();
                //        if (listOfPanchayat != null)

                //            user.PanchayatName = new List<string>();
                //        foreach (var item in listOfPanchayat)
                //        {
                //            user.PanchayatName.Add(item.Name);
                //        }

                //        user.VidhanSabhaName = appDbContext.VidhanSabha.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.VidhanSabhaId).Result.Name;
                //    }

                //}

                logger.LogInformation($"UserRepository : GetAllTeachers : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllTeachers", ex);
            }
            return users;
        }

        public async Task<List<Users>> GetAllRegionalAdmins()
        {
            logger.LogInformation($"UserRepository : GetAllRegionalAdmins : Started");

            List<Users> users = new List<Users>();
            try
            {

                users = await (from u in appDbContext.Users
                               where u.Type == (int)Constant.Type.RegionalAdmin
                               select new Users
                               {
                                   Id = u.Id,
                                   Name = u.Name,
                                   Picture = u.Picture
                               }).OrderByDescending(x => x.Id).ToListAsync();


                //var test = appDbContext.Users.Include(x=>x.District)
                //                              .Include(x=>x.VidhanSabha)
                //                              .Include(x=>x.Panchayat)
                //                              .Include(x=>x.Village)
                //                              .Where(x=>x.Type==2 && x.Id==17).ToList();

                //users = await (from u in appDbContext.Users
                //               join c in appDbContext.Center
                //               on u.Id equals c.AssignedRegionalAdmin
                //               into jts
                //               from Center in jts.DefaultIfEmpty()
                //               join d in appDbContext.District
                //               on u.DistrictId equals d.Id
                //               join vid in appDbContext.VidhanSabha
                //               on u.VidhanSabhaId equals vid.Id
                //               into emp
                //               from Village in emp.DefaultIfEmpty()
                //               where u.Type == (int)Constant.Type.RegionalAdmin
                //               select new Users
                //               {
                //                   Id = u.Id,
                //                   Type = u.Type,
                //                   EnrolmentRollId = u.EnrolmentRollId,
                //                   Name = u.Name,
                //                   Age = u.Age,
                //                   Gender = u.Gender,
                //                   DateOfBirth = u.DateOfBirth,
                //                   PhoneNumber = u.PhoneNumber,
                //                   WhatsApp = u.WhatsApp,
                //                   GuardianName = u.GuardianName,
                //                   GuardianNumber = u.GuardianNumber,
                //                   DistrictId = d.Id,
                //                   DistrictName = d.Name,
                //                   VidhanSabhaId = u.VidhanSabhaId,
                //                   Education = u.Education,
                //                   PanchayatId = u.PanchayatId,
                //                   VillageId=u.VillageId,
                //                   VillageName = Village == null ? string.Empty : Village.Name,
                //                   CenterName= Center == null?string.Empty: Center.CenterName,
                //                   AssignedTeacherStatus = u.AssignedTeacherStatus,
                //                   EnrollmentDate = u.EnrollmentDate
                //               }).AsNoTracking().ToListAsync();



                //if (users != null)
                //{
                //    foreach (var user in users)
                //    {
                //        //Center center = appDbContext.Center.Where(x => x.AssignedTeachers == user.Id);
                //        List<Panchayat> listOfPanchayat = await appDbContext.Panchayat.AsNoTracking().Where(x => user.PanchayatId.Contains(x.Id.ToString())).ToListAsync();
                //        if (listOfPanchayat != null)

                //            user.PanchayatName = new List<string>();
                //        foreach (var item in listOfPanchayat)
                //        {
                //            user.PanchayatName.Add(item.Name);
                //        }

                //        user.VidhanSabhaName = appDbContext.VidhanSabha.AsNoTracking().FirstOrDefaultAsync(x => x.Id == user.VidhanSabhaId).Result.Name;
                //    }

                //}

                logger.LogInformation($"UserRepository : GetAllRegionalAdmins : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllRegionalAdmins", ex);
                throw ex;
            }
            return users;
        }

        public string GenerateToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            Claim[] claims;
            if (string.IsNullOrEmpty(user.Name))
            {
                claims = new[]
               {
                    new Claim(ClaimTypes.Email, user.Email)
                };
            }
            else
            {
                claims = new[]
                 {
                    new Claim(ClaimTypes.NameIdentifier, user.Name.ToString())
                    //new Claim(ClaimTypes.Email, masterAdmin.Email.ToString())
                  };
            }

            var token = new JwtSecurityToken(
           configuration["Jwt:Issuer"],
           configuration["Jwt:Audience"],
           claims,
           expires: DateTime.Now.AddMinutes(1440),
           signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
