using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using MedicalDate.Model;

namespace MedicalDate.Services;
public class PrescriptionServices
{
    //Debe de agregar el link de firebase realtime en el <URL>
    FirebaseClient firebase = new FirebaseClient("<URL>");

    public async Task<List<PrescriptionModel>> GetAll()
    {

        return (await firebase
          .Child("Receta")
          .OnceAsync<PrescriptionModel>()).Select(item => new PrescriptionModel
          {
              Id = item.Object.Id,
              Dosage = item.Object.Dosage,
              Instructions = item.Object.Instructions,
              Medication = item.Object.Medication,
              PatientName = item.Object.PatientName,
          }).ToList();
    }

    public async Task<List<PrescriptionModel>> Search(string name)
    {

        return (await firebase
          .Child("Receta")
          .OnceAsync<PrescriptionModel>()).Where(x => x.Object.PatientName!.ToLower().Contains(name.ToLower())).Select(item => new PrescriptionModel
          {
              Id = item.Object.Id,
              Dosage = item.Object.Dosage,
              Instructions = item.Object.Instructions,
              Medication = item.Object.Medication,
              PatientName = item.Object.PatientName,
          }).ToList();
    }

    public async Task Add(PrescriptionModel prescription)
    {
        Random rnd = new Random();
        await firebase
          .Child("Receta")
          .PostAsync(new PrescriptionModel()
          {
              Id = rnd.Next(),
              Dosage = prescription.Dosage,
              Instructions = prescription.Instructions,
              Medication = prescription.Medication,
              PatientName = prescription.PatientName,
          });
    }

    public async Task DeletePerson(int id)
    {
        var toDeletePerson = (await firebase
          .Child("Receta")
          .OnceAsync<PrescriptionModel>()).Where(a => a.Object.Id == id).FirstOrDefault();
        await firebase.Child("Receta").Child(toDeletePerson!.Key).DeleteAsync();

    }

    public async Task Update(PrescriptionModel prescription)
    {
        var toUpdatePerson = (await firebase
          .Child("Receta")
          .OnceAsync<PrescriptionModel>()).Where(a => a.Object.Id == prescription.Id).FirstOrDefault();

        await firebase
          .Child("Receta")
          .Child(toUpdatePerson!.Key)
          .PutAsync(new PrescriptionModel()
          {
              Id = prescription.Id,
              Dosage = prescription.Dosage,
              Instructions = prescription.Instructions,
              Medication = prescription.Medication,
              PatientName = prescription.PatientName,
          });
    }
}
