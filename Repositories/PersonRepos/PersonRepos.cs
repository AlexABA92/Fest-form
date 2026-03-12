using Fest_form.data;
using Fest_form.data.Entity;

using System.Reflection;
using System.Text.RegularExpressions;

namespace Fest_form.Repositories.PersonRepos
{
    public class PersonRepos(FestDataContext context, ILogger<PersonRepos> logger) : IPersonRepos<Person>
    {
        private readonly FestDataContext _context = context;
        private readonly ILogger<PersonRepos> _logger = logger;
        public List<Person> GetPersonList()
        {
            return _context.Persons.ToList();
        }
        public Person? CheckPerson(Person person, List<Person> list)
        {
            Person? returnPerson = null;
            if (!string.IsNullOrEmpty(person.PersonFatherName))
                returnPerson = list.FirstOrDefault(_person => _person.PersonName == person.PersonName
                && _person.PersonLastName == person.PersonLastName
                && _person.PersonFatherName == person.PersonFatherName);
            else {
                returnPerson = list.FirstOrDefault(_person => _person.PersonName == person.PersonName
                && _person.PersonLastName == person.PersonLastName);
            }
            if (returnPerson != null)
            {
                return returnPerson;
            }
            return null;
        }
        private Dictionary<string, Person> GetAllPersonObject(object obj, string parentKey = "")
        {
            var result = new Dictionary<string, Person>();

            if (obj == null) return result;

            if (obj is IEnumerable<Performance> performances)
            {
                int index = 0;
                foreach (var performance in performances)
                {
                    parentKey = $"Performances[{index}]";
                    result = MergeDictionaries(result, GetAllPersonObject(performance, parentKey));
                    index++;
                }
            }
            else
            {

                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    var value = property.GetValue(obj);

                    if (value == null) continue;

                    var propetryKay = string.IsNullOrEmpty(parentKey) ? property.Name : $"{parentKey}.{property.Name}";
                    if (property.PropertyType == typeof(Person))
                    {
                        result[propetryKay] = (Person)value;
                    }
                    else if (property.PropertyType == typeof(List<Participant>))
                    {
                        result = MergeDictionaries(result, (GetAllPersonObject(value, propetryKay)));
                    }
                    else if (property.PropertyType == typeof(List<Performance>))
                    {
                        result = MergeDictionaries(result, (GetAllPersonObject(value, propetryKay)));
                    }

                }
            }
            return result;
        }

        public void CheckTeamForPerson(ref DanceTeam team, ref List<Person> list)
        {
            try
            {
                var people = GetAllPersonObject(team);

                foreach (var person in people)
                {
                    var isAdded = CheckPerson(person.Value, list);
                    if (isAdded == null)
                    {
                        list.Add(person.Value);
                    }
                    else
                    {
                        if (isAdded.PersonId != Guid.Empty)
                        {
                            SetPropertyValue(team, person.Key, isAdded);
                            _context.Attach(isAdded);
                        }
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "CheckTeamForPerson  ArgumentNullException Error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckTeamForPerson Error");
            }
        }
        private static void SetPropertyValue(object obj, string propertyPath, object value)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (string.IsNullOrEmpty(propertyPath)) throw new ArgumentNullException(nameof(propertyPath));

            var parts = propertyPath.Split('.');

            object currentObject = obj;

            PropertyInfo? property = null;
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];

                Match match = Regex.Match(part, @"(\w+)\[(\d+)\]");

                if (match.Success)
                {
                    string collectionName = match.Groups[1].Value; // Collection name (e.g., Performances)

                    int index = int.Parse(match.Groups[2].Value);   // Index (e.g., 0)

                    property = currentObject.GetType().GetProperty(collectionName);
                    if (property == null)
                        throw new ArgumentException($"Property '{collectionName}' not found.");
                    var collection = property.GetValue(currentObject) as System.Collections.IList;
                    if (collection == null)
                        throw new ArgumentException($"'{collectionName}' is not a collection.");
                    // Navigate to the indexed object
                    currentObject = collection[index];
                }
                else {
                    // Handle regular properties
                    property = currentObject.GetType().GetProperty(part);
                    if (property == null)
                        throw new ArgumentException($"Property '{part}' not found.");

                    if (i == parts.Length - 1)
                    {
                        // Final property in the path, set its value
                        property.SetValue(currentObject, value);
                    }
                    else
                    {
                        // Navigate to the next nested object
                        currentObject = property.GetValue(currentObject);
                    }

                }
            }
        }
            static Dictionary<string, Person> MergeDictionaries(Dictionary<string, Person> dict1, Dictionary<string, Person> dict2)
            {
                foreach (var kvp in dict2)
                {
                    dict1[kvp.Key] = kvp.Value;
                }

                return dict1;
            }


        
    }
}