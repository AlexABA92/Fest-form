using Fest_form.data.Entity;

namespace Fest_form.Interface
{
    public interface IPerformanceValidationService
    {
        public Dictionary<string, string>? validationPerformance(Performance performance, int index);
       
        
    }
}
