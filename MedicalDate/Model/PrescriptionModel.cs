using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalDate.Model;
public class PrescriptionModel
{
    public int Id { get; set; }
    public string? PatientName { get; set; }
    public string? Medication { get; set; }
    public string? Dosage { get; set; }
    public string? Instructions { get; set; }
}
