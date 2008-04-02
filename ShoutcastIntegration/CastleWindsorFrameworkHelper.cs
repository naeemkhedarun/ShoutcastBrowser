using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace ShoutcastIntegration
{
    public class CastleWindsorFrameworkHelper
    {
        private static readonly IWindsorContainer _windsorContainer;

        static CastleWindsorFrameworkHelper()
        {
            _windsorContainer = new WindsorContainer(new XmlInterpreter());
        }

        public static T New<T>()
        {
            return _windsorContainer.Resolve<T>();
        }
    }
}