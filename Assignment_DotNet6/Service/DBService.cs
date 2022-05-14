
using Assignment_DotNet6.Data;
using Assignment_DotNet6.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assignment_DotNet6.Service
{
    public class DBService
    {

        #region propertise
        private UserManager<Doctor> userManager;
        private DataContext dataContext;
        //private ConfigurationManager configuration;
        #endregion

        public DBService(DataContext dataContext, UserManager<Doctor> userManager)
        {
            this.userManager = userManager;
            this.dataContext = dataContext;
        }

        public async Task<UserAuthenticationResponse> AuthenticateUser(LoginDetails model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                if (!(await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    return new UserAuthenticationResponse()
                    {
                        AuthenticationStatus = UserAuthenticationStatus.InvalidPassword
                    };
                }


                var _optionsIdentity = new IdentityOptions();
                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };


                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("x323423@$%@pyw349as@"));

                try
                {
                    var token = new JwtSecurityToken(
                                expires: DateTime.Now.AddMinutes(5),
                                signingCredentials: (new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256))
                                );


                    return new UserAuthenticationResponse()
                    {
                        TokenResponse = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        },
                        AuthenticationStatus = UserAuthenticationStatus.Authenticated,
                        userDetail = user
                    };
                }
                catch (Exception ex)
                {

                }
            }
            return new UserAuthenticationResponse()
            {
                AuthenticationStatus = UserAuthenticationStatus.NotFound
            };
        }

        #region Doctors CRUD
        public async Task<bool> CreateDoctor(Doctor doctor)
        {
            doctor.Id = Guid.NewGuid().ToString();
            doctor.D_ID = Guid.NewGuid();
            dataContext.Doctors.Add(doctor);
            dataContext.SaveChanges();

            return false;
        }
        public async Task<IEnumerable<Doctor>> GetAllDoctors()
        {
            return dataContext.Doctors;
        }
        public async Task<IEnumerable<Doctor>> GetDoctorById(Guid Id)
        {
            return dataContext.Doctors.Where(docs => docs.D_ID == Id).AsQueryable();
        }
        public async Task<bool> UpdateDoctorById(Doctor doctor)
        {
            var doc = dataContext.Doctors.Where(docs => docs.D_ID == doctor.D_ID).AsQueryable().FirstOrDefault();

            if (doc != null)
            {
                doctor.Id = doc.Id;
                doctor.D_ID = doc.D_ID;

                dataContext.Entry(doc).CurrentValues.SetValues(doctor);
                dataContext.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteDoctorById(Guid Id)
        {
            var doc = dataContext.Doctors.Where(docs => docs.D_ID == Id).AsQueryable().FirstOrDefault();

            if (doc != null)
            {
                dataContext.Remove(doc);
                dataContext.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion

        #region Patient CRUD
        public async Task<bool> CreatePatient(Patient patient)
        {
            patient.Id = Guid.NewGuid().ToString();
            patient.P_ID = Guid.NewGuid();
            dataContext.Patients.Add(patient);
            dataContext.SaveChanges();
            return false;
        }
        public async Task<IEnumerable<Patient>> GetAllPatients()
        {
            return dataContext.Patients;
        }
        public async Task<IEnumerable<Patient>> GetPatientById(Guid Id)
        {
            return dataContext.Patients.Where(docs => docs.P_ID == Id).AsQueryable();
        }
        public async Task<bool> UpdatePatientById(Patient patients)
        {
            var pat = dataContext.Patients.Where(pats => pats.P_ID == patients.P_ID).AsQueryable().FirstOrDefault();

            if (pat != null)
            {
                patients.Id = pat.Id;
                patients.P_ID = pat.P_ID;

                dataContext.Entry(pat).CurrentValues.SetValues(patients);
                dataContext.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> DeletePatientById(Guid Id)
        {
            var _Pat = dataContext.Patients.Where(docs => docs.P_ID == Id).AsQueryable().FirstOrDefault();

            if (_Pat != null)
            {
                dataContext.Remove(_Pat);
                dataContext.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region Visit CRUD
        public async Task<bool> CreateVisit(Visit Visit)
        {
            dataContext.Visits.Add(Visit);
            dataContext.SaveChanges();
            return false;
        }
        public async Task<IEnumerable<Visit>> GetAllVisit()
        {
            return dataContext.Visits;
        }
        public async Task<IEnumerable<Visit>> GetVisitsByPatientId(Guid Id)
        {
            return dataContext.Visits.Where(docs => docs.PatientId == Id).AsQueryable();
        }

        public async Task<IEnumerable<Visit>> GetVisitsByDoctorId(Guid Id)
        {
            return dataContext.Visits.Where(docs => docs.DoctorId == Id).AsQueryable();
        }

        public async Task<bool> UpdateVisitById(Visit Visit)
        {
            var pat = dataContext.Visits.Where(pats => pats.ID == Visit.ID).AsQueryable().FirstOrDefault();

            if (pat != null)
            {
                Visit.ID = pat.ID;

                dataContext.Entry(pat).CurrentValues.SetValues(Visit);
                dataContext.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteVisitById(Guid Id)
        {
            var _Pat = dataContext.Visits.Where(docs => docs.ID == Id).AsQueryable().FirstOrDefault();

            if (_Pat != null)
            {
                dataContext.Remove(_Pat);
                dataContext.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion
    }
}
