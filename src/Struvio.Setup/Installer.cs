using Struvio.Persistence;

namespace Struvio.Setup
{
    public static class Installer
    {
        public static void Install(ApplicationDbContext context)
        {

        }

        public static bool IsInstalled(ApplicationDbContext dbContext)
        {
            return dbContext.Users.Any();
        }
    }
}
