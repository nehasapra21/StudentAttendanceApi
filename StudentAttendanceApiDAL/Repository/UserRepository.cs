using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext appDbContext;
        private readonly ILogger logger;

        public UserRepository(AppDbContext appDbContext, ILogger<UserRepository> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.appDbContext = appDbContext;
            this.logger = logger;
            this.configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<string> GetUserDeviceByUserId(int userId)
        {
            logger.LogInformation($"UserRepository : GetUserById : Started");

            try
            {
                Users user = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    return user.DeviceId;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetUserById", ex);
                throw ex;
            }
            return string.Empty;
        }

        //public async Task<string> GetUserTokenByUserId(int userId)
        //{
        //    logger.LogInformation($"UserRepository : GetUserTokenByUserId : Started");

        //    string userToken = string.Empty;
        //    Task<Users> user = new Task<Users>();
        //    try
        //    {
        //        user = await appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId).Result;

        //        if (user != null)
        //        {
        //            userToken = user.Token;
        //        }

        //        logger.LogInformation($"UserRepository : GetUserById : End");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError(ex, $"UserRepository : GetUserById", ex);
        //        throw ex;
        //    }
        //    return userToken;
        //}

        public async Task<List<string>> GetAllSuperAdminToken()
        {
            logger.LogInformation($"UserRepository : GetUserTokenByUserId : Started");

            List<string> superAdminToken = null;
            List<Users> user = new List<Users>();
            try
            {
                superAdminToken = appDbContext.Users.AsNoTracking().Where(x => x.Type == 1).Select(x => x.DeviceId).ToList();

                if (superAdminToken != null && superAdminToken.Count > 0)
                {
                    foreach (var item in superAdminToken)
                    {
                        if (item != null)
                        {
                            superAdminToken=new List<string>();
                            superAdminToken.Add(item);
                        }

                    }
                    return superAdminToken;
                }

                logger.LogInformation($"UserRepository : GetUserById : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetUserById", ex);
                throw ex;
            }
            return null;
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
                    return user;
                }

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
                //await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                user = await appDbContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == userName && x.Password == password);
                if (user != null)
                {
                    user.LastLoginTime = Convert.ToString(DateTime.Now);
                    appDbContext.Update(user);
                    await appDbContext.SaveChangesAsync();
                    ///Generate token for user
                    #region JWT
                    user.Token = CommonUtility.GenerateToken(configuration, user.Email, user.Name);
                    #endregion

                    //save data
                    //     var claims = new List<Claim>
                    //         {
                    //             new Claim("UserId", user.Id.ToString()),
                    //             new Claim("UserName", user.Name)
                    //         };

                    //     var claimsIdentity = new ClaimsIdentity(
                    //        claims,
                    //        CookieAuthenticationDefaults.AuthenticationScheme);

                    //     await _httpContextAccessor.HttpContext.SignInAsync(
                    //CookieAuthenticationDefaults.AuthenticationScheme,
                    //new ClaimsPrincipal(claimsIdentity)

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


        public async Task<Users> UpdateDeviceId(int userId, string deviceId)
        {
            logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");
            Users userVal = null;
            try
            {
                userVal = appDbContext.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (userVal != null)
                {
                    userVal.DeviceId = deviceId;
                    appDbContext.Update(userVal);
                    await appDbContext.SaveChangesAsync();
                }

                logger.LogInformation($"UserRepository : SaveSuperAdmin : Started");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SaveSuperAdmin ", ex);
                throw ex;
            }
            return userVal;
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

        public async Task<List<Users>> GetAllTeachers(int userId)
        {
            logger.LogInformation($"UserRepository : GetRegisteredTeachers : Started");

            List<Users> users = new List<Users>();

            try
            {
                if (userId == 0)
                {
                    users = await (from u in appDbContext.Users.Where(x => x.Type == 3 && x.Status.Value)
                                   select new Users
                                   {
                                       Id = u.Id,
                                       Name = u.Name,
                                       AssignedTeacherStatus = u.AssignedTeacherStatus,
                                       PhoneNumber=u.PhoneNumber
                                   }).OrderBy(x => x.Name).ToListAsync();
                }
                else
                {
                    List<int> userIds=appDbContext.Center.Where(x => x.AssignedRegionalAdmin == userId).Select(x=>x.AssignedTeachers.Value).ToList();

                    users = await (from u in appDbContext.Users.Where(x => x.Type == 3 && x.Status.Value)
                                   select new Users
                                   {
                                       Id = u.Id,
                                       Name = u.Name,
                                       AssignedTeacherStatus = u.AssignedTeacherStatus,
                                          PhoneNumber = u.PhoneNumber
                                   }).OrderBy(x => x.Name).ToListAsync();
                    users = users.Where(x => userIds.Contains(x.Id)).ToList();
                }
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
                users = await (from u in appDbContext.Users.Where(x => x.AssignedTeacherStatus == false && x.Type == 3 && x.Status.Value)
                               select new Users
                               {
                                   Id = u.Id,
                                   Name = u.Name,
                                   Picture = u.Picture,
                                   AssignedTeacherStatus = u.AssignedTeacherStatus,
                                   PhoneNumber = u.PhoneNumber
                               }).OrderByDescending(x => x.Id).ToListAsync();


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


                logger.LogInformation($"UserRepository : GetAllRegionalAdmins : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : GetAllRegionalAdmins", ex);
                throw ex;
            }
            return users;
        }

        public async Task<List<object>> SearchData(string type, string queryString)
        {
            logger.LogInformation($"UserRepository : SearchData : Started");

            List<object> objectList = new List<object>();
            try
            {
                if (type == "Users")
                {
                    List<Users> users = await appDbContext.Users.Where(x => x.Name.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = users.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "Student")
                {
                    List<Student> users = await appDbContext.Student.Where(x => x.FullName.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = users.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "District")
                {
                    List<District> district = await appDbContext.District.Where(x => x.Name.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = district.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "Panchayat")
                {
                    List<Panchayat> Panchayat = await appDbContext.Panchayat.Where(x => x.Name.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = Panchayat.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "VidhanSabha")
                {
                    List<VidhanSabha> vidhanSabha = await appDbContext.VidhanSabha.Where(x => x.Name.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = vidhanSabha.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "Village")
                {
                    List<Village> village = await appDbContext.Village.Where(x => x.Name.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = village.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "Center")
                {
                    List<Center> center = await appDbContext.Center.Where(x => x.CenterName.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = center.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "School")
                {
                    List<School> school = await appDbContext.School.Where(x => x.SchoolName.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = school.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "Class")
                {
                    List<Class> classs = await appDbContext.Class.Where(x => x.Name.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = classs.Select(user => (object)user).ToList();
                    //users
                }
                else if (type == "Teacher")
                {
                    List<Teacher> teacher = await appDbContext.Teacher.Where(x => x.FullName.Contains(queryString)).AsNoTracking().ToListAsync();
                    objectList = teacher.Select(user => (object)user).ToList();
                    //users
                }
                else
                {
                    return objectList;
                }
                logger.LogInformation($"UserRepository : SearchData : End");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"UserRepository : SearchData", ex);
                throw ex;
            }
            return objectList;
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
