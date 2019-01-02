using System;

namespace AlignTag
{
  public class ErrorMessageException : ApplicationException
  {
    public ErrorMessageException()
    {
    }

    public ErrorMessageException(string message)
      : base(message)
    {
    }
  }
}
