using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalDate.Model;
public class HistorialModel
{
    public int Id { get; set; }
    public string? PatientName { get; set; }
    public string? DoctorName { get; set; }
    public string? VisitDate { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Notes { get; set; }
}
