
using testexamination.Entity;
using testexamination.Repo;

namespace ExaminationSystem.Infrastructure.Repo
{
    public class QuestionRepository : GenericRepository<Questions>, IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Questions?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Questions
                .Where(q => !q.IsDeleted)
                .Include(q => q.Options)
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        }

        public async Task<List<Questions>> GetByQuizIdAsync(Guid quizId, CancellationToken cancellationToken = default)
        {
            return await _context.Questions
                .Where(q => !q.IsDeleted && q.QuizId == quizId)
                .Include(q => q.Options)
                .OrderBy(q => q.OrderIndex)
                .ToListAsync(cancellationToken);
        }
    }
}