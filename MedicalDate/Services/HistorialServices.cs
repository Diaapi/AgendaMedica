using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using MedicalDate.Model;

namespace MedicalDate.Services;
public class HistorialServices
{
    //Debe de agregar el link de firebase realtime en el <URL>
    FirebaseClient firebase = new FirebaseClient("<URL>");

    public async Task<List<HistorialModel>> GetAll()
    {

        return (await firebase
          .Child("Historial")
          .OnceAsync<HistorialModel>()).Select(item => new HistorialModel
          {
              Id = item.Object.Id,
              DoctorName = item.Object.DoctorName,
              PatientName = item.Object.PatientName,
              Diagnosis = item.Object.Diagnosis,
              VisitDate = item.Object.VisitDate,
              Notes = item.Object.Notes,
              Treatment = item.Object.Treatment,
          }).ToList();
    }

    public async Task<List<HistorialModel>> Search(string name)
    {

        return (await firebase
          .Child("Historial")
          .OnceAsync<HistorialModel>()).Where(x => x.Object.PatientName!.ToLower().Contains(name.ToLower())|| x.Object.DoctorName!.ToLower().Contains(name.ToLower())).Select(item => new HistorialModel
          {
              Id = item.Object.Id,
              DoctorName = item.Object.DoctorName,
              PatientName = item.Object.PatientName,
              Diagnosis = item.Object.Diagnosis,
              VisitDate = item.Object.VisitDate,
              Notes = item.Object.Notes,
              Treatment = item.Object.Treatment,
          }).ToList();
    }

    public async Task Add(HistorialModel historial)
    {
        Random rnd = new Random();
        await firebase
          .Child("Historial")
          .PostAsync(new HistorialModel()
          {
              Id = rnd.Next(),
              PatientName = historial.PatientName,
              DoctorName = historial.DoctorName,
              Diagnosis = historial.Diagnosis,
              VisitDate = historial.VisitDate,
              Notes = historial.Notes,
              Treatment = historial.Treatment,
          });
    }

    public async Task DeletePerson(int id)
    {
        var toDeletePerson = (await firebase
          .Child("Historial")
          .OnceAsync<HistorialModel>()).Where(a => a.Object.Id == id).FirstOrDefault();
        await firebase.Child("Historial").Child(toDeletePerson!.Key).DeleteAsync();

    }

    public async Task Update(HistorialModel historial)
    {
        var toUpdatePerson = (await firebase
          .Child("Historial")
          .OnceAsync<HistorialModel>()).Where(a => a.Object.Id == historial.Id).FirstOrDefault();

        await firebase
          .Child("Historial")
          .Child(toUpdatePerson!.Key)
          .PutAsync(new HistorialModel()
          {
              Id = historial.Id,
              PatientName = historial.PatientName,
              DoctorName = historial.DoctorName,
              Diagnosis = historial.Diagnosis,
              VisitDate = historial.VisitDate,
              Notes = historial.Notes,
              Treatment = historial.Treatment,
          });
    }
}
