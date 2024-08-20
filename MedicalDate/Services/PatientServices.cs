using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using MedicalDate.Model;

namespace MedicalDate.Services;
public class PatientServices
{
    //Debe de agregar el link de firebase realtime en el <URL>
    FirebaseClient firebase = new FirebaseClient("<URL>");

    public async Task<List<PatientModel>> GetAll()
    {

        return (await firebase
          .Child("Paciente")
          .OnceAsync<PatientModel>()).Select(item => new PatientModel
          {
              Id = item.Object.Id,
              Name = item.Object.Name,
              Address = item.Object.Address,
              Dateofbirth = item.Object.Dateofbirth,
              Email = item.Object.Email,
              Phone = item.Object.Phone,
          }).ToList();
    }

    public async Task<List<PatientModel>> Search(string name)
    {

        return (await firebase
          .Child("Paciente")
          .OnceAsync<PatientModel>()).Where(x=>x.Object.Name!.ToLower().Contains(name.ToLower())).Select(item => new PatientModel
          {
              Id = item.Object.Id,
              Name = item.Object.Name,
              Address = item.Object.Address,
              Dateofbirth = item.Object.Dateofbirth,
              Email = item.Object.Email,
              Phone = item.Object.Phone,
          }).ToList();
    }

    public async Task Add(PatientModel patient)
    {
        Random rnd = new Random();
        await firebase
          .Child("Paciente")
          .PostAsync(new PatientModel()
          {
              Id = rnd.Next(),
              Name = patient.Name,
              Address = patient.Address,
              Dateofbirth = patient.Dateofbirth,
              Email = patient.Email,
              Phone = patient.Phone,
          });
    }

    public async Task DeletePerson(int id)
    {
        var toDeletePerson = (await firebase
          .Child("Paciente")
          .OnceAsync<PatientModel>()).Where(a => a.Object.Id == id).FirstOrDefault();
        await firebase.Child("Paciente").Child(toDeletePerson!.Key).DeleteAsync();

    }

    public async Task Update(PatientModel patient)
    {
        var toUpdatePerson = (await firebase
          .Child("Paciente")
          .OnceAsync<PatientModel>()).Where(a => a.Object.Id == patient.Id).FirstOrDefault();

        await firebase
          .Child("Paciente")
          .Child(toUpdatePerson!.Key)
          .PutAsync(new PatientModel()
          {
              Id = patient.Id,
              Name = patient.Name,
              Address = patient.Address,
              Dateofbirth = patient.Dateofbirth,
              Email = patient.Email,
              Phone = patient.Phone,
          });
    }
}
