using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using MedicalDate.Model;

namespace MedicalDate.Services;
public class AppointmentServices
{
    //Debe de agregar el link de firebase realtime en el <URL>
    FirebaseClient firebase = new FirebaseClient("<URL>");

    public async Task<List<AppointmentModel>> GetAll()
    {

        return (await firebase
          .Child("Appointment")
          .OnceAsync<AppointmentModel>()).Select(item => new AppointmentModel
          {
              Id = item.Object.Id,
              Doctor = item.Object.Doctor,
              Patient = item.Object.Patient,
              AppointmentDate = item.Object.AppointmentDate,
              Reason = item.Object.Reason,
              Status = item.Object.Status,
          }).ToList();
    }

    public async Task<List<AppointmentModel>> Search(string name)
    {

        return (await firebase
          .Child("Appointment")
          .OnceAsync<AppointmentModel>()).Where(x => x.Object.Doctor!.ToLower().Contains(name.ToLower())|| x.Object.Patient!.ToLower().Contains(name.ToLower())).Select(item => new AppointmentModel
          {
              Id = item.Object.Id,
              Doctor = item.Object.Doctor,
              Patient = item.Object.Patient,
              AppointmentDate = item.Object.AppointmentDate,
              Reason = item.Object.Reason,
              Status = item.Object.Status,
          }).ToList();
    }

    public async Task Add(AppointmentModel appointment)
    {
        Random rnd = new Random();
        await firebase
          .Child("Appointment")
          .PostAsync(new AppointmentModel() {
              Id = rnd.Next(),
              Doctor = appointment.Doctor,
              Patient = appointment.Patient,
              AppointmentDate = appointment.AppointmentDate,
              Reason = appointment.Reason,
              Status = appointment.Status,
          });
    }

    public async Task DeletePerson(int id)
    {
        var toDeletePerson = (await firebase
          .Child("Appointment")
          .OnceAsync<AppointmentModel>()).Where(a => a.Object.Id == id).FirstOrDefault();
        await firebase.Child("Appointment").Child(toDeletePerson!.Key).DeleteAsync();

    }

    public async Task Update(AppointmentModel appointment)
    {
        var toUpdatePerson = (await firebase
          .Child("Appointment")
          .OnceAsync<AppointmentModel>()).Where(a => a.Object.Id == appointment.Id).FirstOrDefault();

        await firebase
          .Child("Appointment")
          .Child(toUpdatePerson!.Key)
          .PutAsync(new AppointmentModel() {
              Id = appointment.Id,
              Doctor = appointment.Doctor,
              Patient = appointment.Patient,
              AppointmentDate = appointment.AppointmentDate,
              Reason = appointment.Reason,
              Status = appointment.Status,
          });
    }
}
