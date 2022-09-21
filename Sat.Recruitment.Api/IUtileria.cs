using Sat.Recruitment.Api.Controllers;

namespace Sat.Recruitment.Api
{
    public interface IUtileria
    {
        void ValidateErrors(User user, ref string errors);
    }
}