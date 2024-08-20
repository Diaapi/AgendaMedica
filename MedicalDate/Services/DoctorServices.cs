using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using MedicalDate.Model;

namespace MedicalDate.Services;
public class DoctorServices
{
    //Debe de agregar el link de firebase realtime en el <URL>
    FirebaseClient firebase = new FirebaseClient("<URL>");

    public async Task<List<DoctorModel>> GetAll()
    {

        return (await firebase
          .Child("Doctor")
          .OnceAsync<DoctorModel>()).Select(item => new DoctorModel
          {
              Id = item.Object.Id,
              Name = item.Object.Name,
              Email = item.Object.Email,
              Phone = item.Object.Phone,
              Specialty = item.Object.Specialty,
          }).ToList();
    }

    public async Task<List<DoctorModel>> Search(string search)
    {
        return (await firebase
          .Child("Doctor")
          .OnceAsync<DoctorModel>()).Where(x=>x.Object.Name!.ToLower().Contains(search.ToLower())).Select(item => new DoctorModel
          {
              Id = item.Object.Id,
              Name = item.Object.Name,
              Email = item.Object.Email,
              Phone = item.Object.Phone,
              Specialty = item.Object.Specialty,
          }).ToList();
    }

    public async Task Add(DoctorModel doctor)
    {
        Random rnd = new Random();
        await firebase
          .Child("Doctor")
          .PostAsync(new DoctorModel()
          {
              Id = rnd.Next(),
              Name = doctor.Name,
              Email = doctor.Email,
              Phone = doctor.Phone,
              Specialty = doctor.Specialty,
          });
    }

    public async Task DeletePerson(int id)
    {
        var toDeletePerson = (await firebase
          .Child("Doctor")
          .OnceAsync<AppointmentModel>()).Where(a => a.Object.Id == id).FirstOrDefault();
        await firebase.Child("Doctor").Child(toDeletePerson!.Key).DeleteAsync();

    }

    public async Task Update(DoctorModel doctor)
    {
        var toUpdatePerson = (await firebase
          .Child("Doctor")
          .OnceAsync<DoctorModel>()).Where(a => a.Object.Id == doctor.Id).FirstOrDefault();

        await firebase
          .Child("Doctor")
          .Child(toUpdatePerson!.Key)
          .PutAsync(new DoctorModel()
          {
              Id = doctor.Id,
              Name = doctor.Name,
              Email = doctor.Email,
              Phone = doctor.Phone,
              Specialty = doctor.Specialty,
          });
    }
}
