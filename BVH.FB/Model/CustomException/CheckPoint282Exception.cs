using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BVH.FB.Model.CustomException
{
    [Serializable]
    public class CheckPoint282Exception : Exception
    {
    }

    [Serializable]
    public class UnrecognizeMainPageException : Exception
    {
    }

    [Serializable]
    public class WrongUsernameAndPasswordException : Exception
    {
    }

    [Serializable]
    public class CreatePageException : Exception
    {
    }

    [Serializable]
    public class NotConfigTwoFactorException : Exception
    {
    }
}
