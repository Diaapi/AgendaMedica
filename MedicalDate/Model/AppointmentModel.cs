using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalDate.Model;
public class AppointmentModel
{
    public int Id { get; set; }
    public string? Doctor { get; set; }
    public string? Patient { get; set; }
    public string? AppointmentDate { get; set; }
    public string? Reason { get; set; }
    public string? Status { get; set; }
}
