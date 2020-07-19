using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ProiectDaw.Models;

[assembly: OwinStartupAttribute(typeof(ProiectDaw.Startup))]
namespace ProiectDaw
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createAdminUserAndApplicationRoles();
        }

        private void createAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new
            RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new
            UserStore<ApplicationUser>(context));
            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Admin"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                var adminCreated = UserManager.Create(user, "~1AdminDaw1~");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                }
            }
            /*
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
            */
            if (!roleManager.RoleExists("Registred User"))
            {
                var role = new IdentityRole();
                role.Name = "Registred User";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "defaultU@gmail.com";
                user.Email = "defaultU@gmail.com";
                var RegUserCreated = UserManager.Create(user, "~1DefaultUDaw1~");
                if (RegUserCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Registred User");
                }
            }
                if (!roleManager.RoleExists("Collaborator"))
            {
                var role = new IdentityRole();
                role.Name = "Collaborator";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "defaultC@gmail.com";
                user.Email = "defaultC@gmail.com";
                var RegUserCreated = UserManager.Create(user, "~1DefaultCDaw1~");
                if (RegUserCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Collaborator");
                }
            }
        }

    }
}
