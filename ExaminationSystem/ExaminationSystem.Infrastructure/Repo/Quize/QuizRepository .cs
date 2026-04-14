
using System;
using testexamination.Entity;
using testexamination.Repo;

namespace ExaminationSystem.Infrastructure.Repo
{
    public class QuizRepository : GenericRepository<Quize>, IQuizRepository
    {
        private readonly AppDbContext _context;

        public QuizRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Quize?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Quizzes
                .Where(q => !q.IsDeleted)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Options)
                .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        public async Task<List<Quize>> GetByDiplomaIdAsync(Guid diplomaId, CancellationToken cancellationToken = default)
        {
            return await _context.Quizzes
                .Where(q => !q.IsDeleted && q.DiplomaId == diplomaId)
                .ToListAsync(cancellationToken);
        }
    }
}