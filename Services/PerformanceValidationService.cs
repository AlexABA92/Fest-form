using Fest_form.data.Entity;
using Fest_form.Interface;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.Collections.Generic;

namespace Fest_form.Services
{
    public class PerformanceValidationService : IPerformanceValidationService
    {

        public Dictionary<string,string>? validationPerformance(Performance performance, int index)
        {
            var errors = new Dictionary<string, string>();
            
            
            if (string.IsNullOrWhiteSpace(performance.PhonogramFileURL)) {
                errors.Add( $"Performances_{index}_File", Resources.Resource.RequiredErrorMessage);
            }


            return null;    
        }
    

       
    }

   
}
