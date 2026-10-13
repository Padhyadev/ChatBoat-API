using Microsoft.EntityFrameworkCore;

namespace Chatboat.Infra
{
    public class Datacontext : DbContext
    {
        public Datacontext(DbContextOptions<Datacontext> options) : base(options) { }
    }
}
