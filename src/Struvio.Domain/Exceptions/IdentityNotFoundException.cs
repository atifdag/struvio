

namespace Struvio.Domain.Exceptions;


public class IdentityNotFoundException : BaseApplicationException
{
    public IdentityNotFoundException() : base(LanguageTexts.IdentityUserNotFound)
    {
    }

    public IdentityNotFoundException(string message) : base(message)
    {

    }

}
