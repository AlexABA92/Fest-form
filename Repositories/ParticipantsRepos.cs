using Fest_form.data;
using Fest_form.data.Entity;
using Fest_form.Interface;

namespace Fest_form.Repositories
{
    public class ParticipantsRepos(FestDataContext context) : IParticipantsNumber<ParticipantsNumber>
    {
        private readonly FestDataContext _context = context;
        public List<ParticipantsNumber> GetList() => _context.ParticipantsNumbers.ToList();
    }
}
