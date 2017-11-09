using Ninject.Modules;
using MOUNB.BLL.Services;
using MOUNB.BLL.Interfaces;

namespace MOUNB.WEB.Util
{
    public class PLModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
        }
    }
}