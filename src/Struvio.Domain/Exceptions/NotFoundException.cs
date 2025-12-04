namespace Struvio.Domain.Exceptions;

public class NotFoundException : BaseApplicationException
{
    public NotFoundException() : base(LanguageTexts.RecordNotFound)
    {
    }

    public NotFoundException(string message) : base(message)
    {

    }

}
